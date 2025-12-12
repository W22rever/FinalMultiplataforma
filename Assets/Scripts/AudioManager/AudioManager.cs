using System.Collections.Generic;
using UnityEngine;

namespace AudioManager
{
    
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Biblioteca de Sonidos")]
        public AudioEventSO[] library;
        private Dictionary<string, AudioEventSO> _soundDictionary;
        private AudioSource _sfxSource;
        
        private AudioSource _musicSource;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); 
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _musicSource = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
            _sfxSource = gameObject.AddComponent<AudioSource>();
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _soundDictionary = new Dictionary<string, AudioEventSO>();

            foreach (AudioEventSO soundEvent in library)
            {
                if (!_soundDictionary.ContainsKey(soundEvent.audioName))
                {
                    _soundDictionary.Add(soundEvent.audioName, soundEvent);
                }
                else
                {
                    Debug.LogWarning($"El sonido '{soundEvent.audioName}' está duplicado en la biblioteca.");
                }
            }
        }

        
        public void PlaySound(string name)
        {
            if (_soundDictionary.TryGetValue(name, out AudioEventSO sound))
            {
                sound.Play(_sfxSource);
            }
            else
            {
                Debug.LogWarning($"No se encontró el sonido: {name}");
            }
        }

        public void StopMusic()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop(); 
            }
        }
    }
}