using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreenCanvas : MonoBehaviour
{
    public static LoadScreenCanvas instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
