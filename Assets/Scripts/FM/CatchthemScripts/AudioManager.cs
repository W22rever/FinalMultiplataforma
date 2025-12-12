using UnityEngine;
using UnityEngine.SceneManagement;

namespace FM.CatchthemScripts
{
    


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Musics")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("SFX")]
    public AudioClip giftSFX;
    public AudioClip hazardSFX;
    public AudioClip timeUpSFX;
    public AudioClip deathSFX;

    private void Awake()
    {
        // SINGLETON ROBUSTO
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe otro AudioManager, destruimos este para que no suene doble
            Destroy(gameObject);
            return;
        }
    }

    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu") 
        {
            PlayMusic(menuMusic);
        }
        else if (scene.name == "AtrapalosGameplay") 
        {
            PlayMusic(gameplayMusic);
        }
        else if (scene.name == "Main")
        {
            StopMusic(menuMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (!clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (!clip) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void StopMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Stop();
    }
}}