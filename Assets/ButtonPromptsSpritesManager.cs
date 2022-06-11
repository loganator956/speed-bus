using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPromptsSpritesManager : MonoBehaviour
{
    public Dictionary<string, Sprite> PromptSprites = new Dictionary<string, Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        Sprite[] all = Resources.LoadAll<Sprite>("input-tilemap");
        foreach(Sprite sprite in all)
        {
            PromptSprites.Add(sprite.name, sprite);
        }
    }

    public int CurrentDevice { get; private set; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
