using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWateringCDEnable : MonoBehaviour
{
    private Slider waterCDRing;
    private Button canWaterButton;

    public float wateringCD;
    public bool canWatering = true;
    public bool canCount = false;

    public float timeCounter = 10f;
    private void Start()
    {
        waterCDRing = gameObject.GetComponent<Slider>();
        waterCDRing.maxValue = wateringCD;
        waterCDRing.value = 0;

        canWaterButton = transform.parent.gameObject.GetComponent<Button>();
    }
    private void FixedUpdate()
    {
        if (canCount == true)
        {
            //��ʼ����
            timeCounter -= Time.fixedDeltaTime;
            //��ʾ��ring��slider��
            waterCDRing.maxValue = wateringCD;
            waterCDRing.value = timeCounter;
            canWatering = false;
        }
    }

    private void Update()
    {
        canWaterButton.interactable = canWatering;
        if (timeCounter <= 0)
        {
            canCount = false;
            canWatering = true;
        }
    }

    public void UseWatering()
    {
        if(canWatering == true)
        {
            canWatering = false;
            canCount = true;

            //��ʼ��count����ֵ
            waterCDRing.maxValue = wateringCD;
            timeCounter = waterCDRing.maxValue;
            waterCDRing.value = Mathf.Lerp(waterCDRing.value, timeCounter,Time.deltaTime*10f) ;
        }
    }
}
