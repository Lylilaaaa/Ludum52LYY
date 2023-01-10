using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public enum CropType
{
    Empty,
    UnMature,
    wheat, //小麦
    Corn, //玉米
    Broccoli, //西兰花
    Tomato, //西红柿
    Carrot //胡萝卜
}

public class Land
{
    public CropType cropType;
    public int value;
    public int stage; //阶段
    public int maxStage; //最大阶段
    public int landID;
    public bool fertilized;

    public void UpdateValue()
    {
        switch (cropType)
        {
            case CropType.Empty: //无价值
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
    public int setVal() // 获取土地价值
    {
        value = 0;
        switch (cropType)
        {
            case CropType.Empty: //无价值
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
    public bool fertilized = false; // 未施肥
    public bool watered = false;
    public int unplantedNum; // 所含土地中未种植的植物数
    public int totalValue;
    public bool cultivated = false; // 未开垦
    public LandChunkPosition pos = new LandChunkPosition(); //实际上是FarmMatrix里用的index
    public Vector2 worldPosition; //世界坐标，仅测试用
    public int chunkID;
    public int HumidityValue; // 湿润值
    public bool harvesterOccupied = false; //是否已有收割机

    public bool bOccupied = false; // 是否是播种的目标
    public bool fOccupied = false; // 是否是施肥的目标
    public bool wOccupied = false; // 是否是浇水的目标
    public void setPlantNum() // 计算所含土地中未种植的植物数
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
    public void calVal() // 计算总价值
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

    public LandChunk[,] FarmMatrix = new LandChunk[FarmScale, FarmScale]; //二维矩阵，包含一个个独立的chunk，所以实际是三维矩阵
    public FarmGenerator farmGenerator;
    public void fertilizeLandChunk(int chunkID) //为一片土地施肥
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
    public void broacastSeedsInLand(int chunkID, int landID, CropType type) //单独土地播种
    {
        LandChunk selectedLandChunk = FarmMatrix[Mathf.FloorToInt(chunkID / FarmScale), chunkID % FarmScale];
        Land selectedLand = selectedLandChunk.unitLand[Mathf.FloorToInt(landID / 5), landID % 5];
        if(selectedLand.cropType == CropType.Empty)
        {
            selectedLand.seed(type);
            selectedLandChunk.unplantedNum--;
        }

    }
    public void waterLandChunk(int chunkID) //为一片土地浇水
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
    public void reclaimLandChunk(int i, int j) // 开垦一片土地
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
        // 初始化农田
        FarmMatrix[Origin, Origin].cultivated = true; // 初始已开垦的地
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
        
        // 初始化农田
        FarmMatrix[Origin, Origin].cultivated = true; // 初始已开垦的地
    }

    public void Update()
    {
        UpdateFarm();
    }
}
