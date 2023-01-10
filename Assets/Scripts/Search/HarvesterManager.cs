using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvesterManager : MonoBehaviour
{
    public List<Harvester> harvesterList;
    //public List<BroadcastDrone> broadcastDrones;
    //public List<FertilizeDrone> fertilizeDrones;
    //public List<WaterDrone> waterDrones;

    public GameObject harvesterPrefeb;
    //public GameObject broadcastDronePrefeb;
    //public GameObject fertilizeDrone;
    //public GameObject waterDronePrefeb;


    public void Start()
    {
    }

    public Harvester GenerateNewHarvester(Vector3 GeneratePosition)
    {
        GameObject harvester = Instantiate(harvesterPrefeb, GeneratePosition, Quaternion.identity);

        return harvester.GetComponent<Harvester>();
    }

    public void AddHavester()
    {
        harvesterList.Add(GenerateNewHarvester(new Vector3(0, 0, 0)));
    }

    //public BroadcastDrone GenerateNewBroadcastDrone(Vector3 GeneratePosition)
    //{
    //    GameObject harvester = Instantiate(broadcastDronePrefeb, GeneratePosition, Quaternion.identity);

    //    return harvester.GetComponent<BroadcastDrone>();
    //}

    //public void AddBroadcastDrone()
    //{
    //    broadcastDrones.Add(GenerateNewBroadcastDrone(new Vector3(0, 10, 0)));
    //}

    //public FertilizeDrone GenerateNewFertilizeDrone(Vector3 GeneratePosition)
    //{
    //    GameObject harvester = Instantiate(fertilizeDrone, GeneratePosition, Quaternion.identity);

    //    return harvester.GetComponent<FertilizeDrone>();
    //}

    //public void AddFertilizeDrone()
    //{
    //    fertilizeDrones.Add(GenerateNewFertilizeDrone(new Vector3(0, 10, 0)));
    //}

    //public WaterDrone GenerateWaterDrone(Vector3 GeneratePosition)
    //{
    //    GameObject harvester = Instantiate(waterDronePrefeb, GeneratePosition, Quaternion.identity);

    //    return harvester.GetComponent<WaterDrone>();
    //}

    //public void AddWaterDrone()
    //{
    //    waterDrones.Add(GenerateWaterDrone(new Vector3(0, 10, 0)));
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            harvesterList.Add(GenerateNewHarvester(new Vector3(0,0,0)));
        }

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    broadcastDrones.Add(GenerateNewBroadcastDrone(new Vector3(0, 10, 0)));
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    fertilizeDrones.Add(GenerateNewFertilizeDrone(new Vector3(0, 10, 0)));
        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    waterDrones.Add(GenerateWaterDrone(new Vector3(0, 10, 0)));
        //}

    }

}
