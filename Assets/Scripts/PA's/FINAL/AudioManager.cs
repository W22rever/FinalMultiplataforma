using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    
    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioSource gameplayMusicSource;
    [SerializeField] private AudioSource shootGunSFXSource;
    [SerializeField] private AudioSource doveSFXSource;
    
    private AudioMixerGroup _menuMusicGroup;
    private AudioMixerGroup _gameplayMusicGroup;
    private AudioMixerGroup _sFXMixerGroup;

    public AudioSource SFXGun => shootGunSFXSource;
    public AudioSource SFXDove => doveSFXSource;
    
    private void Awake()
    {
        _menuMusicGroup = audioMixer.FindMatchingGroups("Master/MenuMusic")[0];
        _gameplayMusicGroup = audioMixer.FindMatchingGroups("Master/GameplayMusic")[0];
        _sFXMixerGroup = audioMixer.FindMatchingGroups("Master/SFX")[0];
        
        menuMusicSource.outputAudioMixerGroup = _menuMusicGroup;
        gameplayMusicSource.outputAudioMixerGroup = _gameplayMusicGroup;
        
        shootGunSFXSource.outputAudioMixerGroup = _sFXMixerGroup;
        doveSFXSource.outputAudioMixerGroup = _sFXMixerGroup;
        
        _sFXMixerGroup.audioMixer.SetFloat("SFXVolume", 0.5f);
        
        menuMusicSource.Play();
    }

    /// <summary>
    ///  BOTÓN PLAY
    /// </summary>
    
    public void ChangeToGameplayMusic()
    {
        menuMusicSource.Stop();
        gameplayMusicSource.Play();
        gameplayMusicSource.mute = false;
    }

    /// <summary>
    ///  BOTÓN RETURN TO MENU 
    /// </summary>
    
    public void ChangeToMenuMusic()
    {
        gameplayMusicSource.Stop();
        menuMusicSource.Play();
    }

    /// <summary>
    /// BOTÓN PAUSA
    /// </summary>
    
    public void MuteMusicGameplay()
    {
        gameplayMusicSource.mute = !gameplayMusicSource.mute;
    }

    public void UnmuteMusicGameplay()
    {
        gameplayMusicSource.mute = !gameplayMusicSource.mute;
    }

    /// <summary>
    ///  BOTÓN REINICIAR NIVEL
    /// </summary>
    
    public void RestartMusicGameplay()
    {
        gameplayMusicSource.Stop();
        gameplayMusicSource.mute = false;
        gameplayMusicSource.Play();
    }
}
