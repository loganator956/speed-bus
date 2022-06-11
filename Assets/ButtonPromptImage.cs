using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace SpeedBus.GUI.ButtonPrompts
{
    public class ButtonPromptImage : MonoBehaviour
    {
        public Image[] Images;
        /* 0 - 11
         * 1 - 21
         * 2 - 12
         * 3 - 22
         * 4 - 32
         */

        private readonly string[] PieceOrder = { "11", "21", "12", "22", "32" };

        public ButtonPromptButtonInfo CurrentButton { get; private set; }

        private InputAction _currentAction;

        private ButtonPromptsSpritesManager _spritesManager;

        private bool _isActionPressed = false;
        public bool IsActionPressed
        {
            get { return _isActionPressed; }
            set
            {
                _isActionPressed = value;
                UpdateImage(_isActionPressed);
            }
        }

        private void Awake()
        {
            _spritesManager = FindObjectOfType<ButtonPromptsSpritesManager>();
            ClearSprites();
            Images[0].enabled = true;
        }

        public void SetButton(ButtonPromptButtonInfo info)
        {
            if (_currentAction != null)
            {
                _currentAction.canceled -= _currentAction_canceled;
                _currentAction.started -= _currentAction_started;
            }
            CurrentButton = info;
            IsActionPressed = false;

            _currentAction = CurrentButton.Action;
            _currentAction.started += _currentAction_started;
            _currentAction.canceled += _currentAction_canceled;
        }

        private void _currentAction_canceled(InputAction.CallbackContext obj)
        {
            IsActionPressed = false;
        }

        private void _currentAction_started(InputAction.CallbackContext obj)
        {
            IsActionPressed = true;
        }

        private void UpdateImage(bool isPressed)
        {
            switch (CurrentButton.SpriteLayout)
            {
                case ButtonPromptButtonInfo.ButtonPromptSpriteLayout.Single:
                    // use 12
                    ClearSprites();
                    Images[2].enabled = true;
                    Images[2].sprite = GetSprite(CurrentButton, "", isPressed);
                    break;
                case ButtonPromptButtonInfo.ButtonPromptSpriteLayout.MultiLinear:
                    // use 12 22 32 (Depending on how many sprites there are)
                    ClearSprites();
                    for (int i = 0; i < CurrentButton.SpriteSize; i++)
                    {
                        Images[i + 2].enabled = true;
                        Images[i + 2].sprite = GetSprite(CurrentButton, PieceOrder[i + 2], isPressed);
                    }
                    break;
                case ButtonPromptButtonInfo.ButtonPromptSpriteLayout.MultiSquare:
                    // currently only support 2x2 squares (as that is the largest sprite we got)
                    ClearSprites();
                    for (int i =0; i < 4; i++)
                    {
                        Images[i].enabled = true;
                        Images[i].sprite = GetSprite(CurrentButton, PieceOrder[i], isPressed);
                    }
                    break;
            }
        }

        private Sprite GetSprite(ButtonPromptButtonInfo info, string piece, bool isPressed)
        {
            Sprite sprite;
            string key = $"{info.Name}{piece}_{(isPressed ? "1" : "0")}";
            if (_spritesManager.PromptSprites.TryGetValue(key, out sprite))
            {
                return sprite;
            }
            else
            {
                throw new System.Exception($"Couldn't find the button prompt sprite :( of key {key}");
            }
        }
        

        private void ClearSprites()
        {
            for (int i = 0; i < Images.Length; i++)
            {
                Images[i].sprite = null;
                Images[i].enabled = false;
            }
        }
    }
}