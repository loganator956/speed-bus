using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusSpriteChanger : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public float DoorAnimationFrameTime = 0.2f;

    // up, right, left, down
    public BusSpriteAnimationData[] BusSpriteAnimationDatas;

    private int _currentSpriteIndex = 0;

    public SpriteRenderer BusSpriteRenderer, DoorSpriteRenderer;

    private void Awake()
    {
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private float _doorAnimationTimer = 0f;
    private int _currentDoorFrameIndex = 0;

    public bool DoorIsOpen = false;

    // Update is called once per frame
    void Update()
    {
        float up = Vector3.Dot(transform.up, Vector3.up);
        float right = Vector3.Dot(transform.up, Vector3.right);
        float left = Vector3.Dot(transform.up, Vector3.left);
        float down = Vector3.Dot(transform.up, Vector3.down);
        if (up < 1.1f && up > 0.65f)
        {
            _currentSpriteIndex = 0;
        }
        else if (right < 1.1f && right > 0.65f)
        {
            _currentSpriteIndex = 1;
        }
        else if (left < 1.1f && left > 0.65f)
        {
            _currentSpriteIndex = 2;
        }
        else if (down < 1.1f && down > 0.65f)
        {
            _currentSpriteIndex = 3;
        }
        _doorAnimationTimer += Time.deltaTime;
        if (_doorAnimationTimer > DoorAnimationFrameTime)
        {
            _doorAnimationTimer = 0f;
            // go to next frame, if needed
            if (DoorIsOpen)
            {
                _currentDoorFrameIndex++;
                if (_currentDoorFrameIndex >= GetCurrentSprite().DoorSprites.Length)
                {
                    _currentDoorFrameIndex = GetCurrentSprite().DoorSprites.Length - 1; // preventing out of range exception
                }
            }
            else
            {
                // close door (go down indexes)
                _currentDoorFrameIndex--;
                if (_currentDoorFrameIndex < 0)
                {
                    _currentDoorFrameIndex = 0;
                }
            }
        }
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (GetCurrentSprite().DoorSprites.Length == 0)
        {
            // clear sprite/hide it
            DoorSpriteRenderer.enabled = false;
        }
        else
        {
            // enable sprite and set it
            DoorSpriteRenderer.enabled = true;
            DoorSpriteRenderer.sprite = GetCurrentSprite().DoorSprites[_currentDoorFrameIndex];
        }
        BusSpriteRenderer.sprite = GetCurrentSprite().BusSprite;
    }

    private BusSpriteAnimationData GetCurrentSprite()
    {
        return BusSpriteAnimationDatas[_currentSpriteIndex];
    }
}

[System.Serializable]
public class BusSpriteAnimationData
{
    public Sprite[] DoorSprites;
    public Sprite BusSprite;
}