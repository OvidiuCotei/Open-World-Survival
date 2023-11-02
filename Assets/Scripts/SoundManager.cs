using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvyKode
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; set; }
        private AudioSource audioSource;
        public AudioClip dropItemSound;
        public AudioClip craftindSound;
        public AudioClip toolSwingSound;
        public AudioClip chopSound;
        public AudioClip pickupItemSound;
        public AudioClip grassWalkSound;
        public AudioClip startingZoneBGMusic;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            GameObject bgSoundChild = new GameObject();
            bgSoundChild.name = "BG Audio Source";
            bgSoundChild.AddComponent<AudioSource>();
            bgSoundChild.transform.SetParent(transform);
            AudioSource backgroundAudioSource = bgSoundChild.GetComponent<AudioSource>();
            backgroundAudioSource.loop = true;
            backgroundAudioSource.clip = startingZoneBGMusic;
            backgroundAudioSource.volume = 0.02f;
            backgroundAudioSource.Play();
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip audioClip)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void StopSound()
        {
            audioSource.Stop();
        }

        public void PlaySoundSource(AudioClip audioClip, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(audioClip, position);
        }
    }
}