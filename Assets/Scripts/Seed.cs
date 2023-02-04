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
    public float bridgeAnimationTime = 1.1f;

    public GameObject bridgePiece;

    // Poofs
    public GameObject poofPlat;
    public GameObject poofVine;
    public GameObject poofBridge;

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
            if (secondNode != null && Vector3.Distance(transform.position, secondNode.transform.position) < bridgeRange)
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
        Instantiate(poofPlat, transform.position, Quaternion.identity);

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
        float distance = 0;
        float y, x, maxDistance;
        y = secondNode.transform.position.y - transform.position.y;
        x = secondNode.transform.position.x - transform.position.x;
        Vector3 dir = (secondNode.transform.position - transform.position).normalized;

        GameObject rootObject = Instantiate(bridgePiece, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(y, x))));
        Vector2 tempVector = rootObject.GetComponentInChildren<SpriteRenderer>().size;
        maxDistance = (secondNode.transform.position - transform.position).magnitude + 0.2f;
        tempVector.x = maxDistance;

        SpriteRenderer renderer = rootObject.GetComponentInChildren<SpriteRenderer>();

        while (distance < 1)
        {
            tempVector.x = maxDistance * distance;
            renderer.size = tempVector;

            renderer.transform.position = transform.position + dir * distance / 2;

            distance += Time.deltaTime;
            yield return null;
        }

        distance = 1;
        tempVector.x = maxDistance * distance;
        renderer.size = tempVector;
        renderer.transform.position = transform.position + dir * distance / 2;

        Destroy(gameObject);
        Destroy(secondNode);
    }
}
