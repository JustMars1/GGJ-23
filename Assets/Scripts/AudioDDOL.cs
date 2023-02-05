using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
            instance = this;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
