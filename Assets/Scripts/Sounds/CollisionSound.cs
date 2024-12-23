using System.Collections;
using UnityEngine;

public class CollisionSound : SoundBase
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float startDelay;
    private bool canPlay;
    private void Start()
    {
        StartCoroutine(StartTimer());
    }
    private void OnCollisionEnter(Collision other)
    {
        if(canPlay) PlaySoundWithRandomPitch(clips[Random.Range(0, clips.Length)]);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(startDelay);
        canPlay = true;
    }
}