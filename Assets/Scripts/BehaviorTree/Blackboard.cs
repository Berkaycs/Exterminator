using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard 
{
    private Dictionary<string, object> _blackboardData = new Dictionary<string, object>();

    public delegate void OnBlackboardValueChanged(string key, object value);

    public event OnBlackboardValueChanged onBlackboardValueChanged;

    public void SetOrAddData(string key, object value)
    {
        if (_blackboardData.ContainsKey(key)) 
        {
            _blackboardData[key] = value;
        }
        else
        {
            _blackboardData.Add(key, value);
        }
        onBlackboardValueChanged?.Invoke(key, value);
    }

    public bool GetBlackboardData<T>(string key, out T value)
    {
        value = default(T);

        if (_blackboardData.ContainsKey(key))
        {
            value = (T)_blackboardData[key];
            return true;
        }
        return false;
    }

    public void RemoveBlackboardData(string key)
    {
        _blackboardData.Remove(key);
        onBlackboardValueChanged.Invoke(key, null);
    }

    public bool HasKey(string key)
    {
        return _blackboardData.ContainsKey(key);
    }
}
