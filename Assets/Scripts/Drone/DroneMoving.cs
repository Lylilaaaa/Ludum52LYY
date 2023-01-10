using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMoving : MonoBehaviour
{
    public int drone_x;
    public int drone_y;
    public Farm farm;
    public float speed;
    public float moveSpeed = 5f;
    public int searchRadius = 1; //初始搜索周围九格
    public bool isStay = true; //是否悬停
    public BroadcastDrone drone;
    // Start is called before the first frame update
    void Start()
    {
        drone_x = 0;
        drone_y = 0;
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
        speed = 0f; // stay
        drone = GetComponent<BroadcastDrone>();
    }
    private void Update()
    {
        if (isStay)
        {
            int targetID = searchTarget();
            if (drone_x * 32 + drone_y != targetID)
            {
                isStay = false;
                StartCoroutine("moveToDoThings", targetID);
            }
        }
    }
    public int searchTarget()
    {
        drone_x = Mathf.FloorToInt(transform.position.x / 5);
        drone_y = Mathf.FloorToInt(transform.position.z / 5);
        int res_x = drone_x;
        int res_y = drone_y;
        LandChunk temp;
        int largestNum = 0;
        for (int i = 1; i <= searchRadius; i++) // 从里往外搜索
        {
            for(int x = -1; x <= 1; x++) // 搜索八个方向
            {
                if(x != 0)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (y != 0)
                        {
                            int temp_x = drone_x + x * i;
                            int temp_y = drone_y + y * i;
                            if(temp_x >= 0 && temp_x <= 32 && temp_y >= 0 && temp_y <= 32)
                            {
                                temp = farm.FarmMatrix[temp_x, temp_y];
                                if(temp.cultivated && temp.unplantedNum > largestNum)
                                {
                                    res_x = temp_x;
                                    res_y = temp_y;
                                    largestNum = temp.unplantedNum;
                                }
                            }
                        }
                    }
                    if(largestNum != 0)
                    {
                        return res_x * 32 + res_y;
                    }
                }
            }
        }
        return res_x * 32 + res_y;
    }
    IEnumerator moveToDoThings(int targetID) //移动到特定LandChunk的中央
    {
        int target_x = Mathf.FloorToInt(targetID / 32);
        int target_y = targetID % 32;
        Vector3 direction = new Vector3(target_x - drone_x, 0, target_y - drone_y);
        speed = moveSpeed;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            drone_x = Mathf.FloorToInt(transform.position.x / 5);
            drone_y = Mathf.FloorToInt(transform.position.z / 5);
            if(target_x == drone_x && target_y == drone_y)
            {
                speed = 0f;
                isStay = true;
                drone.broadcastSeedsLandChunk(targetID, CropType.Broccoli);
                StopCoroutine("moveToDoThings");
            }
            gameObject.transform.Translate(direction.normalized * Time.deltaTime * speed, Space.Self);
        }
    }
}
