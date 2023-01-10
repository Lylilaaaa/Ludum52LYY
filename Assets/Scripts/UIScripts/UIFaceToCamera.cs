using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceToCamera : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.forward = -new Vector3(transform.position.x, transform.position.y, transform.position.z) +
                            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
}
