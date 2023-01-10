using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionOnPlantThePlant : MonoBehaviour
{
    
    public void CloseSelectSeedPanel()
    {
        Debug.Log("close succeess");
        gameObject.SetActive(false);
    }

    public void ReduceSeedHold(Plants curPlant)
    {
        //if (curPlant.seedHold <= 0)
        //{
        //    Debug.LogError("No seed any more!!! Buy some seeds plz.");
        //    return;
        //}
        //else
        //{
        //    curPlant.seedHold -= 1;
        //}
        return;
    }
}
