using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuTypingEffect : MonoBehaviour
{
    [TextArea()]
    public string[] Messages;
    public float TypeSpeed = 0.2f;
    public float DisplaySpeed = 1.5f;
    public float RemoveSpeed = 0.05f;

    public bool IsEffecting = true;

    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    int currentMessageIndex = 0;
    int currentCharacterIndex = 0;

    private float _timer = 0f;

    enum TypingDisplayStatus
    {
        Typing, Display, Deleting
    }

    private TypingDisplayStatus _status;

    public UnityEvent OnCompletionEvent;

    // Update is called once per frame
    void Update()
    {
        if (IsEffecting)
        {
            _timer += Time.deltaTime;
            switch (_status)
            {
                case TypingDisplayStatus.Typing:
                    if (_timer > TypeSpeed)
                    {
                        // add a letter
                        currentCharacterIndex++;
                        if (currentCharacterIndex > Messages[currentMessageIndex].Length)
                        {
                            _status = TypingDisplayStatus.Display;
                        }
                        else
                        {
                            text.text = Messages[currentMessageIndex].Substring(0, currentCharacterIndex);
                        }
                        _timer = 0f;
                    }
                    break;
                case TypingDisplayStatus.Display:
                    if (_timer > DisplaySpeed)
                    {
                        // start removuing
                        _timer = 0f;
                        _status = TypingDisplayStatus.Deleting;
                    }
                    break;
                case TypingDisplayStatus.Deleting:
                    if (_timer > RemoveSpeed)
                    {
                        currentCharacterIndex--;
                        if (currentCharacterIndex < 0)
                        {
                            _status = TypingDisplayStatus.Typing;
                            currentMessageIndex++;
                            if (currentMessageIndex >= Messages.Length)
                            {
                                IsEffecting = false;
                                OnCompletionEvent.Invoke();
                                Application.Quit();
                            }
                        }
                        else
                        {
                            text.text = Messages[currentMessageIndex].Substring(0, currentCharacterIndex);
                        }
                        _timer = 0f;
                    }
                    break;
            }
        }
    }
}
