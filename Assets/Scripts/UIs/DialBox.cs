using UnityEngine;
using TMPro;

public class DialBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = null;
    [Space]
    [SerializeField] private float _timeByLetters = 0.5f;

    private int _char = 0;
    private char[] _textArray = null;
    private float _timeBeforeNextChar = 0;

    public float ReturnDuration()
        => (_textArray.Length * _timeByLetters);

    public void AddDialogue(string textToWrite)
    {
        _text.text = string.Empty;

        _char = 0;
        _textArray = textToWrite.ToCharArray();
        _timeBeforeNextChar = 0;

        GameManager.Instance.OnUpdateHUD += OnUpdateText;
    }

    private void OnUpdateText(float time)
    {
        if(time < _timeBeforeNextChar)
            return;
        
        if(_char >= _textArray.Length)
        {
            TextEnded();
            return;
        }

        _text.text += _textArray[_char];
        _char++;
        _timeBeforeNextChar = time + _timeByLetters;
    }

    private void TextEnded()
    {
        Clear();

        GameManager.Instance.OnUpdateHUD -= OnUpdateText;
    }

    public void Clear()
    {
        _text.text = string.Empty;

        _char = 0;
        _textArray = null;
        _timeBeforeNextChar = 0;
    }
}
