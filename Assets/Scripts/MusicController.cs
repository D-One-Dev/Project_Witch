using DG.Tweening;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeMusic(AudioClip track)
    {
        musicSource.DOFade(0f, 1f).SetUpdate(UpdateType.Normal, true).OnKill(() =>
        {
            musicSource.clip = track;
            musicSource.Play();
            musicSource.DOFade(0f, 1f).SetUpdate(UpdateType.Normal, true);
        });
    }
}
