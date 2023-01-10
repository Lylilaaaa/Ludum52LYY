
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIShownInformation : MonoBehaviour
{

    public PlantToFarmMatrix pm;

    public Plants thisPlants;
    public GameObject nameTMP;
    public GameObject matureMaxTMP;
    public GameObject growSpeedTMP;
    public GameObject valueTMP;

    public GameObject curTimeTMP;
    public GameObject curMatureTMP;

    public GameObject canHarvestPanel;
    
    public Slider clockSlider;
    
    private TMP_Text timeCounter;
    public float ftimeCounter=0;
    private TMP_Text matureCounter;
    public int intMatureCounter=0;

    public bool HarvestorActivate = false;
    private bool canHarvest;

    //读取后端的数据，从而得知此作物是否有被施肥或浇水
    private Farm farm;
    private LandChunk landChunk;
    public bool fertilized, watered;
    public int humidityValue;

    private void Start()
    {
        pm = GetComponent<PlantToFarmMatrix>();
        //获取后端数据
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
        landChunk = farm.FarmMatrix[
        transform.root.Find("field interactable").GetComponent<RecordChunkAttribute>().chunkIndex_x,
        transform.root.Find("field interactable").GetComponent<RecordChunkAttribute>().chunkIndex_y];

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        canHarvestPanel.SetActive(false);
        nameTMP.GetComponent<TMP_Text>().text =  thisPlants.name;
        matureMaxTMP.GetComponent<TMP_Text>().text =  thisPlants.matureTime.ToString();

        growSpeedTMP.GetComponent<TMP_Text>().text =  thisPlants.growSpeed.ToString();

        valueTMP.GetComponent<TMP_Text>().text = thisPlants.value.ToString();

        clockSlider.maxValue = thisPlants.growSpeed;
        clockSlider.value = thisPlants.growSpeed;
        timeCounter = curTimeTMP.GetComponent<TMP_Text>();
        matureCounter = curMatureTMP.GetComponent<TMP_Text>();
        matureCounter.text = intMatureCounter.ToString();
        transform.GetChild(1).gameObject.SetActive(true);
        if (transform.childCount >= 2)
        {
            for(int i = 2;i<transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        fertilized = landChunk.fertilized;
        watered = landChunk.watered;
        humidityValue = landChunk.HumidityValue;


        BeHarvested();
        if (!canHarvest)
        {
            HarvestorActivate = false;
        }
    }

    public void BeHarvested()
    {
        //pm.cropType = CropType.Empty;
        float tempGrowSpeed = thisPlants.growSpeed;
        
        if (humidityValue>0)
        {

            tempGrowSpeed = (float)thisPlants.growSpeed * 0.75f;

            //Debug.Log("tempGrowSpeed is :" + tempGrowSpeed);
            //更改growspeed UI max的显示
            growSpeedTMP.GetComponent<TMP_Text>().text = tempGrowSpeed.ToString();
            //防止超出slider max
            if(clockSlider.value > tempGrowSpeed)
            {
                clockSlider.value = tempGrowSpeed;
            }
            clockSlider.maxValue = tempGrowSpeed;
        }

        else
        {
            tempGrowSpeed = thisPlants.growSpeed;
            growSpeedTMP.GetComponent<TMP_Text>().text = tempGrowSpeed.ToString();
            clockSlider.value = tempGrowSpeed - ftimeCounter;
            clockSlider.maxValue = tempGrowSpeed;
        }
        if (ftimeCounter >= tempGrowSpeed && intMatureCounter < thisPlants.matureTime) //mature state add one
        {
            intMatureCounter += 1;
            matureCounter.text = intMatureCounter.ToString();
            if (intMatureCounter >= thisPlants.matureTime) //the mature time is arrive, can Harvest
            {
                transform.GetChild(intMatureCounter).gameObject.SetActive(false); //close the last state
                transform.GetChild(intMatureCounter + 1).gameObject.SetActive(true); //open the current state
                canHarvestPanel.SetActive(true);
                transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);  //
                intMatureCounter = thisPlants.matureTime;
                if (!transform.GetChild(intMatureCounter + 2).gameObject.activeInHierarchy)
                {
                    canHarvest = true;
                    pm.UpdateCorpType(pm.cropType);
                }
            }
            else
            {
                transform.GetChild(intMatureCounter).gameObject.SetActive(false);
                transform.GetChild(intMatureCounter + 1).gameObject.SetActive(true);
                ftimeCounter = 0;
                clockSlider.value = tempGrowSpeed - ftimeCounter;
                clockSlider.maxValue = tempGrowSpeed;
            }
        }
        else if (ftimeCounter < tempGrowSpeed && intMatureCounter < thisPlants.matureTime)
        {
            ftimeCounter += Time.fixedDeltaTime;
            clockSlider.value = tempGrowSpeed - ftimeCounter;
            clockSlider.maxValue = tempGrowSpeed;
            timeCounter.text = Mathf.Round(ftimeCounter).ToString();
        }
        playHarvestAnim();
        //pm.UpdateCorpType(CropType.Empty);
    }

    IEnumerator WaitforFiveS()
    {
        yield return new WaitForSeconds(5.0f);
        transform.GetChild(intMatureCounter+1).gameObject.SetActive(false);
        transform.GetChild(intMatureCounter+2).gameObject.SetActive(true);
        
    }

    private void playHarvestAnim()
    {
        if (HarvestorActivate && canHarvest)
        {
            Debug.Log("playHarvest2");
            pm.cropType = CropType.Empty;
            transform.GetChild(intMatureCounter+1).gameObject.SetActive(false);
            transform.GetChild(intMatureCounter+2).gameObject.SetActive(true);
        }
    }
}
