using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

[ExecuteInEditMode]
public class ToonGrassOperation_URP : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ToonGrassPainter_URP grassPainter = default;
    [SerializeField] private Mesh sourceMesh = default;
    [SerializeField] private Material material = default;
    [SerializeField] private ComputeShader computeShader = default;

    
    [Header("Blade")]
    public float grassHeight = 1;
    public float grassWidth = 0.06f;
    public float grassRandomHeight = 0.25f;
    [Range(0, 1)] public float bladeRadius = 0.6f;
    [Range(0, 1)] public float bladeForwardAmount = 0.38f;
    [Range(1, 5)] public float bladeCurveAmount = 2;

    [Header("Wind")]
    public float windSpeed = 10;
    public float windStrength = 0.05f;

    [Header("Interactor")]
    public float affectRadius = 0.3f;
    public float affectStrength = 5;

    [Header("LOD")]
    public float minFadeDistance = 40;
    public float maxFadeDistance = 60;

    [Header("Material")]
    public Color topTint = new Color(1, 1, 1);
    public Color bottomTint = new Color(0, 0, 1);
    public float ambientStrength = 0.1f;

    [Header("Other")]
    public UnityEngine.Rendering.ShadowCastingMode castShadow;

    private Camera m_MainCamera;

    private readonly int m_AllowedBladesPerVertex = 6;
    private readonly int m_AllowedSegmentsPerBlade = 7;

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind
        .Sequential)]
    private struct SourceVertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
        public Vector3 color;
    }

    private bool m_Initialized;
    private ComputeBuffer m_SourceVertBuffer;
    private ComputeBuffer m_DrawBuffer;
    private ComputeBuffer m_ArgsBuffer;
    private ComputeShader m_InstantiatedComputeShader;
    private Material m_InstantiatedMaterial;
    private int m_IdGrassKernel;
    private int m_DispatchSize;
    private Bounds m_LocalBounds;
    private Camera sceneCam;

    private const int SOURCE_VERT_STRIDE = sizeof(float) * (3 + 3 + 2 + 3);
    private const int DRAW_STRIDE = sizeof(float) * (3 + (3 + 2 + 3) * 3);
    private const int INDIRECT_ARGS_STRIDE = sizeof(int) * 4;

    private int[] argsBufferReset = new int[] { 0, 1, 0, 0 };

#if UNITY_EDITOR
    SceneView view;

    void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnScene;
        SceneView.duringSceneGui += this.OnScene;
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnScene;
    }

    void OnScene(SceneView scene)
    {
        view = scene;

    }

