using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public TextMeshProUGUI aimInfo;
    public TextMeshProUGUI[] grenadeKeyTexts;
    public TextMeshProUGUI[] grenadeCountTexts;

    int highlightedGrenade;

    Coroutine aimInfoCo;

    bool infoVisible = false;

    void Awake()
    {
        Color c = aimInfo.color;
        c.a = 0;
        aimInfo.color = c;

        HighlightSelectedGrenade(0);
    }

    public void HighlightSelectedGrenade(int i)
    {
        grenadeKeyTexts[highlightedGrenade].color = Color.white;
        highlightedGrenade = i;
        grenadeKeyTexts[highlightedGrenade].color = Color.red;
    }

    public void UpdateGrenadeCount(int i, int count)
    {
        grenadeCountTexts[i].text = count.ToString();
        grenadeCountTexts[i].color = count == 0 ? Color.gray : Color.white;
    }

    public void DisplayAimInfo(bool visible)
    {
        if (visible == infoVisible) 
        {
            return;
        }

        if (aimInfoCo != null)
        {
            StopCoroutine(aimInfoCo);
            aimInfoCo = null;
        }

        infoVisible = visible;

        if (visible)
        {
            aimInfoCo = StartCoroutine(FadeAimInfoInCo());
        }
        else
        {
            aimInfoCo = StartCoroutine(FadeAimInfoOutCo());
        }
    }

    IEnumerator FadeAimInfoInCo()
    {
        yield return new WaitForSeconds(0.2f);
        Color c = aimInfo.color;

        while (c.a < 1)
        {
            aimInfo.color = c;
            c.a += Time.deltaTime;
            yield return null;
        }

        c.a = 1;
        aimInfo.color = c;
        aimInfoCo = null;
    }

    IEnumerator FadeAimInfoOutCo()
    {
        yield return new WaitForSeconds(0.2f);
        Color c = aimInfo.color;

        while (c.a > 0)
        {
            aimInfo.color = c;
            c.a -= Time.deltaTime;
            yield return null;
        }

        c.a = 0;
        aimInfo.color = c;
        aimInfoCo = null;
    }
}
