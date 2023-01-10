using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BackpackDetail : MonoBehaviour
{
    public GameStateSO gameState;
    public BackpackElement item;

    public Image itemImageSprite;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;
    public TMP_Text itemPriceNumText;
    public TMP_Text itemNumText;
    public TMP_Text buttonText;



    private int unitPrice;
    private int price;
    private int num;

    public HarvesterManager harvesterManager;
    public DroneGM droneGM;


    private void Start()
    {

        num = 1;

        //itemImageSprite.sprite = item.entity.image;
        //itemNameText.text = item.entity.name.ToString();

        if ((int)item.entity.name < 5)
        {
            unitPrice = item.entity.price;
            price = item.entity.price;

            buttonText.text = "SELL";
            itemPriceText.text = item.entity.price.ToString();
        }
        else
        {
            buttonText.text = "USE";
            itemPriceNumText.enabled = false;
            itemPriceText.enabled = false;
        }
    }

    private void Update()
    {
        harvesterManager = GameObject.Find("HarvesterManager").GetComponent<HarvesterManager>();

        //物品名称和图片
        itemImageSprite.sprite = item.entity.image;
        itemNameText.text = item.entity.name.ToString();

        if ((int)item.entity.name < 5)
        {
            //物品总价
            itemPriceNumText.text = price.ToString();
        }
        //物品数量
        itemNumText.text = num.ToString();
    }

    private void OnEnable()
    {
        //unitPrice = (int)float.Parse(priceText.text);
        //price = (int)float.Parse(priceText.text);

        num = 1;

        //itemImageSprite.sprite = item.entity.image;
        //itemNameText.text = item.entity.name.ToString();

        if ((int)item.entity.name < 5)
        {
            unitPrice = item.entity.price;
            price = item.entity.price;

            buttonText.text = "SELL";
            itemPriceText.text = item.entity.price.ToString();
        }
        else
        {
            buttonText.text = "USE";
            itemPriceNumText.enabled = false;
            itemPriceText.enabled = false;
        }
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void Add()
    {
        if (num <= 50)
        {
            num += 1;
            price += unitPrice;
        }


    }
    public void Minus()
    {
        if (num > 1)
        {
            num -= 1;
            price -= unitPrice;
        }
    }

    public void Cancel()
    {
        num = 1;
        price = unitPrice;
    }

    public void SellOrUse()
    {
        if((int)item.entity.name < 5)
        {
            Sell();
        }
        else if((int)item.entity.name < 10)
        {
            Seed();
        }
        else if((int)item.entity.name < 11)
        {
            Put();
        }
        else if((int)item.entity.name == 12)
        {
            PutUFO(2);
        }
        else if((int)item.entity.name == 13)
        {
            PutUFO(1);

        }
        else if((int)item.entity.name == 14)
        {
            PutUFO(1);
        }
    }
    public void Sell()
    {
        if(item.amount >= num) 
        {

            //得钱
            gameState.money += price;

            //背包减
            item.amount -= num;
        }
    }

    public void Seed()
    {
        if(item.amount >= num)
        {
            // 在游戏界面里面改
        }
    }

    public void Put()
    {
        if(item.amount >= num)
        {
            for(int i = 0; i < num; i++)
            {
                harvesterManager.AddHavester();
                Debug.Log("add harvest");
            }
            item.amount -= num;
        }
    }

    public void PutUFO(int index)
    {
        if(index == 0)
        {
            if (item.amount >= num)
            {
                for (int i = 0; i < num; i++)
                {
                    droneGM.generateBDrone();
                }
            }
            item.amount -= num;
        }
        else if(index == 1)
        {
            if (item.amount >= num)
            {
                for (int i = 0; i < num; i++)
                {
                    droneGM.generateFDrone();
                }
            }
            item.amount -= num;
        }
        else if (index == 2)
        {
            if (item.amount >= num)
            {
                for (int i = 0; i < num; i++)
                {
                    droneGM.generateWDrone();
                }
            }
            item.amount -= num;
        }
    }


}
