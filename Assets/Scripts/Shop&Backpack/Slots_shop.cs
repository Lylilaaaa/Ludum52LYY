using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slots_shop : MonoBehaviour
{

    public GameObject shopDetail;

    public Image itemImageSprite;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;

    //public BackpackElement item;
    public Entity entity;

    public void showDetail()
    {
        shopDetail.SetActive(true);
        shopDetail.GetComponent<ShopDetail>().entity = entity;
        //shopDetail.GetComponent<ShopDetail>().unitPrice = entity.price;
        //shopDetail.GetComponent<ShopDetail>().price = entity.price;
        //shopDetail.GetComponent<ShopDetail>().num = entity.num;

        itemImageSprite.sprite = entity.image;
        itemNameText.text = entity.name.ToString();
        itemPriceText.text = entity.price.ToString();

    }
}
