using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDDOL : MonoBehaviour
{
    static AudioDDOL instance;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}
