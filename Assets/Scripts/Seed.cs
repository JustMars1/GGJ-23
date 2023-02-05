using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum SeedType
{
    PlatformSeed = 0, VineSeed = 1, BridgeSeed = 2
}

public abstract class Seed : MonoBehaviour
{
    public TextMeshPro timerText;
    public int timer = 3;
    public float poofDuration = 0.5f;
    public GameObject poofPrefab;

    [HideInInspector] public Rigidbody2D rb2D;
    protected bool exploding;

    [HideInInspector] public PlayerControl sender;

    Vector3 timerOffset;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        timerOffset = timerText.transform.position - transform.position;
    }

    IEnumerator Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.seeds.Add(gameObject);
        }

        while (timer > 0)
        {
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer--;
        }

        timerText.text = "";


        if (rb2D != null)
        {
            rb2D.simulated = true;
        }
        GetComponent<Collider2D>().enabled = true;
        transform.SetParent(null);

        if (sender.currentThrowable == this)
        {
            sender.currentThrowable = null;
        }

        exploding = true;
        OnExplode();
    }

    void LateUpdate()
    {
        timerText.transform.eulerAngles = Vector3.zero;
        timerText.transform.position = transform.position + timerOffset;
    }

    protected abstract void OnExplode();
}
