using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType
{
    PlatformSeed, VineSeed, BridgeSeed
}

public class Seed : MonoBehaviour
{
    public float timer = 3f;
    public SeedType seedType;
    public float bridgeRange = 3f;
    public bool stuckSeed  = false;
    
    // Prefabs for the gameobjects that the seed instantiates
    public GameObject platform;
    public GameObject vine;

    public float animationTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if(seedType == SeedType.PlatformSeed)
            {
                GrowPlatform();
            }
        }
    }

    void GrowPlatform()
    {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        // Here to instantiate platform grow animation
        Invoke("InstantiatePlatform", animationTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null && collision.gameObject.tag == "Platform" && seedType == SeedType.BridgeSeed)
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            stuckSeed = true;
                
            // Finding closest bridge seed
            GameObject[] bridgeSeeds = GameObject.FindGameObjectsWithTag("BridgeSeed");
            GameObject secondNode = null;
            foreach(GameObject seed in bridgeSeeds)
            {
                // Checking if the bridge seed is this bridge seed
                if (!GameObject.ReferenceEquals(gameObject, seed) &&
                    Vector3.Distance(gameObject.transform.position, seed.transform.position) < bridgeRange &&
                    seed.GetComponent<Seed>().stuckSeed == true)
                {
                    // Setting the seed as the second node seed if it is closer than the previous
                    if(secondNode == null)
                    {
                        secondNode = seed;
                    }
                    else if (Vector3.Distance(gameObject.transform.position, seed.transform.position) <
                        Vector3.Distance(gameObject.transform.position, secondNode.transform.position))
                    {
                        secondNode = seed;
                    }

                }
            }

            if(secondNode != null)
            {
                Destroy(gameObject);
                Destroy(secondNode);
            }
                
            // Here to instantiate bridge grow animation
            Invoke("InstantiateBridge", animationTime);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Platform" && seedType == SeedType.VineSeed)
        {
            if (timer <= 0)
            {
                // Here to instantiate vine grow animation
                Invoke("InstantiateVine", animationTime);
            }
        }
    }

    void InstantiatePlatform()
    {
        Instantiate(platform, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void InstantiateVine()
    {
        Instantiate(vine, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void InstatiateBridge ()
    {

    }
}
