using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectedUIcloseDefault : MonoBehaviour
{
    public Button[] buttonList;

    void Start()
    {
        transform.gameObject.GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        SetButtImageGraphic();
    }
    
    void SetButtImageGraphic()
    {
        foreach (Button i in buttonList)
        {
            Image tempImg = i.gameObject.transform.parent.transform.GetChild(0).GetComponent<Image>();
            i.targetGraphic = tempImg;
        }
    }
}
