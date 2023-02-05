using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class Fade : MonoBehaviour
{
    Image fade;
    void Awake()
    {
        fade = GetComponent<Image>();
        FadeIn();
    }

    [HideInInspector] public bool fadeInCompleted = false;

    public void FadeOut(UnityEvent afterFade)
    {
        StartCoroutine(FadeOutCo(afterFade));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCo());
    }

    IEnumerator FadeOutCo(UnityEvent afterFade)
    {
        fade.raycastTarget = true;
        Color c = fade.color;
        c.a = 0;

        while (c.a < 1)
        {
            fade.color = c;
            c.a += Time.unscaledDeltaTime;
            yield return null;
        }

        c.a = 1;
        fade.color = c;

        if (afterFade != null)
        {
            afterFade.Invoke();
        }
    }

    IEnumerator FadeInCo()
    {
        fade.raycastTarget = true;
        Color c = fade.color;
        c.a = 1;

        while (c.a > 0)
        {
            fade.color = c;
            c.a -= Time.unscaledDeltaTime;
            yield return null;
        }

        c.a = 0;
        fade.color = c;
        fade.raycastTarget = false;
        fadeInCompleted = true;
    }
}
