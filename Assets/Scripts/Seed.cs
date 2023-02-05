using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType
{
    PlatformSeed = 0, VineSeed = 1, BridgeSeed = 2
}

public abstract class Seed : MonoBehaviour
{
    public int timer = 3;
    public float poofDuration = 0.5f;
    public GameObject poofPrefab;

    [HideInInspector] public Rigidbody2D rb2D;
    protected bool exploding;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    IEnumerator Start()
    {
        while(timer > 0) 
        {
            yield return new WaitForSeconds(1);
            timer--;
        }
        
        exploding = true;
        OnExplode();
    }

    protected abstract void OnExplode();
}
