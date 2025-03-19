using System;
using UnityEngine;
using PixelCrew.Model.Data.Properties;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettingsData", fileName = "GameSettingsData")]
    public class GameSettingsData : ScriptableObject
    {
        [SerializeField] private FloatPersistentProperty _music;
        [SerializeField] private FloatPersistentProperty _sound;

        public FloatPersistentProperty Music => _music;
        public FloatPersistentProperty Sound => _sound;

        private static GameSettingsData _instance;
        public static GameSettingsData I => _instance == null ? LoadGameSettingsData() : _instance;

        private static GameSettingsData LoadGameSettingsData()
        {
            return _instance = Resources.Load<GameSettingsData>("GameSettingsData");
        }

        private void OnEnable()
        {
            _music = new FloatPersistentProperty(1, AudioSettingsKeys.Music.ToString());
            _sound = new FloatPersistentProperty(1, AudioSettingsKeys.Sound.ToString());
        }

        private void OnValidate()
        {
            Predicate<float> isInRange = x => x >= 0 && x <= 1;
            _music.Validate(isInRange);
            _sound.Validate(isInRange);
        }
    }

    public enum AudioSettingsKeys
    {
        Music,
        Sound
    }
}