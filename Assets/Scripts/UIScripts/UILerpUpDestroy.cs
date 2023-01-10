using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILerpUpDestroy : MonoBehaviour
{
    public float lerpUpSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tempTargetPos = transform.localPosition + new Vector3(0f, 1f, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition,tempTargetPos,Time.fixedDeltaTime * lerpUpSpeed);
    }
}
