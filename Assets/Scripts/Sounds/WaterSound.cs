using Unity.Mathematics;
using UnityEngine;

public class WaterSound : SoundBase
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private GameObject soundPrefab;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameObject sound = Instantiate(soundPrefab, other.transform.position, quaternion.identity, null);
            AS = sound.GetComponent<AudioSource>();
            SoundTimer soundTimer = sound.GetComponent<SoundTimer>();
            PlaySoundWithRandomPitch(clips[UnityEngine.Random.Range(0, clips.Length)]);
            soundTimer.StartWaiting(AS);
        }
    }
}