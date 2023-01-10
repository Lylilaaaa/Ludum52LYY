using UnityEngine;

namespace AppsTools.URP
{

    public class PlayerMove_URP : MonoBehaviour
    {
        void Update()
        {
            Shader.SetGlobalVector("_PositionMoving", transform.position);
        }
    }

}