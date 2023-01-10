using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 1;
    public float scrollSensitivity = 1;
    private Vector3 newCamPos;

    private void Awake()
    {
        newCamPos = Camera.main.transform.position;
    }

    void OnGUI()
    {
        newCamPos += Camera.main.transform.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

        if (Input.GetKey(KeyCode.W))
        {
            newCamPos += new Vector3(0, 0, moveSpeed * newCamPos.y / 15);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newCamPos += new Vector3(0, 0, -moveSpeed * newCamPos.y / 15);
        }

        if (Input.GetKey(KeyCode.A))
        {
            newCamPos += new Vector3(-moveSpeed * newCamPos.y / 15, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newCamPos += new Vector3(moveSpeed * newCamPos.y / 15, 0, 0);
        }

        if (newCamPos.y > 2 && newCamPos.y < 15)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCamPos, Time.deltaTime);

        }
        else
        {
            newCamPos = Camera.main.transform.position;
        }
    }
}
