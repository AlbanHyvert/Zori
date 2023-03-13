using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject _dialogueBox = null;
    [SerializeField] private TextMeshProUGUI _dialogueText = null;
    [Space(10)]
    [SerializeField] private float _charDelay = 0.05f;

    private Queue<string> _sentences = new Queue<string>();
    private int _currentDialogueIndex = 0;
    private bool _isTyping = false;

    public bool IsTyping
    => _isTyping;

    public void StartDialogue(string[] dialogue)
    {
        _dialogueBox.SetActive(true);
        _dialogueText.text = string.Empty;

        _sentences.Clear();

        foreach (string sentence in dialogue)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void StartDialogue(string sentences)
    {
        _dialogueBox.SetActive(true);
        _dialogueText.text = string.Empty;

        _sentences.Clear();

        _sentences.Enqueue(sentences);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        _isTyping = true;
        _dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_charDelay);
        }

        yield return new WaitForSecondsRealtime(_charDelay + 1);

        DisplayNextSentence();

        _isTyping = false;
    }

    void EndDialogue()
    {
        StopAllCoroutines();

        _dialogueBox.SetActive(false);
        _dialogueText.text = string.Empty;

        Debug.Log("End of dialogue");
    }
}