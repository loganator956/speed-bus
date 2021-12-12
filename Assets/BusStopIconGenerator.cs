using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopIconGenerator : MonoBehaviour
{
    public GameObject IconPrefab;
    // Start is called before the first frame update
    void Start()
    {
        BusStopScript[] busStops = FindObjectsOfType<BusStopScript>();
        for (int i = 0; i < busStops.Length; i++)
        {
            GameObject newObj = Instantiate(IconPrefab, transform);
            BusStopIcon icon = newObj.GetComponent<BusStopIcon>();
            icon.Target = busStops[i].transform;
            icon.BusStop = busStops[i];
        }
    }
}
