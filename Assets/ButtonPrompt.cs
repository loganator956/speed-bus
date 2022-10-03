using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace SpeedBus.GUI.ButtonPrompts
{
    public class ButtonPrompt : MonoBehaviour
    {
        // TODO: make this integrate with the input system or whatever
        // TODO: remove the style of the prompt and replace it with just the 11 21 31 12 22 32 grid references things
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