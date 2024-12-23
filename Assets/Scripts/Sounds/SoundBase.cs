using UnityEngine;

public class SoundBase : MonoBehaviour
{
    [SerializeField] protected float minPitch = .75f;
    [SerializeField] protected float maxPitch = 1.25f;
    public AudioSource AS;
    public void PlaySoundWithRandomPitch(AudioClip clip)
    {
        AS.pitch = Random.Range(minPitch, maxPitch);
        AS.PlayOneShot(clip);
    }
}