using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandUI : MonoBehaviour
{
    private GameObject selectedobj;
    private bool isOver = true;
    private void OnMouseEnter()
    {
        isOver = false;
    }
    private void OnMouseExit()
    {
        isOver = true;
    }
    private void Update()
    {
        if (!isOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedobj == null)
                {
                    RaycastHit hit = CastRay();
                    if (!hit.collider.gameObject)
                    {
                        return;
                    }
                    selectedobj = hit.collider.gameObject;
                    selectedobj.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    selectedobj.GetComponent<Outline>().enabled = false;
                    selectedobj = null;
                }
            }
        }
    }
    private RaycastHit CastRay()
    {
        Vector3 screenFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 far = Camera.main.ScreenToWorldPoint(screenFar);
        Vector3 near = Camera.main.ScreenToWorldPoint(screenNear);

        RaycastHit hit;
        Physics.Raycast(near, far - near, out hit);

        return hit;
    }
}
