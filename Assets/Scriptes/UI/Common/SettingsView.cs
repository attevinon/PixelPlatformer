using PixelCrew.Model.Data;
using UnityEngine;
using PixelCrew.UI.Widgets;

namespace PixelCrew.UI
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sound;

        private void Start()
        {
            _music.Initialize(GameSettingsData.I.Music);
            _sound.Initialize(GameSettingsData.I.Sound);
        }
    }
}