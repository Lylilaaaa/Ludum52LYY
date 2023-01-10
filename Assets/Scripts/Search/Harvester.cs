using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum HarvesterState
{
    Searching,
    MovingToNewChunk,
    Harvesting,
    FollowOrder
}

public class Harvester : MonoBehaviour
{
    public HarvesterState harvesterState= HarvesterState.Searching;
    public Farm farm;
    public Search search;
    public LandChunk searchTarget;
    public LandChunk currentChunk;
    public Vector3 targetLandPosition;
    public List<Vector3> harvesterPoints;
    public Vector2 ChunkIndex = new Vector2(-1,-1), LocalPositionInChunk;
    public bool startHarvest = false;
    public int HarvestIndex= 0;

    public bool rotateState1, rotateState2;

    public int level = 1;
    public float speed = 1f;

    public void Upgrade()
    {
        if (level < 5)
        {
            level++;
            speed += 0.25f;
        }
    }
    public void Start()
    {

        search = GetComponent<Search>();
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
    }
    public void Update()
    {
        Vector3 temp = transform.position;
        temp.y = 0.15f;
        transform.position = temp;

        Quaternion _temp = transform.rotation;
        _temp.x = 0;
        _temp.z = 0;
        transform.rotation = _temp;
    }
    public void FixedUpdate()
    {   
        if(harvesterState== HarvesterState.Searching)
        {
            //这行会报错，不用管
            currentChunk.harvesterOccupied = false;
            

            searchTarget = search.FindHighestValueChunk(this);
            searchTarget.harvesterOccupied = true;
            if(searchTarget.totalValue != 0)
            {
                harvesterState = HarvesterState.MovingToNewChunk;

                startHarvest = true;
            }

        }

        if(harvesterState == HarvesterState.MovingToNewChunk)
        {
            MoveToTargetChunk(searchTarget);
            if(GetComponent<Rigidbody>().velocity == Vector3.zero ) 
            {
                Debug.Log(1114444455111166666);
                harvesterState = HarvesterState.Harvesting;
            }
        }

        if (harvesterState == HarvesterState.Harvesting)
        {
            if (currentChunk.totalValue != 0)
            {
                HarvesterWholeChunk();
            }
            else
            {
                harvesterState = HarvesterState.Searching;
            }

        }
        Vector3 temp = transform.position;
        temp.y = 0.15f;
        transform.position = temp;

        Quaternion _temp = transform.rotation;
        _temp.x = 0;
        _temp.z = 0;
        transform.rotation = _temp;
    }


    public void HarvesterWholeChunk()//Vector2 direction)
    {
        Vector3 start = new Vector3
        { x = currentChunk.pos.x *5, y = 0, z = currentChunk.pos.y *5 },
        up = new Vector3 { x = 0,z = 4}, right = new Vector3 { x = 1, z =0};

        if (startHarvest)
        {
            harvesterPoints.Add(start);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] - up);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count-1] + right);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] + up);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] + right);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] -up);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] + right);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] +up);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] + right);
            harvesterPoints.Add(harvesterPoints[harvesterPoints.Count - 1] -up);

            startHarvest = false;
        }

        //Move(harvesterPoints[HarvestIndex]);
        transform.position = Vector3.MoveTowards(transform.position,
            harvesterPoints[HarvestIndex],
            3f * Time.deltaTime *speed);
        transform.LookAt(harvesterPoints[HarvestIndex]);

        if ((Vector3.Distance(harvesterPoints[HarvestIndex], transform.position) < 0.1f))
        {

            HarvestIndex++;
            if (HarvestIndex == harvesterPoints.Count)
            {
                //临时效果，将本片chunk value清零
                //currentChunk.totalValue = 0;


                harvesterState = HarvesterState.Searching;
                harvesterPoints = new List<Vector3>();
                HarvestIndex = 0;
            }
            
        }
    }

    public static Quaternion GetLookAtEuler(Transform originalObj, Vector3 targetPoint)
    {
        //计算物体在朝向某个向量后的正前方
        Vector3 forwardDir = targetPoint - originalObj.position;


        //计算朝向这个正前方时的物体四元数值
        Quaternion lookAtRot = Quaternion.LookRotation(forwardDir);

        return lookAtRot;
    }

    public void Rotate(Vector3 targetPosition)
    {
        Quaternion dir = Quaternion.FromToRotation(this.transform.position, targetPosition);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, dir, 1f);
    }

    public void Move(Vector3 targetPosition)
    {

        Vector3 midPoint = new Vector3
        {
            x = transform.position.x,
            y= transform.position.y,
            z = targetPosition.z
        };

        Vector3 tempFinalPosition = new Vector3
        {
            x = targetPosition.x,
            z = targetPosition.z,
            y = transform.position.y
        };

        Quaternion midPointRotation = GetLookAtEuler(transform, midPoint);
        Quaternion finalPointRotation = GetLookAtEuler(transform, targetPosition);

        //if (!rotateState1)
        //{
        //    transform.rotation = midPointRotation;
        //    rotateState1 = true;
        //}
        //if(!rotateState2)
        //{

        //}

        if (Mathf.Abs(transform.position.z - midPoint.z) > 0.5f)
        {
            transform.rotation = midPointRotation;
            transform.position = Vector3.MoveTowards(transform.position,
                midPoint, 3f * Time.deltaTime *speed);
        }
        else if (Vector3.Distance(transform.position, tempFinalPosition)>0.5f)
        {
            transform.rotation = finalPointRotation;
            transform.position = Vector3.MoveTowards(transform.position,
                targetPosition, 3f * Time.deltaTime *speed);
        }
        else
        {
            //harvesterState = HarvesterState.Harvesting;
        }
    }

    public void MoveToTargetChunk(LandChunk targetChunk)
        {

        //下面这个坐标移动到的位置实际是chunk的左上角
        targetLandPosition = new Vector3
        {
            x = searchTarget.pos.x *5,
            y = 0.25f,
            z = searchTarget.pos.y *5 ,
        };

        //transform.position = Vector3.MoveTowards(transform.position,
        //    targetLandPosition,
        //    10f * Time.deltaTime
        //);

        Move(targetLandPosition);


        if (Vector3.Distance(targetLandPosition, transform.position) < 0.5f)
        {
            harvesterState = HarvesterState.Harvesting;
        }
    }

    public void OnTriggerEnter(Collider trigger)
    {
        // Debug.Log("TRIGGER");
        //更新所处的chunk
        if (trigger.transform.tag == "LandChunk")
        {
            ChunkIndex = new Vector2
            {
                x = trigger.transform.GetComponent<RecordChunkAttribute>().chunkIndex_x,
                y = trigger.transform.GetComponent<RecordChunkAttribute>().chunkIndex_y,
            };
            currentChunk = farm.FarmMatrix[(int) ChunkIndex.x,(int) ChunkIndex.y];
        }

        if(trigger.transform.tag == "Plant" && harvesterState == HarvesterState.Harvesting)
        {
            Debug.Log(trigger.transform.name);
            trigger.transform.GetComponent<UIShownInformation>().HarvestorActivate = true;
        }
    }
}
