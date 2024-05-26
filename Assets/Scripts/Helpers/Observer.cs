using System;
using UnityEngine;

namespace DefaultNamespace.Helpers
{
    [Serializable]
    public class Observer<T>
    {
        [SerializeField]
        private T _value;

        private Action<T> _actions;

        public T Value
        {
            get => _value;
            set => Set(value);
        }
        
        public Observer(T value, Action<T> actions)
        {
            _value = value;
            _actions = actions;
        } 

        private void Set(T value)
        {
            if (Equals(value, _value)) return;
            _value = value;
            _actions?.Invoke(_value);
        }

        public void AddListener(Action<T> act)
        {
            _actions -= act;
            _actions += act;
        }
        
        public void RemoveListener(Action<T> act)
        {
            _actions -= act;
        }
    }
}