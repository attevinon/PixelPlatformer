using UnityEngine;
using UnityEngine.UI;
using PixelCrew.Model.Data.Properties;

namespace PixelCrew.UI.Widgets
{
    public class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _valueText;

        private FloatPersistentProperty _model;

        public void Initialize(FloatPersistentProperty model)
        {
            _model = model;
            _model.OnChanged += OnModelValueChanged;
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
            UpdateView(model.Value);
            
        }

        private void OnModelValueChanged(float newValue, float oldValue)
        {
            UpdateView(newValue);
        }

        private void UpdateView(float newValue)
        {
            _valueText.text = Mathf.RoundToInt(newValue * 100).ToString();
            _slider.normalizedValue = newValue;
        }

        private void OnSliderValueChanged(float inputValue)
        {
            _model.Value = inputValue;
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnModelValueChanged;
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}