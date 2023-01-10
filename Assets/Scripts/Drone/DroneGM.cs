using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGM : MonoBehaviour
{
    public Transform[] spawnPos;

    // ���ַɻ���list
    public List<BroadcastDrone> broadcastDrones;
    public List<FertilizeDrone> fertilizeDrones;
    public List<WaterDrone> waterDrones;

    public GameObject broadcastDrone; // ���ַɻ�
    public GameObject fertilizeDrone; // ʩ�ʷɻ�
    public GameObject waterDrone; // ��ˮ�ɻ�
    //int bcount = 0; // ���ַɻ�����
    //int fcount = 0; // ʩ�ʷɻ�����
    //int wcount = 0; // ��ˮ�ɻ�����

    private void Start()
    {
        spawnPos = new Transform[transform.childCount];
        for(int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = transform.GetChild(i);
        }
        // test
        generateBDrone();
        //generateFDrone();
        generateWDrone();
    }
    public void upgradeBDrone() // �������в��ַɻ�
    {
        for(int i = 0; i < broadcastDrones.Count; i++)
        {
            broadcastDrones[i].upgrade();
        }
    }
    public void upgradeFDrone() // ��������ʩ�ʷɻ�
    {
        for(int i = 0; i < fertilizeDrones.Count; i++)
        {
            fertilizeDrones[i].upgrade();
        }
    }
    public void upgradeWDrone() // �������н�ˮ�ɻ�
    {
        for(int i = 0; i < waterDrones.Count; i++)
        {
            waterDrones[i].upgrade();
        }
    }
    //private void Update()
    //{
    //    //for(int i = 0; i < GameManager.instance.game_state.backpack.Count; i++)
    //    //{
    //    //    if(GameManager.instance.game_state.backpack[i].entity.name == EntityType.Drone_seed)
    //    //    {
    //    //        bcount = GameManager.instance.game_state.backpack[i].amount;
    //    //    }
    //    //    if (GameManager.instance.game_state.backpack[i].entity.name == EntityType.Drone_fertilizer)
    //    //    {
    //    //        fcount = GameManager.instance.game_state.backpack[i].amount;
    //    //    }
    //    //    if (GameManager.instance.game_state.backpack[i].entity.name == EntityType.Drone_water)
    //    //    {
    //    //        wcount = GameManager.instance.game_state.backpack[i].amount;
    //    //    }
    //    //}
    //}
    public void generateBDrone() // ���ɲ��ַɻ�
    {
        //for (int i = 0; i < GameManager.instance.game_state.backpack.Count; i++)
        //{
        //    if (GameManager.instance.game_state.backpack[i].entity.name == EntityType.Drone_seed)
        //    {
        //        GameManager.instance.game_state.backpack[i].amount--;
        //        break;
        //    }
        //}
        GameObject drone = Instantiate(broadcastDrone, spawnPos[Random.Range(0, spawnPos.Length)].position, Quaternion.identity);
        broadcastDrones.Add(drone.GetComponent<BroadcastDrone>());
        //����һ���������Ƹ�ֵ
        Quaternion tempRotation = new Quaternion
        {
            x = drone.transform.rotation.x - 1,
            y = drone.transform.rotation.y,
            z = drone.transform.rotation.z,
            w = drone.transform.rotation.w
        };
        drone.transform.rotation = tempRotation;
        //bcount++;
    }
    public void generateFDrone() // ����ʩ�ʷɻ�
    {
        //for (int i = 0; i < GameManager.instance.game_state.backpack.Count; i++)
        //{
        //    if (GameManager.instance.game_state.backpack[i].entity.name == EntityType.Drone_fertilizer)
        //    {
        //        GameManager.instance.game_state.backpack[i].amount--;
        //        break;
        //    }
        //}

        GameObject drone = Instantiate(fertilizeDrone, spawnPos[Random.Range(0, spawnPos.Length)].position, Quaternion.identity);
        fertilizeDrones.Add(drone.GetComponent<FertilizeDrone>());
        Quaternion tempRotation = new Quaternion
        {
            x = drone.transform.rotation.x - 1,
            y = drone.transform.rotation.y,
            z = drone.transform.rotation.z,
            w = drone.transform.rotation.w
        };
        drone.transform.rotation = tempRotation;
        //fcount++;
    }
    public void generateWDrone() // ���ɽ�ˮ�ɻ�
    {
        //for (int i = 0; i < GameManager.instance.game_state.backpack.Count; i++)
        //{
        //    if (GameManager.instance.game_state.backpack[i].entity.name == EntityType.Drone_water)
        //    {
        //        GameManager.instance.game_state.backpack[i].amount--;
        //        break;
        //    }
        //}
        GameObject drone = Instantiate(waterDrone, spawnPos[Random.Range(0, spawnPos.Length)].position, Quaternion.identity);
        waterDrones.Add(drone.GetComponent<WaterDrone>());
        Quaternion tempRotation = new Quaternion
        {
            x = drone.transform.rotation.x - 1,
            y = drone.transform.rotation.y,
            z = drone.transform.rotation.z,
            w = drone.transform.rotation.w
        };
        drone.transform.rotation = tempRotation;
        //wcount++;
    }
}
