using System;

namespace ChestSystem.Events
{
    public class EventController
    {
        public event Action baseEvent;
        public void InvokeEvent() => baseEvent?.Invoke();
        public void AddListener(Action listener) => baseEvent += listener;
        public void RemoveListener(Action listener) => baseEvent -= listener;
    }

    public class EventController<T>
    {
        public event Action<T> baseEvent;
        public void InvokeEvent(T type) => baseEvent?.Invoke(type);
        public void AddListener(Action<T> listener) => baseEvent += listener;
        public void RemoveListener(Action<T> listener) => baseEvent -= listener;
    }

    public class EventController<T1, T2, T3>
    {
        public event Action<T1, T2, T3> baseEvent;
        public void InvokeEvent(T1 type1, T2 type2, T3 type3) => baseEvent?.Invoke(type1, type2, type3);
        public void AddListener(Action<T1, T2, T3> listener) => baseEvent += listener;
        public void RemoveListener(Action<T1, T2, T3> listener) => baseEvent -= listener;
    }
}