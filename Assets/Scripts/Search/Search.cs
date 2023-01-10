using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;


/*
 A*过程：
 1.将开始节点放入开放列表(开始节点的F和G值都视为0);
 2.重复一下步骤:
  i.在开放列表中查找具有最小F值的节点,并把查找到的节点作为当前节点;
  ii.把当前节点从开放列表删除, 加入到封闭列表;
  iii.对当前节点相邻的每一个节点依次执行以下步骤:
   1.如果该相邻节点不可通行或者该相邻节点已经在封闭列表中,则什么操作也不执行,
      继续检验下一个节点;
   2.如果该相邻节点不在开放列表中,则将该节点添加到开放列表中, 并将该相邻节点的父节点设为当前节点,
       同时保存该相邻节点的G和F值;
   3.如果该相邻节点在开放列表中, 则判断若经由当前节点到达该相邻节点的G值是否小于原来保存的G值,
       若小于,则将该相邻节点的父节点设为当前节点,并重新设置该相邻节点的G和F值.
  iv.循环结束条件:
   当终点节点被加入到开放列表作为待检验节点时, 表示路径被找到,此时应终止循环;
   或者当开放列表为空,表明已无可以添加的新节点,而已检验的节点中没有终点节点，
   则意味着路径无法被找到,此时也结束循环;
 3.从终点节点开始沿父节点遍历, 并保存整个遍历到的节点坐标,遍历所得的节点就是最后得到的路径;
 */
public class Search : MonoBehaviour
{
    public Harvester harvester; //每个独立的收割机
    public LandChunk searchTarget; //获取的目标chunk
    public Vector3 searchTargetPosition;
    public Vector2 TEST;
    public Farm farm;
    private void Start()
    {
        farm = GameObject.FindWithTag("Farm").GetComponent<Farm>();
        harvester = GetComponent<Harvester>();
    }

    public LandChunk FindHighestValueChunk(Harvester _harvester)
    {
        int x = (int)_harvester.ChunkIndex.x, y = (int)_harvester.ChunkIndex.y; //Harvester所在的Chunk
        LandChunk targetLandChunk = farm.FarmMatrix[x, y];
        
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j< 2; j++)
            {
                if (x+i >= 0 && y+j >=0 && x+i<9 && y+j <9)
                {
                    //Debug.Log("SearchedChunk:"+(x+i)+ " " + (y+j));
                    LandChunk tempLandChunk;
                    tempLandChunk = farm.FarmMatrix[x + i, y + j];
                    
                    if (tempLandChunk.totalValue >= targetLandChunk.totalValue
                        && tempLandChunk.cultivated == true && tempLandChunk.harvesterOccupied == false)
                    {
                        //harvester.currentChunk.harvesterOccupied = false;
                        //tempLandChunk.harvesterOccupied = true;
                        targetLandChunk = tempLandChunk;
                    }
                }
            }
        }
        
        return targetLandChunk;
    }

    public void FixedUpdate()
    {

        //new Vector3(chunk_i * 5 + i*5, 0, chunk_j * 5 + j*5);

        searchTarget = FindHighestValueChunk(GetComponent<Harvester>());
        searchTargetPosition = new Vector3
        {
            x = searchTarget.pos.x * 5,
            y = 0,
            z = searchTarget.pos.y * 5,
        };

        //TEST = new Vector2 { x = searchTarget.pos.x, y = searchTarget.pos.y };
    }
}
