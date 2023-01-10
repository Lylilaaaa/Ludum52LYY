using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public enum CropType
{
    Empty,
    UnMature,
    wheat, //С��
    Corn, //����
    Broccoli, //������
    Tomato, //������
    Carrot //���ܲ�
}

public class Land
{
    public CropType cropType;
    public int value;
    public int stage; //�׶�
    public int maxStage; //���׶�
    public int landID;
    public bool fertilized;

    public void UpdateValue()
    {
        switch (cropType)
        {
            case CropType.Empty: //�޼�ֵ
                value = 0;
                break;
            case CropType.UnMature:
                value = 0;
                break;
            case CropType.wheat:
                value = 6;
                break;
            case CropType.Corn:
                value = 5;
                break;
            case CropType.Broccoli:
                value = 4;
                break;
            case CropType.Tomato:
                value = 3;
                break;
            case CropType.Carrot:
                value = 2;
                break;
            default: break;
        }
    }



    public void seed(CropType type)
    {
        cropType = type;
        switch (type)
        {
            case CropType.wheat:
                maxStage = 2;
                break;
            case CropType.Corn:
                maxStage = 2;
                break;
            case CropType.Broccoli:
                maxStage = 2;
                break;
            case CropType.Tomato:
                maxStage = 3;
                break;
            case CropType.Carrot:
                maxStage = 3;
                break;
            default: break;
        }
    }
    public void water()
    {
        if(stage < maxStage && cropType != CropType.Empty)
        {
            stage++;
        }
    }
    public int setVal() // ��ȡ���ؼ�ֵ
    {
        value = 0;
        switch (cropType)
        {
            case CropType.Empty: //�޼�ֵ
                return value;
            case CropType.wheat:
                value += 6 * stage;
                break;
            case CropType.Corn:
                value += 5 * stage;
                break;
            case CropType.Broccoli:
                value += 4 * stage;
                break;
            case CropType.Tomato:
                value += 3 * stage;
                break;
            case CropType.Carrot:
                value += 2 * stage;
                break;
            default: break;
        }
        if (fertilized)
        {
            value += 10;
        }
        return value;
    }
}

public class LandChunkPosition
{
    public int x;
    public int y;
}


public class LandChunk
{
    public Land[,] unitLand = new Land[5, 5];
    public bool fertilized = false; // δʩ��
    public bool watered = false;
    public int unplantedNum; // ����������δ��ֲ��ֲ����
    public int totalValue;
    public bool cultivated = false; // δ����
    public LandChunkPosition pos = new LandChunkPosition(); //ʵ������FarmMatrix���õ�index
    public Vector2 worldPosition; //�������꣬��������
    public int chunkID;
    public int HumidityValue; // ʪ��ֵ
    public bool harvesterOccupied = false; //�Ƿ������ո��

    public bool bOccupied = false; // �Ƿ��ǲ��ֵ�Ŀ��
    public bool fOccupied = false; // �Ƿ���ʩ�ʵ�Ŀ��
    public bool wOccupied = false; // �Ƿ��ǽ�ˮ��Ŀ��
    public void setPlantNum() // ��������������δ��ֲ��ֲ����
    {
        unplantedNum = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                unplantedNum += unitLand[i, j].cropType == CropType.Empty ? 1 : 0;
            }
        }
    }
    public void calVal() // �����ܼ�ֵ
    {
        totalValue = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                unitLand[i, j].UpdateValue();
                //totalValue += unitLand[i, j].setVal();
                totalValue += unitLand[i, j].value;
            }
        }
    }
    public void initLand()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                Land land = new Land();
                land.landID = i * 5 + j;
                land.cropType = CropType.Empty;
                land.stage = 0;
                land.maxStage = 0;
                land.fertilized = false;
                unitLand[i, j] = land;
            }
        }
    }
}


public class Farm : MonoBehaviour
{
    public static int FarmScale = 32;
    // public static int Origin = FarmScale / 2;
    public static int Origin = 0;

