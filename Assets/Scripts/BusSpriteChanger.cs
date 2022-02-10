using UnityEngine;

public class BusSpriteChanger : MonoBehaviour
{
    [Header("Variables")]
    public BusSpriteAnimationData[] BusSpriteAnimationDatas; // up, right, left, down
    public float DoorAnimationFrameTime = 0.2f;
    public bool DoorIsOpen = false;

    [Header("References")]
    public SpriteRenderer BusSpriteRenderer, DoorSpriteRenderer;

    #region Private Variables
    private int _currentSpriteIndex = 0;
    private float _doorAnimationTimer = 0f;
    private int _currentDoorFrameIndex = 0;
    private Rigidbody2D _rigidbody2D;
    #endregion

    private void Awake()
    {
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        // using dot products to establish which direction the player is facing
        // and then applying the corresponding sprite index
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