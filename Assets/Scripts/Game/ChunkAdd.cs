using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkAdd : MonoBehaviour
{
    private Vector3 original_scale;
    private Material ChunkAdd_add_mat;

    private Color original_color;
    public Color hover_color;
    public Color pressed_color;

    public int target_chunk_i;
    public int target_chunk_j;


    public AnimationCurve curve;
    public GameObject land_chunk;


    private void Start()
    {
        original_scale = transform.localScale;
        ChunkAdd_add_mat = GetComponent<MeshRenderer>().material;
        original_color = ChunkAdd_add_mat.color;
        
    }

    private void OnMouseOver()
    {
        //Debug.Log("Hover");
        //transform.localScale = original_scale * (Mathf.Sin(Time.deltaTime / 10) + 0.7f);
        //Debug.Log(curve.Evaluate(Time.deltaTime));
    }

    private void OnMouseEnter()
    {
        ChunkAdd_add_mat.color = hover_color;
        //transform.localScale = original_scale * 0.85f;
        SoundManager.instance.Selected();
    }

    private void OnMouseExit()
    {
        ChunkAdd_add_mat.color = original_color;
        transform.localScale = original_scale;
    }

    private void OnMouseDown()
    {
        ChunkAdd_add_mat.color = pressed_color;
        transform.localScale = original_scale * 0.7f;

        if (GameManager.instance.game_state.money >= 200)
        {
            SoundManager.instance.Press();
            GameManager.instance.game_state.money -= 200;
            GameObject.FindGameObjectWithTag("Farm").GetComponent<Farm>().reclaimLandChunk(target_chunk_i, target_chunk_j);
            GameObject.FindGameObjectWithTag("Farm").GetComponent<FarmGenerator>().GenerateLandChunk(target_chunk_i, target_chunk_j);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Money not enough");
        }

    }

}
