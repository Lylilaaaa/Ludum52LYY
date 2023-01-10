using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{


    public GameObject backpackDetail;

    public bool isEmpty;
    public BackpackElement item;

    public TMP_Text slotNum;
    public Image slotImage;


    private void Start()
    {

    }

    private void Update()
    {
        if (isEmpty)

        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            //foreach (Transform itemInfo in GetComponentsInChildren<Transform>())
            //{
            //    itemInfo.gameObject.SetActive(false);
            //}

        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            //foreach (Transform itemInfo in GetComponentsInChildren<Transform>())
            //{
            //    itemInfo.gameObject.SetActive(true);
            //}


            slotImage.sprite = item.entity.image;
            slotNum.text = item.amount.ToString();
        }
    }

    public void showDetail()
    {
        if (!isEmpty)
        {
            backpackDetail.SetActive(true);
            backpackDetail.GetComponent<BackpackDetail>().item = item;

        }

    }
}
