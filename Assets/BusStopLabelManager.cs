using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SpeedBus.Gameplay;

namespace SpeedBus.GUI
{
    public class BusStopLabelManager : MonoBehaviour
    {
        public GameObject labelPrefab;

        List<GameObject> labels = new List<GameObject>();
        BusStop[] stops;

        private void Start()
        {
            stops = FindObjectsOfType<BusStop>();
            foreach (BusStop stop in stops)
            {
                labels.Add(Instantiate(labelPrefab, transform));
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(stops[i].transform.position);
                labels[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{stops[i].DisplayName} ({stops[i].WaitingPassengers.Count})";
            }
        }
    }
}