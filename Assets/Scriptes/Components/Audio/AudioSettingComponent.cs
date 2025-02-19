using System;
using UnityEngine;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;

namespace PixelCrew.Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingComponent : MonoBehaviour
    { 
        [SerializeField] private AudioSettingsKeys _audioType;
        private AudioSource _source;
        private FloatPersistentProperty _model;

        private void Start()
        {
            _source = GetComponent<AudioSource>();

            _model = FindProperty(_audioType);
            _model.OnChanged += OnAudioSettingChanged;
            UpdateVolume(_model.Value);
        }

        private void OnAudioSettingChanged(float newValue, float oldValue)
        {
            UpdateVolume(newValue);
        }

        private void UpdateVolume(float newValue)
        {
            _source.volume = newValue;
        }

        private FloatPersistentProperty FindProperty(AudioSettingsKeys type)
        {
            switch (type)
            {
                case AudioSettingsKeys.Music:
                    return GameSettingsData.I.Music;
                case AudioSettingsKeys.Sound:
                    return GameSettingsData.I.Sound;
                default:
                    throw new ArgumentException("Undefined Audio Type");
            }
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnAudioSettingChanged;
        }
    }
}