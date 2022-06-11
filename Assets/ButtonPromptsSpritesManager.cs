using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpeedBus.GUI.ButtonPrompts
{
    public class ButtonPromptsSpritesManager : MonoBehaviour
    {
        public Dictionary<string, Sprite> PromptSprites = new Dictionary<string, Sprite>();
        public Dictionary<string, string> SchemeToSpriteSheetMap = new Dictionary<string, string>();

        public string CurrentDeviceScheme { get; private set; }

        private PlayerInput _playerInput;

        private void Awake()
        {
            SchemeToSpriteSheetMap.Add("keyboard", "pc");
            SchemeToSpriteSheetMap.Add("gamepad", "genericpad");
            SchemeToSpriteSheetMap.Add("playstation controller", "ps");

            _playerInput = FindObjectOfType<PlayerInput>();
            _playerInput.onControlsChanged += _playerInput_onControlsChanged;
        }

        private void _playerInput_onControlsChanged(PlayerInput obj)
        {
            CurrentDeviceScheme = SchemeToSpriteSheetMap[obj.currentControlScheme];
            Debug.Log($"Sprites now changing to {CurrentDeviceScheme}");
            GatherActions();
        }

        /// <summary>
        /// Gather list of possible actions in the current map and their corresponding controls to be displayed in the menu
        /// </summary>
        private void GatherActions()
        {
            foreach (InputAction action in _playerInput.currentActionMap.actions)
            {
                // action.GetBindingDisplayString() returns nothing for axis inputs. Works good for buttons though :) (If there are multiple bindings: will return separated by ' | ')
                /*Debug.Log(action.GetBindingDisplayString());*/
                List<string> controlNames = new List<string>();
                /*foreach (InputControl control in action.controls)
                {
                    Debug.Log($"Control: {action.name}/{control.displayName}");
                    controlNames.Add(control.name);
                }*/
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    string deviceLayout, controlPath;
                    var actionName = action.name;
                    var bindingString = action.GetBindingDisplayString(i, out deviceLayout, out controlPath);
                    // ignore if both deviceLayout and controlPath are empty. 
                    // should use these variables and filter out the ones that aren't on the current scheme to display them.
                }
            }
        }

        private void AddPrompt()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            Sprite[] all = Resources.LoadAll<Sprite>("input-tilemap");
            foreach (Sprite sprite in all)
            {
                PromptSprites.Add(sprite.name, sprite);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}