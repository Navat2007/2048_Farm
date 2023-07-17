using Bayat.SaveSystem;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private const string MusicVolumeKey = "Music";
    private const string EffectsVolumeKey = "Effects";

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioMixer _mixer;
    
    public enum AudioType
    {
        Music,
        Effects,
    }
    
    private void Awake()
    {
        ServiceLocator.AudioManager = this;
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }
    
    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
        AudioListener.volume = silence ? 0 : 1;
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position, 1);
    }

    public void SetVolume(float volume, AudioType type)
    {
        switch (type)
        {
            case AudioType.Music:
                _mixer.SetFloat(MusicVolumeKey, ValueToVolume(volume));
                //await SaveSystemAPI.SaveAsync(MusicVolumeKey, volume);
                ServiceLocator.SaveLoadManager.SaveFloat(MusicVolumeKey, volume);
                break;
            
            case AudioType.Effects:
                _mixer.SetFloat(EffectsVolumeKey, ValueToVolume(volume));
                //await SaveSystemAPI.SaveAsync(EffectsVolumeKey, volume);
                ServiceLocator.SaveLoadManager.SaveFloat(EffectsVolumeKey, volume);
                break;
        }
    }
    
    private float ValueToVolume(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
    }
}