using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace SpeedBus.GUI.ButtonPrompts
{
    public class ButtonPromptText : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private TextMeshProUGUI _text;

        public ButtonPromptButtonInfo info;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _text = GetComponent<TextMeshProUGUI>();
            UpdatePrompt("Test", info);
        }

        private const int ImagePieceSize = 24;
        private const int ImageTextPadding = 2;

        public void UpdatePrompt(string newText, ButtonPromptButtonInfo info)
        {
            switch (info.SpriteLayout)
            {
                case ButtonPromptButtonInfo.ButtonPromptSpriteLayout.Single:
                    _rectTransform.anchoredPosition = new Vector2(ImagePieceSize * 1 + ImageTextPadding, 0);
                    break;
                case ButtonPromptButtonInfo.ButtonPromptSpriteLayout.MultiLinear:
                    _rectTransform.anchoredPosition = new Vector2(ImagePieceSize * info.SpriteSize + ImageTextPadding, 0);
                    break;
                case ButtonPromptButtonInfo.ButtonPromptSpriteLayout.MultiSquare:
                    _rectTransform.anchoredPosition = new Vector2(ImagePieceSize * info.SpriteSize + ImageTextPadding, ImagePieceSize / 2);
                    break;
            }
            _text.text = newText;
        }
    }
}
