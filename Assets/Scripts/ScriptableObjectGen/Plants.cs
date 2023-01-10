
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]
public class Plants : ScriptableObject
{
    public string plantName;
    public int matureTime;   //一共需要多少阶段变成可收割
    public float value;    //价格
    public float growSpeed;    //需要多少秒成熟一个阶段，是1/速度
    public int seedHold;
}
