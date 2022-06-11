using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpeedBus.GUI.ButtonPrompts
{
    [CreateAssetMenu()]
    public class ButtonPromptButtonInfo : ScriptableObject
    {
        public string Name;
        public InputAction Action;
        /*public Sprite[] UnpressedSprites;*/
        /*public Sprite[] PressedSprites;*/
        public int SpriteSize = 1; // if single = 1, linear = linear, square = one of the sides of the square (_x_*x)
        public ButtonPromptSpriteLayout SpriteLayout;

        public enum ButtonPromptSpriteLayout
        {
            Single, MultiLinear, MultiSquare
        }
    }
}
