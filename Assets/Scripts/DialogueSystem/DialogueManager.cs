using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DialogueManager
{
    [Inject(Id = "DialogueScreen")]
    private readonly GameObject _dialogueScreen;
    [Inject(Id = "DialogueText")]
    private readonly TMP_Text _dialogueText;
    [Inject(Id = "PlayerAudioSource")]
    private readonly AudioSource _audioSource;

    private Queue<string> _textPhrases;
    private Queue<AudioClip> _audioPhrases;
    private Dialogue _currentDialogue = null;
    private Controls _controls;
    private AnimationsController _animationsController;

    [Inject]
    public void Construct(Controls controls, AnimationsController animationsController)
    {
        _controls = controls;
        _animationsController = animationsController;

        PlayerHealth.OnPlayerDeath += DisableControls;
        _controls.Gameplay.Interact.performed += ctx => Trigger();
        _controls.Enable();
    }

    public void Trigger()
    {
        if (_currentDialogue != null && _currentDialogue.pharses != null && _currentDialogue.pharses.Length > 0)
        {
            string[] text = _currentDialogue.pharses;
            AudioClip[] audio = _currentDialogue.audio;
            if (text.Length != audio.Length)
            {
                Debug.LogError("Text amount and audio amount are not equal");
                return;
            }

            if (_textPhrases == null)
            {
                _animationsController.FadeInScreen(_dialogueScreen);
                _textPhrases = new Queue<string>();
                _audioPhrases = new Queue<AudioClip>();
                for (int i = 0; i < text.Length; i++)
                {
                    _textPhrases.Enqueue(text[i]);
                    _audioPhrases.Enqueue(audio[i]);
                }
            }
            DisplayPhrase();
        }
    }

    private void DisplayPhrase()
    {
        if (_textPhrases.Count == 0) EndDialogue();
        else
        {
            _dialogueText.text = _textPhrases.Dequeue();
            _audioSource.clip = _audioPhrases.Dequeue();
            _audioSource.Play();
        }
    }

    private void EndDialogue()
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

    private void DisableControls()
    {
        _controls.Disable();
    }

    public void SetDialogue(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
    }
}