#endif
    private void OnValidate()
    {
        m_MainCamera = Camera.main;
        grassPainter = GetComponent<ToonGrassPainter_URP>();
        sourceMesh = GetComponent<MeshFilter>().sharedMesh;  
    }

    private void OnEnable()
    {
        OnValidate();
        if (m_Initialized)
        {
            OnDisable();
        }
#if UNITY_EDITOR
        SceneView.duringSceneGui += this.OnScene;
#endif
        m_MainCamera = Camera.main;

        if (grassPainter == null || sourceMesh == null || computeShader == null || material == null)
        {
            return;
        }
        sourceMesh = GetComponent<MeshFilter>().sharedMesh;

        if (sourceMesh.vertexCount == 0)
        {
            return;
        }

        m_Initialized = true;

        m_InstantiatedComputeShader = Instantiate(computeShader);
        m_InstantiatedMaterial = Instantiate(material);

        Vector3[] positions = sourceMesh.vertices;
        Vector3[] normals = sourceMesh.normals;
        Vector2[] uvs = sourceMesh.uv;
        Color[] colors = sourceMesh.colors;

        SourceVertex[] vertices = new SourceVertex[positions.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Color color = colors[i];
            vertices[i] = new SourceVertex()
            {
                position = positions[i],
                normal = normals[i],
                uv = uvs[i],
                color = new Vector3(color.r, color.g, color.b) 
            };
        }

        int numSourceVertices = vertices.Length;

        int maxBladesPerVertex = Mathf.Max(1, m_AllowedBladesPerVertex);
        int maxSegmentsPerBlade = Mathf.Max(1, m_AllowedSegmentsPerBlade);
        int maxBladeTriangles = maxBladesPerVertex * ((maxSegmentsPerBlade - 1) * 2 + 1);

        m_SourceVertBuffer = new ComputeBuffer(vertices.Length, SOURCE_VERT_STRIDE,
            ComputeBufferType.Structured, ComputeBufferMode.Immutable);
        m_SourceVertBuffer.SetData(vertices);

        m_DrawBuffer = new ComputeBuffer(numSourceVertices * maxBladeTriangles, DRAW_STRIDE,
            ComputeBufferType.Append);
        m_DrawBuffer.SetCounterValue(0);

        m_ArgsBuffer =
            new ComputeBuffer(1, INDIRECT_ARGS_STRIDE, ComputeBufferType.IndirectArguments);

        m_IdGrassKernel = m_InstantiatedComputeShader.FindKernel("Main");

        m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_SourceVertices",
            m_SourceVertBuffer);
        m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_DrawTriangles", m_DrawBuffer);
        m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_IndirectArgsBuffer",
            m_ArgsBuffer);
     
        m_InstantiatedComputeShader.SetInt("_NumSourceVertices", numSourceVertices);
        m_InstantiatedComputeShader.SetInt("_MaxBladesPerVertex", maxBladesPerVertex);
        m_InstantiatedComputeShader.SetInt("_MaxSegmentsPerBlade", maxSegmentsPerBlade);

        m_InstantiatedMaterial.SetBuffer("_DrawTriangles", m_DrawBuffer);

        m_InstantiatedMaterial.SetColor("_TopTint", topTint);
        m_InstantiatedMaterial.SetColor("_BottomTint", bottomTint);
        m_InstantiatedMaterial.SetFloat("_AmbientStrength", ambientStrength);


        m_InstantiatedComputeShader.GetKernelThreadGroupSizes(m_IdGrassKernel,
            out uint threadGroupSize, out _, out _);
        m_DispatchSize = Mathf.CeilToInt((float)numSourceVertices / threadGroupSize);

        m_LocalBounds = sourceMesh.bounds;
        m_LocalBounds.Expand(Mathf.Max(grassHeight + grassRandomHeight, grassWidth));

        SetGrassDataBase();
    }

    private void OnDisable()
    {
        if (m_Initialized)
        {
            if (Application.isPlaying)
            {
                Destroy(m_InstantiatedComputeShader);
                Destroy(m_InstantiatedMaterial);
            }
            else
            {
                DestroyImmediate(m_InstantiatedComputeShader);
                DestroyImmediate(m_InstantiatedMaterial);
            }

            m_SourceVertBuffer?.Release();
            m_DrawBuffer?.Release();
            m_ArgsBuffer?.Release();
        }

        m_Initialized = false;
    }

    private void LateUpdate()
    {
        if (Application.isPlaying == false)
        {
            OnDisable();
            OnEnable();
        }

        if (!m_Initialized)
        {
            return;
        }

        m_DrawBuffer.SetCounterValue(0);
        m_ArgsBuffer.SetData(argsBufferReset);

        Bounds bounds = TransformBounds(m_LocalBounds);

        SetGrassDataUpdate();

        m_InstantiatedComputeShader.Dispatch(m_IdGrassKernel, m_DispatchSize, 1, 1);

        Graphics.DrawProceduralIndirect(m_InstantiatedMaterial, bounds, MeshTopology.Triangles,
            m_ArgsBuffer, 0, null, null, castShadow, true, gameObject.layer);
    }

    private void SetGrassDataBase()
    {
        m_InstantiatedComputeShader.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
        m_InstantiatedComputeShader.SetFloat("_Time", Time.time);

        m_InstantiatedComputeShader.SetFloat("_GrassHeight", grassHeight);
        m_InstantiatedComputeShader.SetFloat("_GrassWidth", grassWidth);
        m_InstantiatedComputeShader.SetFloat("_GrassRandomHeight", grassRandomHeight);

        m_InstantiatedComputeShader.SetFloat("_WindSpeed", windSpeed);
        m_InstantiatedComputeShader.SetFloat("_WindStrength", windStrength);

        m_InstantiatedComputeShader.SetFloat("_InteractorRadius", affectRadius);
        m_InstantiatedComputeShader.SetFloat("_InteractorStrength", affectStrength);

        m_InstantiatedComputeShader.SetFloat("_BladeRadius", bladeRadius);
        m_InstantiatedComputeShader.SetFloat("_BladeForward", bladeForwardAmount);
        m_InstantiatedComputeShader.SetFloat("_BladeCurve", Mathf.Max(0, bladeCurveAmount));

        m_InstantiatedComputeShader.SetFloat("_MinFadeDist", minFadeDistance);
        m_InstantiatedComputeShader.SetFloat("_MaxFadeDist", maxFadeDistance);

    }

    private void SetGrassDataUpdate()
    {
        m_InstantiatedComputeShader.SetFloat("_Time", Time.time);


        if (m_MainCamera != null)
        {
            m_InstantiatedComputeShader.SetVector("_CameraPositionWS", m_MainCamera.transform.position);

        }
#if UNITY_EDITOR
        else if (view != null)
        {
            m_InstantiatedComputeShader.SetVector("_CameraPositionWS", view.camera.transform.position);
        }
#endif

    }


    private Bounds TransformBounds(Bounds boundsOS)
    {
        var center = transform.TransformPoint(boundsOS.center);

        var extents = boundsOS.extents;
        var axisX = transform.TransformVector(extents.x, 0, 0);
        var axisY = transform.TransformVector(0, extents.y, 0);
        var axisZ = transform.TransformVector(0, 0, extents.z);

        extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
        extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
        extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

        return new Bounds { center = center, extents = extents };
    }
}