using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace SpeedBus.GUI.ButtonPrompts
{
    public class ButtonPrompt : MonoBehaviour
    {
        public ButtonPromptImage PromptImage;
        public ButtonPromptText PromptText;
        public void SetButton(string text, ButtonPromptButtonInfo info)
        {
            info.Action.Enable();
            PromptImage.SetButton(info);
            PromptText.UpdatePrompt(text, info);
        }

        public ButtonPromptButtonInfo[] infos;

        private void Update()
        {
            if (((KeyControl)Keyboard.current["1"]).wasPressedThisFrame)
            {
                SetButton("Test 1", infos[0]);
            }
            if (((KeyControl)Keyboard.current["2"]).wasPressedThisFrame)
            {
                SetButton("Test 2", infos[1]);
            }
            if (((KeyControl)Keyboard.current["3"]).wasPressedThisFrame)
            {
                SetButton("Test 3", infos[2]);
            }
        }
    }
}