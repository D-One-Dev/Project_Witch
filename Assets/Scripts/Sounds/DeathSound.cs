using Unity.Mathematics;
using UnityEngine;

public class DeathSound : SoundBase
{
    [SerializeField] private GameObject deathSoundPrefab;
    [SerializeField] private AudioClip[] clips;
    public void PlayDeathSound()
    {
        GameObject sound = Instantiate(deathSoundPrefab, transform.position, quaternion.identity, null);
        AS = sound.GetComponent<AudioSource>();
        AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        PlaySoundWithRandomPitch(clip);
        sound.GetComponent<SoundTimer>().StartWaiting(AS);
    }
}