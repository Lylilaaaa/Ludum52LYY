using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;


/*
 A*���̣�
 1.����ʼ�ڵ���뿪���б�(��ʼ�ڵ��F��Gֵ����Ϊ0);
 2.�ظ�һ�²���:
  i.�ڿ����б��в��Ҿ�����СFֵ�Ľڵ�,���Ѳ��ҵ��Ľڵ���Ϊ��ǰ�ڵ�;
  ii.�ѵ�ǰ�ڵ�ӿ����б�ɾ��, ���뵽����б�;
  iii.�Ե�ǰ�ڵ����ڵ�ÿһ���ڵ�����ִ�����²���:
   1.��������ڽڵ㲻��ͨ�л��߸����ڽڵ��Ѿ��ڷ���б���,��ʲô����Ҳ��ִ��,
      ����������һ���ڵ�;
   2.��������ڽڵ㲻�ڿ����б���,�򽫸ýڵ���ӵ������б���, ���������ڽڵ�ĸ��ڵ���Ϊ��ǰ�ڵ�,
       ͬʱ��������ڽڵ��G��Fֵ;
   3.��������ڽڵ��ڿ����б���, ���ж������ɵ�ǰ�ڵ㵽������ڽڵ��Gֵ�Ƿ�С��ԭ�������Gֵ,
       ��С��,�򽫸����ڽڵ�ĸ��ڵ���Ϊ��ǰ�ڵ�,���������ø����ڽڵ��G��Fֵ.
  iv.ѭ����������:
   ���յ�ڵ㱻���뵽�����б���Ϊ������ڵ�ʱ, ��ʾ·�����ҵ�,��ʱӦ��ֹѭ��;
   ���ߵ������б�Ϊ��,�������޿�����ӵ��½ڵ�,���Ѽ���Ľڵ���û���յ�ڵ㣬
   ����ζ��·���޷����ҵ�,��ʱҲ����ѭ��;
 3.���յ�ڵ㿪ʼ�ظ��ڵ����, �����������������Ľڵ�����,�������õĽڵ�������õ���·��;
 */
public class Search : MonoBehaviour
{
    public Harvester harvester; //ÿ���������ո��
    public LandChunk searchTarget; //��ȡ��Ŀ��chunk
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
        int x = (int)_harvester.ChunkIndex.x, y = (int)_harvester.ChunkIndex.y; //Harvester���ڵ�Chunk
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
