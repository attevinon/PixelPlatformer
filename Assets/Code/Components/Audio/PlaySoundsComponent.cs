using System;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    public class PlaySoundsComponent : MonoBehaviour
    { 
        [SerializeField] private AudioData[] _sounds;
        private AudioSource _audioSource;

        public void PlaySound(string id)
        {
            foreach (var sound in _sounds)
            {
                if (sound.Id != id) continue;
                
                if (_audioSource == null)
                    _audioSource = GameObject.FindWithTag("SoundsAudioSource").GetComponent<AudioSource>();

                _audioSource.PlayOneShot(sound.Clip);
                break;
            }
        }
    }

    [Serializable]
    public class AudioData
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip _clip;

        public string Id => _id;
        public AudioClip Clip => _clip;
    }
}

