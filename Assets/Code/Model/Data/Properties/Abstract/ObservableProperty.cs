using System;
using PixelCrew.Utils.Disposables;
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
        
        public IDisposable Subscribe(OnPropertyChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public IDisposable SubscribeAndInvoke(OnPropertyChanged call)
        {
            OnChanged += call;
            call?.Invoke(_value);
            return new ActionDisposable(() => OnChanged -= call);
        }
    }
}