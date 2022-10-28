using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using SpeedBus.Gameplay.Passengers;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DebugMenuMainText : MonoBehaviour
{
    private TextMeshProUGUI _textBox;

    private Transform _playerTransform;
    private TopDownVehicleController _topDownVehicleController;
    private PlayerInput _playerInput;
    private PassengerCarriage _passengerCarriage;

    private BusStop[] _busStops;

    private void Awake()
    {
        _textBox = GetComponent<TextMeshProUGUI>();

        _topDownVehicleController = FindObjectOfType<TopDownVehicleController>();
        _playerTransform = _topDownVehicleController.transform;
        _playerInput = _topDownVehicleController.GetComponent<PlayerInput>();
        _passengerCarriage = _topDownVehicleController.GetComponent<PassengerCarriage>();

        _busStops = FindObjectsOfType<BusStop>();
    }

    private void Update()
    {
        _textBox.text =
@$"Player Position : {_playerTransform.position}
Player Input Actions Enabled (Current Map) :
{GetInputActionsString()}
Passengers
{GetBusStopsString()}
On Bus : {_passengerCarriage.PassengersList.Count}
Total : {GetTotalPassengers()}
Targets : 
{GetPassengerTargetsCount()}";
    }

    private string GetInputActionsString()
    {
        string val = "";
        foreach (InputAction action in _playerInput.currentActionMap.actions)
        {
            val += $"{action.name} : {ColourText(action.enabled)}\n";
        }
        return val.TrimEnd('\n');
    }

    private string GetBusStopsString()
    {
        string val = "";
        foreach(BusStop stop in _busStops)
        {
            val += $"{stop.name} : {stop.Passengers.Count}\n";
        }
        return val.TrimEnd('\n');
    }

    private int GetTotalPassengers()
    {
        int x = _passengerCarriage.PassengersList.Count;
        foreach(BusStop stop in _busStops)
        {
            x += stop.Passengers.Count;
        }
        return x;
    }

    private string GetPassengerTargetsCount()
    {
        // I'm aware this is terrible, it's only temporary for testing
        string val = "";
        foreach (BusStop stop in _busStops)
        {
            int x = 0;
            foreach (Passenger passenger in _passengerCarriage.PassengersList)
            {
                if (passenger.TargetStop == stop)
                    x++;
            }
            foreach (BusStop s in _busStops)
            {
                foreach(Passenger pass in s.Passengers)
                {
                    if (pass.TargetStop == stop)
                        x++;
                }
            }
            val += $"{stop.name} : {x}\n";
        }
        return val.TrimEnd('\n');
    }

    const string ColourBoolFalse = "<color=\"red\">";
    const string ColourBoolTrue = "<color=\"green\">";

    private string ColourText(bool b)
    {
        string val = "";

        val = $"{(b ? ColourBoolTrue : ColourBoolFalse)}{b}</color>";

        return val;
    }
}
