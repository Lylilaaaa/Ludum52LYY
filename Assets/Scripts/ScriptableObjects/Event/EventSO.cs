using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Event", order = 2)]
public class EventSO : ScriptableObject
{
    public float probability;
    public GameObject effect_prefab;
}
