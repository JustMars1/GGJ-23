using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType
{
    PlatformSeed = 0, VineSeed = 1, BridgeSeed = 2
}

public abstract class Seed : MonoBehaviour
{
    public float timer = 3f;
    public float poofDuration = 0.5f;
    public GameObject poofPrefab;

    protected Rigidbody2D rb2D;
    protected bool exploding;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(timer);
        exploding = true;
        OnExplode();
    }

    protected abstract void OnExplode();
}
