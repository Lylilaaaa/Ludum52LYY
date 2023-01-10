using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordChunkAttribute : MonoBehaviour
{
    /* ��FarmGenerator 
     * private void GenerateLands(LandChunk land_chunk, int chunk_i, int chunk_j)
     * ʱ��¼i j
     */
    public Farm farm;
    public int chunkIndex_x, chunkIndex_y;
    public bool CUL;
    public bool occ;
    public int HumidityValue;

    public LandChunk landChunk;

    //chunk�������ܼ�ֵ�����ֵ, ��������
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
