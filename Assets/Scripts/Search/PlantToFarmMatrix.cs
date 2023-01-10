using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantToFarmMatrix : MonoBehaviour
{
    public CropType cropType;
    public Farm farm;
    public int index_x, index_y;
    public int chunkIndex_x, chunkIndex_y;
    public Land land;

    public void Start()
    {
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();

        chunkIndex_x = transform.root.Find("field interactable").GetComponent<RecordChunkAttribute>().chunkIndex_x;
        chunkIndex_y = transform.root.Find("field interactable").GetComponent<RecordChunkAttribute>().chunkIndex_y;

        char[] b = transform.parent.name.ToCharArray();
        Debug.Log(b);
        Debug.Log(b[0]);
        Debug.Log(b[1]);
        index_x = int.Parse(b[0].ToString());
        index_y = int.Parse(b[1].ToString());

        land = farm.FarmMatrix[chunkIndex_x, chunkIndex_y].unitLand[index_x-1, index_y-1];
        UpdateMature();
    }
    public void UpdateMature()
    {
        land.cropType = CropType.UnMature;
    }
    public void UpdateCorpType(CropType _cropType)
    {
        land.cropType = _cropType;
    }


    public void Update()
    {
        
    }
}
