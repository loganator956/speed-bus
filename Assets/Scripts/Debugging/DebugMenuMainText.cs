using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using SpeedBus.Gameplay.Passengers;
using SpeedBus.Gameplay;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DebugMenuMainText : MonoBehaviour
{
    private TextMeshProUGUI _textBox;

    private Transform _playerTransform;
    private TopDownVehicleController _topDownVehicleController;
    private PlayerInput _playerInput;
    private PassengerCarriage _passengerCarriage;

    private BusStop[] _busStops;

    private GameController _gameController;

    private void Awake()
    {
        _textBox = GetComponent<TextMeshProUGUI>();

        _topDownVehicleController = FindObjectOfType<TopDownVehicleController>();
        _playerTransform = _topDownVehicleController.transform;
        _playerInput = _topDownVehicleController.GetComponent<PlayerInput>();
        _passengerCarriage = _topDownVehicleController.GetComponent<PassengerCarriage>();

        _busStops = FindObjectsOfType<BusStop>();

        _gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        _textBox.text =
@$"Player Position : {_playerTransform.position}
Player Input Actions Enabled (Current Map) :
{GetInputActionsString()}
Passengers
{GetBusStopsString()}
On Bus : {_passengerCarriage.PassengerCount}
Total : {GetTotalPassengers()}
Target : {_gameController.CurrentDropOffPoint.name}";
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
            val += $"{stop.name} : {stop.PassengerCount}\n";
        }
        return val.TrimEnd('\n');
    }

    private int GetTotalPassengers()
    {
        int x = _passengerCarriage.PassengerCount;
        foreach(BusStop stop in _busStops)
        {
            x += stop.PassengerCount;
        }
        return x;
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
