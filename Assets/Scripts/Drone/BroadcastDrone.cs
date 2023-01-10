using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastDrone : MonoBehaviour
{
    public int maxSearchRadius = 3; // 最大搜索半径
    public float stayTime = 5f;
    public float minStayTime = 2f;

    public int drone_x; //实际等于chunk index
    public int drone_y;

    public Farm farm;
    public float speed;
    public float moveSpeed = 10f;
    public int searchRadius = 1; //初始搜索周围九格
    public bool isStay = true; //是否悬停
    public Vector3 targetPos;
    public AudioSource musicPlay;

    public FarmGenerator farmGenerator;
    // Start is called before the first frame update
    void Start()
    {


        drone_x = 0;
        drone_y = 0;
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
        farmGenerator = GameObject.FindWithTag("Farm").GetComponent<FarmGenerator>();
        musicPlay = GetComponent<AudioSource>();
        speed = 0f; // stay
    }
    private void Update()
    {
        if (isStay)
        {
            int targetID = searchTarget();
            if (drone_x * 32 + drone_y != targetID)
            {
                int target_x = Mathf.FloorToInt(targetID / 32);
                int target_y = targetID % 32;
                targetPos = new Vector3(target_x * 5 + 2.5f, transform.position.y, target_y * 5 - 2.5f);
                isStay = false;
                StartCoroutine("moveToDoThings", targetID);
            }
        }
    }

    public GameObject getChunkInScene(int targetID)
    {
        List<GameObject> TEMP = farmGenerator.all_chunk_land;
        for(int i= 0; i< farmGenerator.all_chunk_land.Count; i++)
        {
            if (TEMP[i].transform.GetChild(0).
                GetComponent<RecordChunkAttribute>().landChunk.chunkID == targetID)
            {
                return TEMP[i].transform.GetChild(0).gameObject;
            }
        }
        return null;
    }

    public void upgrade() // 升级
    {
        if(searchRadius <= maxSearchRadius)
        {
            searchRadius++;
        }
        if(stayTime >= minStayTime)
        {
            stayTime -= 0.5f;
        }
    }
    public int searchTarget()
    {
        drone_x = Mathf.FloorToInt((transform.position.x) / 5);
        drone_y = Mathf.FloorToInt((transform.position.z + 3) / 5);
        int res_x = drone_x;
        int res_y = drone_y;
        LandChunk temp = farm.FarmMatrix[0, 0];
        //int largestNum = 0;
        for (int i = 1; i <= searchRadius; i++) // 从里往外搜索
        {
            for (int x = -1; x <= 1; x++) // 搜索八个方向
            {
                for (int y = -1; y <= 1; y++)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }
                    int temp_x = drone_x + x * i;
                    int temp_y = drone_y + y * i;
                    if (temp_x >= 0 && temp_x < 32 && temp_y >= 0 && temp_y < 32)
                    {
                        temp = farm.FarmMatrix[temp_x, temp_y];
                        //if (temp.cultivated && !temp.bOccupied && temp.unplantedNum > largestNum)
                        //{
                        //    res_x = temp_x;
                        //    res_y = temp_y;
                        //    largestNum = temp.unplantedNum;
                        //}
                        if (temp.cultivated && !temp.bOccupied && temp.unplantedNum == 25)
                        {
                            Debug.Log(temp.unplantedNum);
                            temp.bOccupied = true;
                            res_x = temp_x;
                            res_y = temp_y;
                            return res_x * 32 + res_y;
                        }
                    }
                }
                //if (largestNum != 0 && !temp.bOccupied)
                //{
                //    temp.bOccupied = true;
                //    return res_x * 32 + res_y;
                //}
            }
        }
        return res_x * 32 + res_y;
    }
    IEnumerator moveToDoThings(int targetID) //移动到特定LandChunk的中央
    {
        speed = moveSpeed;
        musicPlay.Play();
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            Vector3 direction = targetPos - transform.position;
            if (Vector3.Distance(transform.position, targetPos) <= 0.5f)
            {
                speed = 0f;
                int seedIndex = chooseFirstSeed();
                if(seedIndex != -1)
                {
                    CropType type = CropType.Broccoli;
                    switch (GameManager.instance.game_state.backpack[seedIndex].entity.name)
                    {
                        case EntityType.Broccoli_seed:
                            type = CropType.Broccoli;
                            break;
                        case EntityType.Carrot_seed:
                            type = CropType.Carrot;
                            break;
                        case EntityType.Corn_seed:
                            type = CropType.Corn;
                            break;
                        case EntityType.Tomato_seed:
                            type = CropType.Tomato;
                            break;
                        case EntityType.Wheat_seed:
                            type = CropType.wheat;
                            break;
                        default: break;
                    }
                    GameManager.instance.game_state.backpack[seedIndex].amount += 24;
                    broadcastSeedsLandChunk(targetID, type);
                }
                farm.FarmMatrix[Mathf.FloorToInt(targetID / 32), targetID % 32].bOccupied = false;
                yield return new WaitForSeconds(stayTime);
                isStay = true;
                StopCoroutine("moveToDoThings");
            }
            gameObject.transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
        }
    }
    public int chooseFirstSeed() // 飞机自动选择第一个可种植种子种植
    {
        for (int i = 0; i < GameManager.instance.game_state.backpack.Count; i++)
        {
            if(GameManager.instance.game_state.backpack[i].amount <= 0)
            {
                continue;
            }
            EntityType tempType = GameManager.instance.game_state.backpack[i].entity.name;
            if (tempType == EntityType.Wheat_seed || tempType == EntityType.Corn_seed || tempType == EntityType.Broccoli_seed || tempType == EntityType.Tomato_seed || tempType == EntityType.Carrot_seed)
            {
                return i;
            }
        }
        return -1;
    }
    public void broadcastSeedsLandChunk(int chunkID, CropType type)
    {
        GameObject landChunk = getChunkInScene(chunkID);
        Transform plantHolder = landChunk.transform.Find("PlantsHolder");
        List<GameObject> allLandPos = new List<GameObject>();
        foreach(Transform child in plantHolder.transform)
        {
            foreach(Transform sun in child.transform)
            {
                allLandPos.Add(sun.gameObject);
            }
        }
        for(int i = 0; i < allLandPos.Count; i++)
        {
            switch (type)
            {
                case CropType.Broccoli:
                    landChunk.GetComponent<PlantManagement>().PlantingTheBroccoli(allLandPos[i]);
                    break;
                case CropType.Carrot:
                    landChunk.GetComponent<PlantManagement>().PlantingTheCarrot(allLandPos[i]);
                    break;
                case CropType.Corn:
                    landChunk.GetComponent<PlantManagement>().PlantingTheCorn(allLandPos[i]);
                    break;
                case CropType.Tomato:
                    landChunk.GetComponent<PlantManagement>().PlantingTheTomato(allLandPos[i]);
                    break;
                case CropType.wheat:
                    landChunk.GetComponent<PlantManagement>().PlantingTheWheat(allLandPos[i]);
                    break;
                default: break;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                farm.broacastSeedsInLand(chunkID, i * 5 + j, type);
            }
        }
        //Debug.Log(farm.FarmMatrix[Mathf.FloorToInt(chunkID / 32), chunkID % 32].unplantedNum);
        farm.FarmMatrix[Mathf.FloorToInt(chunkID / 32), chunkID % 32].unplantedNum = 0;
    }
}
