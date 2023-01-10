using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public void AddMoney(int i)
    {
        GameManager.instance.AddMoney(i);
    }
}
