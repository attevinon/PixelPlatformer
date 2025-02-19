using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.UI.Widgets
{
    public class PlaySoundOnClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _sound;
        private AudioSource _audioSource;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_audioSource == null)
                _audioSource = GameObject.FindWithTag("SoundsAudioSource").GetComponent<AudioSource>();

            _audioSource.PlayOneShot(_sound);
        }
    }
}