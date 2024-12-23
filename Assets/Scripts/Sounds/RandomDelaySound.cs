using System.Collections;
using UnityEngine;

public class RandomDelaySound : SoundBase
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;

    private void Start()
    {
        StartCoroutine(SoundDelay());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator SoundDelay()
    {
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        PlaySoundWithRandomPitch(clip);
        yield return new WaitWhile(() => AS.isPlaying);
        StartCoroutine(SoundDelay());
    }
}