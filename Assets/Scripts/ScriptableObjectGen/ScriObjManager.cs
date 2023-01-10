using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriObjManager : MonoBehaviour
{
    public GameStateSO gameState;
    public static ScriObjManager instance;

    public Plants Wheat;
    public Plants Corn;
    public Plants Broccoli;
    public Plants Tomato;
    public Plants Carrot;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        for (int i = 0; i < gameState.backpack.Count; i++)
        {
            //Debug.Log(1);

            switch ((int)gameState.backpack[i].entity.name)
            {
                case 5:
                    Wheat.seedHold = gameState.backpack[i].amount;
                    //Debug.Log("5");
                    break;
                    
                case 6:
                    Corn.seedHold = gameState.backpack[i].amount;
                    break;
                case 7:
                    Broccoli.seedHold = gameState.backpack[i].amount;
                    break;
                case 8:
                    Tomato.seedHold = gameState.backpack[i].amount;
                    break;
                case 9:
                    Carrot.seedHold = gameState.backpack[i].amount;
                    break;
                //default:
                //    Wheat.seedHold = 0;
                //    Corn.seedHold = 0;
                //    Broccoli.seedHold = 0;
                //    Tomato.seedHold = 0;
                //    Carrot.seedHold = 0;
                //    break;

            }
        }
    }

    public void RemoveFromBag(Plants plant, int amount)
    {
        if(plant.name == "Wheat")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if(gameState.backpack[i].entity.name == EntityType.Wheat_seed)
                {
                    gameState.backpack[i].amount -= amount;
                }
            }
        }
        else if(plant.name == "Corn")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Corn_seed)
                {
                    gameState.backpack[i].amount -= amount;
                }
            }
        }
        else if(plant.name == "Broccoli")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Broccoli_seed)
                {
                    gameState.backpack[i].amount -= amount;
                }
            }
        }
        else if(plant.name == "Tomato")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Tomato_seed)
                {
                    gameState.backpack[i].amount -= amount;
                }
            }
        }
        else if(plant.name == "Carrot")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Carrot_seed)
                {
                    gameState.backpack[i].amount -= amount;
                }
            }
        }
    } 
    
    public void AddToBag(Plants plant, int amount)
    {
        if (plant.name == "Wheat")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Wheat)
                {
                    gameState.backpack[i].amount += amount;
                }
            }
        }
        else if (plant.name == "Corn")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Corn)
                {
                    gameState.backpack[i].amount += amount;
                }
            }
        }
        else if (plant.name == "Broccoli")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Broccoli)
                {
                    gameState.backpack[i].amount += amount;
                }
            }
        }
        else if (plant.name == "Tomato")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Tomato)
                {
                    gameState.backpack[i].amount += amount;
                }
            }
        }
        else if (plant.name == "Carrot")
        {
            for (int i = 0; i < gameState.backpack.Count; i++)
            {
                if (gameState.backpack[i].entity.name == EntityType.Carrot)
                {
                    gameState.backpack[i].amount += amount;
                }
            }
        }
    }
}
