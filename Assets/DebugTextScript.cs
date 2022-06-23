using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DebugTextScript : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private GameController _gameController;
    private TopDownVehicleController _topDownVehicleController;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _gameController = FindObjectOfType<GameController>();
        _topDownVehicleController = FindObjectOfType<TopDownVehicleController>();
        _rigidbody2D = _topDownVehicleController.GetComponent<Rigidbody2D>();

        // register event handlers
        _topDownVehicleController.PlayerInput.onControlsChanged += ctx => UpdateData(false);
        UpdateData(true);
    }

    private float _timer = 0f;
    private const float UpdateDelta = 0.2f;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > UpdateDelta)
        {
            _timer = 0f;
            UpdateData(true);
        }
    }

    public const string ColourRed = "#fe4a49";
    public const string ColourBlue = "#2ab7ca";
    public const string ColourYellow = "#fed766";
    public const string ColourGreen = "#88d8b0";

    private float _lastVelocity = 0f;
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    void UpdateData(bool includePhysics)
    {
        // do non physics things here
        _data["Player.Velocity"] = _rigidbody2D.velocity;
        _data["Player.Input.Scheme"] = _topDownVehicleController.PlayerInput.currentControlScheme;
        _data["Game.People.Count"] = _gameController.GetTotalPeople();
        if (includePhysics)
        {
            // do physics things here
            // _lastVelocity is {UpdateDelta} seconds ago
            float acceleration = Mathf.Abs(_lastVelocity - ((Vector2)_data["Player.Velocity"]).magnitude) / UpdateDelta;
            _lastVelocity = ((Vector2)_data["Player.Velocity"]).magnitude;
            _data["Player.Acceleration"] = acceleration;
        }
        RefreshText();
    }

    void RefreshText()
    {
        _text.text = $"<b>Player Stats</b>" +
            $"\nPlayerInput Scheme = <color={ColourBlue}>{(string)_data["Player.Input.Scheme"]}</color>" +
            $"\nPlayerController Velocity = <color={ColourBlue}>{((Vector2)_data["Player.Velocity"]).ToString("")}</color> ({((Vector2)_data["Player.Velocity"]).magnitude.ToString("0.00")} m/s)" +
            $"\nPlayerController Acceleration = <color={ColourBlue}>{((float)_data["Player.Acceleration"]).ToString("0.00")} m/s²</color>" +
            $"\n<b>Game Stats</b>" +
            $"\nPeople Count = <color={ColourBlue}>{((int)_data["Game.People.Count"]).ToString()}</color>";
    }
}
