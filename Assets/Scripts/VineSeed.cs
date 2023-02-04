using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineSeed : Seed
{
    public GameObject vinePrefab;

    bool exploded = false;

    HashSet<GameObject> collidingObjects = new HashSet<GameObject>();

    protected override void OnExplode()
    {
        if (!exploded && collidingObjects.Count > 0)
        {
            exploded = true;
            StartCoroutine(InstantiateVineCo());
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Bridge"))
        {
            collidingObjects.Add(other.gameObject);
        }

        if (exploding && !exploded && collidingObjects.Count > 0)
        {
            exploded = true;
            StartCoroutine(InstantiateVineCo());
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Bridge"))
        {
            collidingObjects.Remove(other.gameObject);
        }
    }

    IEnumerator InstantiateVineCo()
    {
        GameObject poof = Instantiate(poofPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(poofDuration);
        Instantiate(vinePrefab, transform.position, Quaternion.identity);
        Destroy(poof);
        Destroy(gameObject);
    }
}
