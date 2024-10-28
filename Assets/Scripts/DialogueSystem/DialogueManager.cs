using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public abstract class DialogueManager
{
    [Inject(Id = "DialogueScreen")]
    protected readonly GameObject _dialogueScreen;
    [Inject(Id = "DialogueText")]
    protected readonly TMP_Text _dialogueText;
    [Inject(Id = "DialogueAudioSource")]
    protected readonly AudioSource _audioSource;
    [Inject(Id = "UIInstaller")]
    protected readonly MonoInstaller _installer;

    protected Queue<string> _textPhrases;
    protected Queue<AudioClip> _audioPhrases;
    protected Queue<float> _phrasesDelays;
    protected Dialogue _currentDialogue = null;
    protected AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    public void Trigger()
    {
        if (_currentDialogue != null && _currentDialogue.pharses != null && _currentDialogue.pharses.Length > 0)
        {
            string[] text = _currentDialogue.pharses;
            AudioClip[] audio = _currentDialogue.audio;
            float[] delays = _currentDialogue.phraseDelays;
            if (text.Length != audio.Length)
            {
                Debug.LogError("Text amount and audio amount are not equal");
                return;
            }
            if (text.Length != delays.Length)
            {
                Debug.LogError("Text amount and delays amount are not equal");
                return;
            }

            if (_textPhrases == null)
            {
                _animationsController.FadeInScreen(_dialogueScreen);
                _textPhrases = new Queue<string>();
                _audioPhrases = new Queue<AudioClip>();
                _phrasesDelays = new Queue<float>();
                for (int i = 0; i < text.Length; i++)
                {
                    _textPhrases.Enqueue(text[i]);
                    _audioPhrases.Enqueue(audio[i]);
                    _phrasesDelays.Enqueue(delays[i]);
                }
            }
            DisplayPhrase();
            if (_phrasesDelays.Count > 0) _installer.StartCoroutine(OnPhraseEnd(_phrasesDelays.Dequeue()));
        }
    }

    protected void DisplayPhrase()
    {
        if (_textPhrases.Count == 0) EndDialogue();
        else
        {
            _dialogueText.text = _textPhrases.Dequeue();
            _audioSource.clip = _audioPhrases.Dequeue();
            _audioSource.Play();
        }
    }

    protected void EndDialogue()
    {
        _animationsController.FadeOutScreen(_dialogueScreen);
        _textPhrases = null;
        _audioPhrases = null;
    }

    public void LeaveDialogue()
    {
        _currentDialogue = null;
        EndDialogue();
    }

    public void SetDialogue(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
    }

    protected abstract IEnumerator OnPhraseEnd(float delayTime);
}