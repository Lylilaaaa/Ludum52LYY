using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetSeedNumShowUI : MonoBehaviour
{
    public Plants curPlant;

    private TMP_Text shownSeed;
    private void Start()
    {
        shownSeed = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        shownSeed.text = curPlant.seedHold.ToString();
    }
}
