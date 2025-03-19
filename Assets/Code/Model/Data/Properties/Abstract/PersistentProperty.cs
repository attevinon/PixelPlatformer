using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;
        private TPropertyType _stored;
        private readonly TPropertyType _defaultValue;
        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;
        public TPropertyType Value
        {
            get => _stored;
            set
            {
                bool isEquals = _stored.Equals(value);
                if(isEquals) return;
                TPropertyType oldValue = _value;
                Write(value);
                _stored = _value = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected void Initialize()
        {
            _stored = _value = Read(_defaultValue);
        }
        
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);

        public void Validate(Predicate<TPropertyType> predicate)
        {
            if(predicate.Invoke(_value) && !_stored.Equals(_value))
                Value = _value;
        }
    }
}
