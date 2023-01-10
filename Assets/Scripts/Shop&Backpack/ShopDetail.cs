using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopDetail : MonoBehaviour
{
    public GameStateSO gameState;
    public Entity entity;

    public TMP_Text priceText;
    public TMP_Text numText;
    public TMP_Text buttonText;

    private int unitPrice;
    private int price;
    private int num;

    public HarvesterManager harvesterManager;
    public DroneGM droneGM;
    public Harvester harvester;


    private void Start()
    {
        gameState = GameManager.instance.game_state;
        //unitPrice = (int)float.Parse(priceText.text);
        //price = (int)float.Parse(priceText.text);

        unitPrice = entity.price;
        price = entity.price;
        num = 1;
    }

    private void Update()
    {
        priceText.text = price.ToString();
        numText.text = num.ToString();

        if ((int)entity.name < 11 | (int)entity.name == 12 | (int)entity.name == 13 | (int)entity.name == 14)
        {
            buttonText.text = "BUY";
        }
        else
        {
            buttonText.text = "UPGRADE";
        }
    }

    private void OnEnable()
    {
        unitPrice = entity.price;
        price = entity.price;
        num = 1;

        

    }

    //private void Awake()
    //{
    //    //unitPrice = (int)float.Parse(priceText.text);
    //    //price = (int)float.Parse(priceText.text);

    //    unitPrice = entity.price;
    //    price = entity.price;
    //    num = 1;
    //}


    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void Add()
    {
        if(num <= 50)
        {
            num += 1;
            price += unitPrice;
        }


    }
    public void Minus()
    {
        if(num > 1)
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

    public void BuyOrUpgrade()
    {

        if ((int)entity.name < 11 | (int)entity.name == 12 | (int)entity.name == 13 | (int)entity.name == 14)
        {
            Buy();
        }
        else if ((int)entity.name == 11)
        {
            Upgrade();
        }
        else if((int)entity.name == 15)
        {
            Upgrade3();
        }
        else if ((int)entity.name == 16)
        {
            Upgrade2();
        }
        else if((int)entity.name == 17)
        {
            Upgrade1();
        }
    }

    public void Buy()
    {
        if(gameState.money >= price)
        {
            //¿ÛÇ®
            gameState.money -= price;

            //¼Ó±³°ü
            gameState.AddEntityToBackpack(entity, num);
        }

        
    }

    public void Upgrade()
    {
        if (gameState.money >= price)
        {
            //¿ÛÇ®
            gameState.money -= price;

            //Éý¼¶
            for (int i = 0; i < num; i++)
            {
                harvester.Update();

            }
        }
    }

    public void Upgrade1()
    {
        if (gameState.money >= price)
        {
            //¿ÛÇ®
            gameState.money -= price;

            //Éý¼¶
            for (int i = 0; i < num; i++)
            {
                droneGM.upgradeBDrone();
               
            }
        }
    }

    public void Upgrade2()
    {
        if (gameState.money >= price)
        {
            //¿ÛÇ®
            gameState.money -= price;

            //Éý¼¶
            for (int i = 0; i < num; i++)
            {
                droneGM.upgradeFDrone();

            }
        }
    }

    public void Upgrade3()
    {
        if (gameState.money >= price)
        {
            //¿ÛÇ®
            gameState.money -= price;

            //Éý¼¶
            for (int i = 0; i < num; i++)
            {
                droneGM.upgradeWDrone();

            }
        }
    }

}
