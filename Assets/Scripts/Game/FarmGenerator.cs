using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmGenerator : MonoBehaviour
{
    
    public GameObject land_chunk_object;
    public GameObject add_land_object;
    private Farm farm;
    public List<ChunkAdd> all_chunk_add;
    public List<GameObject> all_chunk_land;

    // Start is called before the first frame update
    void Start()
    {
        all_chunk_add = new List<ChunkAdd>();
        farm = GetComponent<Farm>();
        GenerateLandChunks();
    }


    public void GenerateAddLandObject(int i, int j)
    {
        add_land_object.SetActive(false);

        for (int add_i = 0; add_i < all_chunk_add.Count; add_i++)
        {
            if (i == all_chunk_add[add_i].target_chunk_i && j == all_chunk_add[add_i].target_chunk_j)
            {
                //Debug.Log("Existed");
                return;
            }
        }
        GameObject new_add_land_object = Instantiate(add_land_object);
        new_add_land_object.transform.position = new Vector3(i * 5 + 2, 0, j * 5 - 2);
        new_add_land_object.SetActive(true);
        new_add_land_object.GetComponent<ChunkAdd>().target_chunk_i = i;
        new_add_land_object.GetComponent<ChunkAdd>().target_chunk_j = j;
        //Debug.Log(new_add_land_object.activeSelf);

        //for (int add_i = 0; add_i < all_chunk_add.Count; add_i++)
        //{
        //    if (all_chunk_add[i] == null)
        //    {
        //        //Debug.Log("Existed");
        //        all_chunk_add[i] = new_add_land_object.GetComponent<ChunkAdd>();
        //        return;
        //    }
        //}
        all_chunk_add.Add(new_add_land_object.GetComponent<ChunkAdd>());
        
    }

    public void GenerateLandChunk(int i, int j)
    {
        LandChunk[,] curr_farm_matrix = farm.FarmMatrix;
        LandChunk curr_land_chunk = curr_farm_matrix[i, j];

        if (curr_land_chunk.cultivated)
        {
            //Debug.Log("Generate cultivated Land Chunk");
            GenerateLands(curr_land_chunk, i, j);

            //Debug.Log("Generate add mark");
            if (i >= 1 && !curr_farm_matrix[i - 1, j].cultivated)
            {
                GenerateAddLandObject(i - 1, j);
                //Debug.Log("Left");
            }
            if (i < curr_farm_matrix.Length - 1 && !curr_farm_matrix[i + 1, j].cultivated)
            {
                GenerateAddLandObject(i + 1, j);
                //Debug.Log("Right");
            }
            if (j >= 1 && !curr_farm_matrix[i, j - 1].cultivated)
            {
                GenerateAddLandObject(i, j - 1);
                //Debug.Log("Down");
            }
            if (j < curr_farm_matrix.Length - 1 && !curr_farm_matrix[i, j + 1].cultivated)
            {

                GenerateAddLandObject(i, j + 1);
                //Debug.Log("Up");
            }
        }
    }

    private void GenerateLandChunks()
    {
        farm = GetComponent<Farm>();
        LandChunk[,] curr_farm_matrix = farm.FarmMatrix;
        //Debug.Log(farm.FarmMatrix.Length);
        for (int i = 0; i < curr_farm_matrix.GetLength(0); i++)
        {
            for (int j = 0; j < curr_farm_matrix.GetLength(1); j++)
            {
                GenerateLandChunk(i, j);
            }
        }
    }


    // i: element index dimension 0 in farm_matrix
    // j: element index dimension 1 in farm_matrix
    private void GenerateLands(LandChunk land_chunk, int chunk_i, int chunk_j)
    {
        land_chunk_object.SetActive(false);
        GameObject new_chunk_object = Instantiate(land_chunk_object);
        RecordChunkAttribute recordIndex = new_chunk_object.GetComponentInChildren<RecordChunkAttribute>();

        //�ڳ�ʼ��ʱ��¼chunk��FarmMatrix���index
        recordIndex.chunkIndex_x = chunk_i;
        recordIndex.chunkIndex_y = chunk_j;

        //��������ǲ����õ�һ��Ҫɾ
        farm.FarmMatrix[chunk_i, chunk_j].totalValue = Random.Range(1, 10);

        new_chunk_object.transform.position = new Vector3(chunk_i * 5, 0, chunk_j * 5);
        new_chunk_object.SetActive(true);
        all_chunk_land.Add(new_chunk_object);
    }
}
