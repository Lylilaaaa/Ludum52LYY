using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFieldLauch : MonoBehaviour
{
    public GameObject fieldParent;

    private Animator fieldAnim;

    public GameObject speedUpPanel;
    public GameObject deleteMoneyPanel;

    public GetChunkState getchunk;

    // Start is called before the first frame update
    void Start()
    {
        fieldAnim = fieldParent.GetComponent<Animator>();
        gameObject.GetComponent<Collider>().enabled = false;
        //gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GameObject tempPanel = Instantiate(deleteMoneyPanel, transform);
        tempPanel.transform.localPosition = Vector3.zero;
        tempPanel.transform.localRotation = Quaternion.identity;
    }

    public void Update()
    {
        OpenCollider();
        getchunk = transform.gameObject.GetComponent<GetChunkState>();
        fieldAnim.SetFloat("HumidityValue", getchunk.HumidityValue);
    }

    public void WaterPlant()
    {
        //fieldAnim.SetBool("IsWet",true);



        //生成特效提示
        GameObject tempPanel = Instantiate(speedUpPanel, transform);
        tempPanel.transform.localPosition = Vector3.zero;
        tempPanel.transform.localRotation = Quaternion.identity;

        SoundManager.instance.Watering();

        OpenWateringMode.instance.wateringMode = false;

        //同步
        getchunk.Water();
    }

    public void OpenCollider()
    {
        if (OpenWateringMode.instance.wateringMode)
        {
            gameObject.GetComponent<Collider>().enabled = true;
            //Debug.Log("open watering collider!");
        }
        else
        {
            gameObject.GetComponent<Collider>().enabled = false;
            //Debug.Log("open watering collider!");
        }
    }
}