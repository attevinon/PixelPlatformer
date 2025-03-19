using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class ObservableProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;
        public delegate void OnPropertyChanged(TPropertyType newValue);
        public event OnPropertyChanged OnChanged;
        public TPropertyType Value
        {
            get => _value;
            set
            {
                bool isEquals = _value.Equals(value);
                if(isEquals) return;
                _value = value;
                
                OnChanged?.Invoke(value);
            }
        }
    }
}