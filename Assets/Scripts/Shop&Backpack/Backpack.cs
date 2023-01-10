using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    [SerializeField]
    private int itemNum;
    public TMP_Text itemText;

    public GameStateSO gameState;
    public Transform slotGrid;

    public GameObject shopAndBackpack;

    private void Start()
    {
        gameState = GameManager.instance.game_state;
        // get itemNum and show itemNum
        itemNum = gameState.backpack.Count;
        itemText.text = itemNum.ToString();

        
    }

    private void Awake()
    {

    }

    private void Update()
    {
        // get itemNum and show itemNum
        // itemNum = gameState.backpack.Count;
        itemText.text = itemNum.ToString();


        //for(int i = 0; i < 16; i++)
        //{
        //    Transform slot = slotGrid.GetChild(i);
        //    Debug.Log(gameState.backpack.Count);

        //    if (i < gameState.backpack.Count && gameState.backpack[i].amount != 0)
        //    {
                
        //        slot.gameObject.GetComponent<Slots>().item = gameState.backpack[i];
        //        slot.gameObject.GetComponent<Slots>().isEmpty = false;
        //        //Debug.Log(i);
        //        //Debug.Log(j);
        //    }
        //    else
        //    {
        //        slot.gameObject.GetComponent<Slots>().isEmpty = true;
        //        //Debug.Log("isEmpty");
        //    }
        //}

        int j = 0;
        for(int i = 0; i < gameState.backpack.Count; i++)
        {
            Transform slot = slotGrid.GetChild(j);
            if (gameState.backpack[i].amount != 0)
            {
                slot.gameObject.GetComponent<Slots>().item = gameState.backpack[i];
                slot.gameObject.GetComponent<Slots>().isEmpty = false;
                j++;
            }
        }
        for(int i = j; i < 16; i++)
        {
            Transform slot = slotGrid.GetChild(i);
            slot.gameObject.GetComponent<Slots>().isEmpty = true;
        }

        itemNum = j;
    }

    public void Back()
    {
        shopAndBackpack.SetActive(false);
    }

}
