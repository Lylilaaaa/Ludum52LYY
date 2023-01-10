using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWateringMode : MonoBehaviour
{
    public static OpenWateringMode instance;

    private void Awake()
    {
        instance = this;
    }
    public bool wateringMode = false;

    public void OpenWholeWateringMode()
    {
        wateringMode = true;
    }

}
