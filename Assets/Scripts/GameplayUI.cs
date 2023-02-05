using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public TextMeshProUGUI aimInfo;
    public TextMeshProUGUI[] grenadeKeyTexts;

    int highlightedGrenade;
    
    public void HighlightSelectedGrenade(int i) 
    {
        grenadeKeyTexts[highlightedGrenade].color = Color.white;
        highlightedGrenade = i;
        grenadeKeyTexts[highlightedGrenade].color = Color.red;
    }

    public void DisplayAimInfo(bool visible) 
    {
        aimInfo.gameObject.SetActive(visible);
    }

    void Awake() 
    {
        HighlightSelectedGrenade(0);
    }
}
