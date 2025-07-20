using System;
using System.Collections;
using System.Collections.Generic;

//[Serializable]
public class ObservableField<T>
{
    //[UnityEngine.SerializeField]
    private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            var pre = _value; _value = value;
            if (!pre.Equals(_value)) OnChanged?.Invoke(_value);
        }
    }

    public event Action<T> OnChanged;

    public ObservableField(T initial)
    {
        _value = initial;
        OnChanged = null;
    }
}