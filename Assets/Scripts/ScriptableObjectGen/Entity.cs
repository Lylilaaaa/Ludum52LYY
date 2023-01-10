using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity")]
public class Entity: ScriptableObject
{
    public EntityType name;
    public int price;
    public int id;
    public Sprite image;
}
