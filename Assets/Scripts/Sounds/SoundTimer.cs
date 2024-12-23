using System.Collections;
using UnityEngine;

public class SoundTimer : MonoBehaviour
{
    private AudioSource AS;
    public void StartWaiting(AudioSource AS)
    {
        this.AS = AS;
        StartCoroutine(WaitForSoundEnd());
    }

    private IEnumerator WaitForSoundEnd()
    {
        yield return new WaitWhile(() => AS.isPlaying);
        Destroy(gameObject);
    }
}