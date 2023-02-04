using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSeed : Seed
{
    public GameObject bridgePrefab;
    public float bridgeRange = 10f;
    public float bridgeAnimationTime = 1.1f;
    [HideInInspector] public bool isStuck = false;

    protected override void OnExplode()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isStuck && collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Bridge"))
        {
            Destroy(rb2D);
            GetComponent<Collider2D>().enabled = false;
            isStuck = true;

            // Finding closest bridge seed
            GameObject[] bridgeSeeds = GameObject.FindGameObjectsWithTag("BridgeSeed");
            GameObject secondNode = null;

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
                if (bridgeSeed.isStuck)
                {
                    secondNode = seed;
                }
            }

            // Here to instantiate bridge grow animation
            if (secondNode != null)
            {
                StartCoroutine(InstantiateBridgeCo(secondNode));
            }
        }
    }

    IEnumerator InstantiateBridgeCo(GameObject secondNode)
    {
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

        Destroy(gameObject);
        Destroy(secondNode);
    }

}
