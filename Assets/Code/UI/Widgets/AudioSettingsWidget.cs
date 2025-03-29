using UnityEngine;
using UnityEngine.UI;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.UI.Widgets
{
    public class AudioSettingsWidget : MonoBehaviour
    {
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _valueText;

        private FloatPersistentProperty _model;

        public void Initialize(FloatPersistentProperty model)
        {
            _model = model;
            _trash.Retain(_model.Subscribe(OnModelValueChanged));
            _trash.Retain(_slider.onValueChanged.Subscribe(OnSliderValueChanged));
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
            _trash.Dispose();
        }
    }
}