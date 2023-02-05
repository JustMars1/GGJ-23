using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSeed : Seed
{
    public GameObject bridgePrefab;
    public float bridgeRange = 10f;
    [HideInInspector] public bool isStuck = false;
    [HideInInspector] public bool growing = false;

    HashSet<GameObject> collidingObjects = new HashSet<GameObject>();

    protected override void OnExplode()
    {
        if (!isStuck && exploding && collidingObjects.Count > 0)
        {
            Stick();
        }

        if (!growing && isStuck)
        {
            TryGrow();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Bridge"))
        {
            collidingObjects.Add(other.gameObject);
        }

        if (!isStuck && exploding && collidingObjects.Count > 0)
        {
            Stick();
        }

        if (!growing && isStuck)
        {
            TryGrow();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Bridge"))
        {
            collidingObjects.Remove(other.gameObject);
        }
    }

    void Stick()
    {
        isStuck = true;
        Destroy(rb2D);
        GetComponent<Collider2D>().enabled = false;
    }

    void TryGrow()
    {
        // Finding closest bridge seed
        GameObject[] bridgeSeeds = GameObject.FindGameObjectsWithTag("BridgeSeed");
        BridgeSeed secondNode = null;

        float shortestDist = float.MaxValue;

        foreach (GameObject seed in bridgeSeeds)
        {
            // Checking if the bridge seed is this bridge seed
            if (gameObject == seed)
            {
                continue;
            }

            float dist = Vector3.Distance(transform.position, seed.transform.position);
            if (dist > bridgeRange || dist > shortestDist)
            {
                continue;
            }

            BridgeSeed bridgeSeed = seed.GetComponent<BridgeSeed>();
            if (bridgeSeed != null && bridgeSeed.isStuck && !bridgeSeed.growing)
            {
                secondNode = bridgeSeed;
            }
        }

        // Here to instantiate bridge grow animation
        if (secondNode != null)
        {
            growing = true;
            secondNode.growing = true;
            StartCoroutine(InstantiateBridgeCo(secondNode.gameObject));
        }
    }

    IEnumerator InstantiateBridgeCo(GameObject secondNode)
    {
        {
            GameObject poof = Instantiate(poofPrefab, transform.position, Quaternion.identity);
            Destroy(poof, poofDuration);
            Destroy(gameObject.GetComponent<SpriteRenderer>());
        }

        Vector3 dir = (secondNode.transform.position - transform.position).normalized;

        GameObject bridge = Instantiate(bridgePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x))));
        Vector2 size = bridge.GetComponentInChildren<SpriteRenderer>().size;
        float targetDistance = (secondNode.transform.position - transform.position).magnitude;
        SpriteRenderer renderer = bridge.transform.GetChild(0).GetComponent<SpriteRenderer>();

        Vector3 origin = transform.position;

        float distance = 0;
        while (distance < targetDistance)
        {
            size.x = distance;
            renderer.size = size;
            renderer.transform.position = origin + dir * distance / 2;
            distance += Time.deltaTime;
            yield return null;
        }

        size.x = targetDistance;
        renderer.size = size;
        renderer.transform.position = origin + dir * targetDistance / 2;

        {
            GameObject poof = Instantiate(poofPrefab, secondNode.transform.position, Quaternion.identity);
            Destroy(poof, poofDuration);
            Destroy(secondNode.GetComponent<SpriteRenderer>());
        }

        Destroy(gameObject);
        Destroy(secondNode);
    }

}