    public LandChunk[,] FarmMatrix = new LandChunk[FarmScale, FarmScale]; //��ά���󣬰���һ����������chunk������ʵ������ά����
    public FarmGenerator farmGenerator;
    public void fertilizeLandChunk(int chunkID) //ΪһƬ����ʩ��
    {
        LandChunk selectedLandChunk = FarmMatrix[Mathf.FloorToInt(chunkID / FarmScale), chunkID % FarmScale];
        selectedLandChunk.fertilized = true;
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if(selectedLandChunk.unitLand[i, j].cropType != CropType.Empty)
                {
                    selectedLandChunk.unitLand[i, j].fertilized = true;
                }
            }
        }
    }
    public void broacastSeedsInLand(int chunkID, int landID, CropType type) //�������ز���
    {
        LandChunk selectedLandChunk = FarmMatrix[Mathf.FloorToInt(chunkID / FarmScale), chunkID % FarmScale];
        Land selectedLand = selectedLandChunk.unitLand[Mathf.FloorToInt(landID / 5), landID % 5];
        if(selectedLand.cropType == CropType.Empty)
        {
            selectedLand.seed(type);
            selectedLandChunk.unplantedNum--;
        }

    }
    public void waterLandChunk(int chunkID) //ΪһƬ���ؽ�ˮ
    {
        LandChunk selectedLandChunk = FarmMatrix[Mathf.FloorToInt(chunkID / FarmScale), chunkID % FarmScale];
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                selectedLandChunk.unitLand[i, j].water();
            }
        }
        selectedLandChunk.HumidityValue += 30;
    }
    public void reclaimLandChunk(int i, int j) // ����һƬ����
    {
        
        //LandChunk selectedLandChunk = FarmMatrix[i, j];
        //selectedLandChunk.cultivated = true;
        FarmMatrix[i, j].cultivated = true;
    }

    public void InitFarm()
    {
        for (int i = 0; i < FarmScale; i++)
        {
            for (int j = 0; j < FarmScale; j++)
            {
                FarmMatrix[i, j].pos.x = i;
                FarmMatrix[i, j].pos.y = j;
                FarmMatrix[i, j].chunkID = i * FarmScale + j;
                FarmMatrix[i, j].initLand();
                FarmMatrix[i, j].setPlantNum();
                FarmMatrix[i, j].calVal();
                FarmMatrix[i, j].HumidityValue = 0;
            }
        }
        // ��ʼ��ũ��
        FarmMatrix[Origin, Origin].cultivated = true; // ��ʼ�ѿ��ѵĵ�
    }


    public void SaveFarm()
    {
        GameManager.instance.game_state.FarmMatrix = FarmMatrix.Clone() as LandChunk[,];
        Debug.Log("Farm Saved");
    }

    public void LoadFarm()
    {
        FarmMatrix = GameManager.instance.game_state.FarmMatrix.Clone() as LandChunk[,];
        Debug.Log("Farm Loaded");
    }


    public void UpdateFarm()
    {
        for (int i = 0; i < FarmScale; i++)
        {
            for (int j = 0; j < FarmScale; j++)
            {
                FarmMatrix[i, j].calVal();
            }
        }
    }
    public GameObject getChunkInScene(int targetID)
    {
        List<GameObject> TEMP = farmGenerator.all_chunk_land;
        for (int i = 0; i < farmGenerator.all_chunk_land.Count; i++)
        {
            if (TEMP[i].transform.GetChild(0).
                GetComponent<RecordChunkAttribute>().landChunk.chunkID == targetID)
            {
                return TEMP[i].transform.GetChild(0).gameObject;
            }
        }
        return null;
    }

    private void Awake()
    {

        farmGenerator = GetComponent<FarmGenerator>();
        //farmGenerator.all_chunk_land


        if (GameManager.instance.game_state.FarmMatrix == null)
        {
            for (int i = 0; i < FarmScale; i++)
            {
                for (int j = 0; j < FarmScale; j++)
                {
                    LandChunk landChunk = new LandChunk();
                    landChunk.pos.x = i;
                    landChunk.pos.y = j;
                    landChunk.chunkID = i * FarmScale + j;
                    landChunk.initLand();
                    landChunk.setPlantNum();
                    landChunk.calVal();
                    landChunk.HumidityValue = 0;
                    FarmMatrix[i, j] = landChunk;
                }
            }
            SaveFarm();
        }
        else
        {
            LoadFarm();
        }
        
        // ��ʼ��ũ��
        FarmMatrix[Origin, Origin].cultivated = true; // ��ʼ�ѿ��ѵĵ�
    }

    public void Update()
    {
        UpdateFarm();
    }
}
