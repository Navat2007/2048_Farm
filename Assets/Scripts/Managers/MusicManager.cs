using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        ServiceLocator.MusicManager = this;
        
        EventBus.AdsEvents.OnAdsShown += MuteMusic;
        EventBus.AdsEvents.OnAdsFailed += UnMuteMusic;
        EventBus.AdsEvents.OnAdsClose += UnMuteMusic;
    }
    
    private void OnDestroy()
    {
        EventBus.AdsEvents.OnAdsShown -= MuteMusic;
        EventBus.AdsEvents.OnAdsFailed -= UnMuteMusic;
        EventBus.AdsEvents.OnAdsClose -= UnMuteMusic;
    }

    private void MuteMusic()
    {
        _audioSource.mute = true;
    }
    
    private void UnMuteMusic()
    {
        _audioSource.mute = false;
    }
}