using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDestroyHarvest : MonoBehaviour
{
    public void DestroyPlantSelf()
    {
        //
        print("shou le");

        //update backpack data 
        //ScriObjManager.instance.AddToBag(transform.parent.GetComponent<UIShownInformation>().thisPlants,1);
        GameManager.instance.game_state.money += (int)transform.parent.GetComponent<UIShownInformation>().thisPlants.value;

        //destory self
        Destroy(transform.parent.gameObject);
        
    }
}
