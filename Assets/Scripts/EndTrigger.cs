using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndTrigger : MonoBehaviour
{
    bool finishing = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!finishing && other.gameObject.CompareTag("Player")) 
        {
            finishing = true;
            if (GameManager.Instance != null) 
            {
                UnityEvent afterFade = new UnityEvent();
                afterFade.AddListener(GameManager.Instance.LoadNextLevel);
                GameManager.Instance.fade.FadeOut(afterFade);
            }
        }
    }
}
