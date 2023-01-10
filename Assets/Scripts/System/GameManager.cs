using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStateSO game_state;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (game_state.money == 0)
        {
            // init money
            game_state.money = 1000;
        }
    }

    public void AddMoney(int i)
    {
        game_state.money += i;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {

    }
}
