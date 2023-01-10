using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizeDrone : MonoBehaviour
{
    public int maxSearchRadius = 3; // 最大搜索半径
    public float stayTime = 7f;
    public float minStayTime = 3.5f;
    public int drone_x;
    public int drone_y;
    public Farm farm;
    public float speed;
    public float moveSpeed = 10f;
    public int searchRadius = 1; //初始搜索周围九格
    public bool isStay = true; //是否悬停
    public Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        drone_x = 0;
        drone_y = 0;
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
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

    public void upgrade() // 升级
    {
        if (searchRadius <= maxSearchRadius)
        {
            searchRadius++;
        }
        if (stayTime >= minStayTime)
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
        for (int i = 1; i <= searchRadius; i++) // 从里往外搜索
        {
            for (int x = -1; x <= 1; x++) // 搜索八个方向
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int temp_x = drone_x + x * i;
                    int temp_y = drone_y + y * i;
                    if (temp_x >= 0 && temp_x < 32 && temp_y >= 0 && temp_y < 32)
                    {
                        temp = farm.FarmMatrix[temp_x, temp_y];
                        if (temp.cultivated && !temp.fOccupied && !temp.fertilized)
                        {
                            temp.fOccupied = true;
                            res_x = temp_x;
                            res_y = temp_y;
                            return res_x * 32 + res_y;
                        }
                    }
                }
            }
        }
        return res_x * 32 + res_y;
    }
    IEnumerator moveToDoThings(int targetID) //移动到特定LandChunk的中央
    {
        speed = moveSpeed;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            Vector3 direction = targetPos - transform.position;
            if (Vector3.Distance(transform.position, targetPos) <= 0.5f)
            {
                speed = 0f;
                fertilizeLandChunk(targetID);
                farm.FarmMatrix[Mathf.FloorToInt(targetID / 32), targetID % 32].fOccupied = false;
                yield return new WaitForSeconds(stayTime);
                isStay = true;
                StopCoroutine("moveToDoThings");
            }
            gameObject.transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
        }
    }

    public void fertilizeLandChunk(int chunkID)
    {
        farm.fertilizeLandChunk(chunkID);
    }
}
