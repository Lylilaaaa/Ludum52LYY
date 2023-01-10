using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


[System.Serializable]
public enum EntityType
{
    // plants
    Wheat,
    Corn,
    Broccoli,
    Tomato,
    Carrot,

    // seeds
    Wheat_seed,
    Corn_seed,
    Broccoli_seed,
    Tomato_seed,
    Carrot_seed,

    // machines
    Harvester01,
    Harvester02,

    UFO01,
    UFO02,
    UFO03,

    UFO001_upgraded,
    UFO002_upgraded,
    UFO003_upgraded,

    Drone_water,
    Drone_fertilizer,
    Drone_seed
}

[System.Serializable]

public class BackpackElement
{
    public Entity entity;
    public int amount;
}


[System.Serializable]
public class PrefabMap
{
    public EntityType type;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameState", order = 1)]
public class GameStateSO : ScriptableObject
{

    public int money;

    public EntityType current_harvester;

    public List<BackpackElement> backpack;
    public List<BackpackElement> seed_shop;
    public List<BackpackElement> marchine_shop;

    public List<Harvester> owned_harvesters;

    public LandChunk[,] FarmMatrix;

    public List<PrefabMap> prefab_map;

    public void AddEntityToBackpack(Entity entity, int amount)
    {
        for (int i = 0; i < backpack.Count; i++)
        {
            if (backpack[i].entity.name == entity.name)
            {
                // exists same entity
                backpack[i].amount += amount;
                return;
            }
        }
        // entity first time appear
        BackpackElement new_backpack_element = new BackpackElement();
        new_backpack_element.entity = entity;
        new_backpack_element.amount = amount;
        backpack.Add(new_backpack_element);
    }

}
