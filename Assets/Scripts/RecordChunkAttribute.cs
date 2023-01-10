using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordChunkAttribute : MonoBehaviour
{
    /* 在FarmGenerator 
     * private void GenerateLands(LandChunk land_chunk, int chunk_i, int chunk_j)
     * 时记录i j
     */
    public Farm farm;
    public int chunkIndex_x, chunkIndex_y;
    public bool CUL;
    public bool occ;
    public int HumidityValue;

    public LandChunk landChunk;

    //chunk内作物总价值作物价值, 仅调试用
    public int totalCorpValue;

    private void Start()
    {
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
        landChunk = farm.FarmMatrix[chunkIndex_x, chunkIndex_y];
    }
    private void FixedUpdate()
    {
        totalCorpValue = landChunk.totalValue;
        CUL = landChunk.cultivated;
        occ = landChunk.harvesterOccupied;
        HumidityValue = landChunk.HumidityValue;
        landChunk.setPlantNum();
        landChunk.calVal();
    }

    public int GetChunkId()
    {
        return chunkIndex_y * 32 + chunkIndex_x;
    }
}
