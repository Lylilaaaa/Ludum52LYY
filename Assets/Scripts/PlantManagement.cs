using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManagement : MonoBehaviour
{
    public GameObject wheatPrefab;
    public GameObject cornPrefab;
    public GameObject tomatoPrefab;
    public GameObject broccoliPrefab;
    public GameObject carrotPrefab;
    private LandChunk cur_landchunk;

    private void Start()
    {
        //cur_landchunk = this.gameObject.GetComponent<RecordChunkAttribute>().landChunk;
    }

    // ÖÖ¶«Î÷
    public void PlantingTheWheat(GameObject fieldTrans)
    {
        if (wheatPrefab.GetComponent<UIShownInformation>().thisPlants.seedHold <= 0) 
        {
            Debug.Log("no enough seed");
            return;
        }
        GameObject tempPlant = Instantiate(wheatPrefab, fieldTrans.transform);
        tempPlant.transform.localPosition = Vector3.zero;
        tempPlant.transform.localRotation = Quaternion.identity;
        ScriObjManager.instance.RemoveFromBag(wheatPrefab.GetComponent<UIShownInformation>().thisPlants, 1);

        //update farm data



        //Debug.Log(fieldTrans.name.Substring(0));
        //Debug.Log(fieldTrans.name.Substring(1));
    }
    public void PlantingTheCorn(GameObject fieldTrans)
    {
        if (cornPrefab.GetComponent<UIShownInformation>().thisPlants.seedHold <= 0)
        {
            Debug.Log("no enough seed");
            return;
        }
        GameObject tempPlant = Instantiate(cornPrefab, fieldTrans.transform);
        tempPlant.transform.localPosition = Vector3.zero;
        tempPlant.transform.localRotation = Quaternion.identity;
        ScriObjManager.instance.RemoveFromBag(cornPrefab.GetComponent<UIShownInformation>().thisPlants, 1);
        //Debug.Log(fieldTrans.name.Substring(0));
        //Debug.Log(fieldTrans.name.Substring(1));
    }
    public void PlantingTheTomato(GameObject fieldTrans)
    {
        if (tomatoPrefab.GetComponent<UIShownInformation>().thisPlants.seedHold <= 0)
        {
            Debug.Log("no enough seed");
            return;
        }
        GameObject tempPlant = Instantiate(tomatoPrefab, fieldTrans.transform);
        tempPlant.transform.localPosition = Vector3.zero;
        tempPlant.transform.localRotation = Quaternion.identity;
        ScriObjManager.instance.RemoveFromBag(tomatoPrefab.GetComponent<UIShownInformation>().thisPlants, 1);
        //Debug.Log(fieldTrans.name.Substring(0));
        //Debug.Log(fieldTrans.name.Substring(1));
    }
    public void PlantingTheBroccoli(GameObject fieldTrans)
    {
        if (broccoliPrefab.GetComponent<UIShownInformation>().thisPlants.seedHold <= 0)
        {
            Debug.Log("no enough seed");
            return;
        }
        GameObject tempPlant = Instantiate(broccoliPrefab, fieldTrans.transform);
        tempPlant.transform.localPosition = Vector3.zero;
        tempPlant.transform.localRotation = Quaternion.identity;
        ScriObjManager.instance.RemoveFromBag(broccoliPrefab.GetComponent<UIShownInformation>().thisPlants, 1);
        //Debug.Log(fieldTrans.name.Substring(0));
        //Debug.Log(fieldTrans.name.Substring(1));
    }
    public void PlantingTheCarrot(GameObject fieldTrans)
    {
        if (carrotPrefab.GetComponent<UIShownInformation>().thisPlants.seedHold <= 0)
        {
            Debug.Log("no enough seed");
            return;
        }
        GameObject tempPlant = Instantiate(carrotPrefab, fieldTrans.transform);
        tempPlant.transform.localPosition = Vector3.zero;
        tempPlant.transform.localRotation = Quaternion.identity;
        ScriObjManager.instance.RemoveFromBag(carrotPrefab.GetComponent<UIShownInformation>().thisPlants, 1);
        //Debug.Log(fieldTrans.name.Substring(0));
        //Debug.Log(fieldTrans.name.Substring(1));
    }
    
}
