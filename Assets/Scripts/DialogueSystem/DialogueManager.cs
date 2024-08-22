using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueScreen;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private AudioSource audioSource;
    private Queue<string> textPhrases;
    private Queue<AudioClip> audioPhrases;
    private Controls _controls;

    public Dialogue currentDialogue = null;
    public static DialogueManager Instance;

    private void Awake()
    {
        Instance = this;
        _controls = new Controls();

        _controls.Gameplay.Interact.performed += ctx => Trigger();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    public void Trigger()
    {
        if (currentDialogue != null && currentDialogue.pharses != null && currentDialogue.pharses.Length > 0)
        {
            string[] text = currentDialogue.pharses;
            AudioClip[] audio = currentDialogue.audio;
            if (text.Length != audio.Length)
            {
                Debug.LogError("Text amount and audio amount are not equal");
                return;
            }

            if (textPhrases == null)
            {
                AnimationsController.instance.FadeInScreen(dialogueScreen);
                textPhrases = new Queue<string>();
                audioPhrases = new Queue<AudioClip>();
                for (int i = 0; i < text.Length; i++)
                {
                    textPhrases.Enqueue(text[i]);
                    audioPhrases.Enqueue(audio[i]);
                }
            }
            DisplayPhrase();
        }
    }

    private void DisplayPhrase()
    {
        if (textPhrases.Count == 0) EndDialogue();
        else
        {
            dialogueText.text = textPhrases.Dequeue();
            audioSource.clip = audioPhrases.Dequeue();
            audioSource.Play();
        }
    }

    private void EndDialogue()
    {
        AnimationsController.instance.FadeOutScreen(dialogueScreen);
        textPhrases = null;
        audioPhrases = null;
    }

    public void LeaveDialogue()
    {
        currentDialogue = null;
        EndDialogue();
    }
}
