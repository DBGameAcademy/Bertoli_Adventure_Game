using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// singleton design pattern

// where si utilizza per specificare una certa caratteristica che il tipo T deve avere

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Instance of this singleton " + (T)this + " already exists, deleting!");
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = (T)this;
        }
    }

    public static T Instance
    {
        get
        {
            return instance;
        }
    }
}