using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType
{
    PlatformSeed = 0, VineSeed = 1, BridgeSeed = 2
}

public class Seed : MonoBehaviour
{
    public float timer = 3f;
    public SeedType seedType;
    public float bridgeRange = 3f;
    public bool stuckSeed = false;

    // Prefabs for the gameobjects that the seed instantiates
    public GameObject platform;
    public GameObject vine;

    public float animationTime = 0.5f;

    Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (seedType == SeedType.PlatformSeed)
            {
                GrowPlatform();
            }
        }
    }

    void GrowPlatform()
    {
        Destroy(rb2D);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        // Here to instantiate platform grow animation
        StartCoroutine(InstantiatePlatformCo());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Platform") && seedType == SeedType.BridgeSeed)
        {
            Destroy(rb2D);
            GetComponent<Collider2D>().enabled = false;
            stuckSeed = true;

            // Finding closest bridge seed
            GameObject[] bridgeSeeds = GameObject.FindGameObjectsWithTag("BridgeSeed");
            GameObject secondNode = null;
            foreach (GameObject seed in bridgeSeeds)
            {
                // Checking if the bridge seed is this bridge seed
                if (!GameObject.ReferenceEquals(gameObject, seed) &&
                    Vector3.Distance(transform.position, seed.transform.position) < bridgeRange &&
                    seed.GetComponent<Seed>().stuckSeed == true)
                {
                    // Setting the seed as the second node seed if it is closer than the previous
                    if (secondNode == null)
                    {
                        secondNode = seed;
                    }
                    else if (Vector3.Distance(transform.position, seed.transform.position) < Vector3.Distance(transform.position, secondNode.transform.position))
                    {
                        secondNode = seed;
                    }

                }
            }

            // Here to instantiate bridge grow animation
            if (secondNode != null)
            {
                StartCoroutine(InstantiateBridgeCo(secondNode));
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Platform") && seedType == SeedType.VineSeed)
        {
            if (timer <= 0)
            {
                // Here to instantiate vine grow animation
                StartCoroutine(InstantiateVineCo());
            }
        }
    }
    
    IEnumerator InstantiatePlatformCo()
    {
        yield return new WaitForSeconds(animationTime);
        Instantiate(platform, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator InstantiateVineCo()
    {
        yield return new WaitForSeconds(animationTime);
        Instantiate(vine, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator InstantiateBridgeCo(GameObject secondNode)
    {
        yield return new WaitForSeconds(animationTime);
        Destroy(gameObject);
        Destroy(secondNode);
    }
}
