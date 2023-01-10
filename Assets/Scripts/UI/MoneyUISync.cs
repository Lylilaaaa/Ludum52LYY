using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUISync : MonoBehaviour
{
    private TMP_Text tmp_text;

    private void Start()
    {
        tmp_text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp_text.text = GameManager.instance.game_state.money.ToString();
    }
}
