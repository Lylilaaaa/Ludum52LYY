using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChunkEventType
{
    Normal,
    Drought,
    Raining,
    Lightning
}


public class RandomEvent : MonoBehaviour
{
    #region Variables
    public float[] probArray;
    public ChunkEventType eventType;
    public float eventTimer;
    private float normalTime = 5f;
    private float CDTime = 25f;
    private LandChunk mLandChunk;
    private RecordChunkAttribute recordChunkAttribute;
    public FarmEffect mEffect;
    public int fertilizeTime = 20;
    #endregion
    private void Awake()
    {
        recordChunkAttribute = GetComponent<RecordChunkAttribute>();
        mEffect = GetComponent<FarmEffect>();
        mLandChunk = GameObject.FindWithTag("Farm").GetComponent<Farm>().FarmMatrix[recordChunkAttribute.chunkIndex_x, recordChunkAttribute.chunkIndex_y];
        eventTimer = normalTime;
        probArray = new float[]
        {50,25,15, 5}; // 50概率正常生长，25概率缺水，15概率下雨，5概率被雷劈
    }
    private void Start()
    {
        StartCoroutine("decreasedHumidityValue");
    }
    private void Update()
    {
        if (mLandChunk.cultivated)
        {
            statusCountdown();
        }
    }
    IEnumerator decreasedHumidityValue() //降低湿润值
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            if(mLandChunk.HumidityValue > 0)
            {
                mLandChunk.HumidityValue--;
            }
        }
    }
    IEnumerator fertilizedTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (mLandChunk.fertilized)
            {
                fertilizeTime--;
                if (fertilizeTime <= 0)
                {
                    mLandChunk.fertilized = false;
                    fertilizeTime = 30;
                }
            }
        }
    }
    public void statusCountdown()
    {
        if (eventTimer < 0)
        {
            if (eventType != ChunkEventType.Normal)
            {
                GetComponent<Outline>().enabled = false;
                eventType = ChunkEventType.Normal;
                eventTimer = CDTime;
            }
            else
            {
                eventType = Choose(probArray);
                if (eventType == ChunkEventType.Normal)
                {
                    eventTimer = normalTime;
                }
                else
                {
                    GetComponent<Outline>().enabled = true;
                    switch (eventType)
                    {
                        case ChunkEventType.Drought:
                            eventTimer = mEffect.dry_duration;
                            mLandChunk.HumidityValue = 0;
                            mEffect.Dry();
                            break;
                        case ChunkEventType.Lightning:
                            eventTimer = mEffect.thunder_duration;
                            mEffect.Thunder();
                            break;
                        case ChunkEventType.Raining:
                            eventTimer = mEffect.rain_duration;
                            mLandChunk.HumidityValue += 30;
                            mEffect.Rain();
                            break;
                        default: break;
                    }
                }
            }
        }
        eventTimer -= Time.deltaTime;
    }
    public ChunkEventType Choose(float[] probs)
    {
        //计算总概率
        float total = 0;
        foreach (float elem in probs)
            total += elem;
        //Random.value方法返回一个0—1的随机数
        float randomPoint = Random.value * total;
        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return (ChunkEventType)i;
            else
                randomPoint -= probs[i];
        }
        return ChunkEventType.Raining;
    }
}
