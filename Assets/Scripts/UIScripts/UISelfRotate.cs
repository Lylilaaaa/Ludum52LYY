using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelfRotate : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(Vector3.forward,45*Time.fixedDeltaTime);
    }
}
