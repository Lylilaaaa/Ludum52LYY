using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChunkState : MonoBehaviour
{
    //读取后端的数据，从而得知此作物是否有被施肥或浇水
    private Farm farm;
    private LandChunk landChunk;
    public bool fertilized, watered;
    public ChunkEventType chunkEventType;
    public int HumidityValue;

    private void Awake()
    {
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
    }

    void FixedUpdate()
    {

        landChunk = farm.FarmMatrix[
            transform.root.Find("field interactable").GetComponent<RecordChunkAttribute>().chunkIndex_x,
            transform.root.Find("field interactable").GetComponent<RecordChunkAttribute>().chunkIndex_y];


        HumidityValue = landChunk.HumidityValue;
        fertilized = landChunk.fertilized;
        watered = landChunk.watered;
        chunkEventType = transform.root.Find("field interactable").GetComponent<RandomEvent>().eventType;
    }

    public void Water()
    {
        landChunk.watered = true;
        landChunk.HumidityValue += 30;
    }

    public void Fertilize()
    {
        landChunk.fertilized = true;
    }
}
