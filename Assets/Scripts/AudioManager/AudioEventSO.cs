using UnityEngine;

namespace AudioManager
{
    [CreateAssetMenu(fileName = "new Audio", menuName = "Audio/Audio Event", order = 0)]
    public class AudioEventSO : ScriptableObject
    {
        [Header("Audio Settings")] 
        public string audioName;
        public AudioClip audioClip;
        [Range(0f, 1f)] public float volumen;

        public void Play(AudioSource source)
        {
            source.volume = volumen;
            source.pitch = UnityEngine.Random.Range(0.8f, 1.5f);

        // 3. Reproducir
            source.PlayOneShot(audioClip);
        }

    }
}