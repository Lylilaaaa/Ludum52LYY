using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetValueUI : MonoBehaviour
{
    public float rootParentValue;
    private void Start()
    {
        rootParentValue = transform.parent.parent.GetComponent<UIShownInformation>().thisPlants.value;
    }
    private void Update()
    {
        rootParentValue = transform.parent.parent.GetComponent<UIShownInformation>().thisPlants.value;
    }
}
