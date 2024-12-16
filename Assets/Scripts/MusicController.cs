using DG.Tweening;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeMusic(AudioClip track)
    {
        musicSource.DOFade(0f, 1f).SetUpdate(UpdateType.Normal, true).OnKill(() =>
        {
            musicSource.clip = track;
            musicSource.Play();
            musicSource.DOFade(1f, 1f).SetUpdate(UpdateType.Normal, true);
        });
    }
    
    public void ChangeMusicWithoutFade(AudioClip track)
    {
        musicSource.Stop();
        musicSource.clip = track;
        musicSource.Play();
    }
    
    public void DisableMusic()
    {
        musicSource.DOFade(0f, 1f).SetUpdate(UpdateType.Normal, true);
    }
}
