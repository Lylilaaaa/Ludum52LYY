using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private int moneyNum;
    public TMP_Text moneyText;

    public GameObject seedsBackground;
    public GameObject seedsInventory;
    public GameObject machinesBackground;
    public GameObject machinesInventory;

    public GameStateSO gameState;


    private void Start()
    {
        gameState = GameManager.instance.game_state;
        seedsBackground.SetActive(true);
        machinesBackground.SetActive(false);
        seedsInventory.SetActive(true);
        machinesInventory.SetActive(false);

        //get money and show money
        moneyNum = gameState.money;
        moneyText.text = moneyNum.ToString();
    }

    private void Update()
    {
        //get money and show money
        moneyNum = gameState.money;
        moneyText.text = moneyNum.ToString();
    }

    public void showSeeds()
    {
        seedsBackground.SetActive(true);
        machinesBackground.SetActive(false);
        seedsInventory.SetActive(true);
        machinesInventory.SetActive(false);
    }

    public void showMachines()
    {
        seedsBackground.SetActive(false);
        machinesBackground.SetActive(true);
        seedsInventory.SetActive(false);
        machinesInventory.SetActive(true);
    }
}
