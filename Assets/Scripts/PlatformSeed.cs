using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSeed : Seed
{
    public GameObject platformPrefab;

    override protected void OnExplode()
    {
        Destroy(rb2D);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Here to instantiate platform grow animation
        StartCoroutine(InstantiatePlatformCo());
    }

    IEnumerator InstantiatePlatformCo()
    {
        GameObject poof = Instantiate(poofPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(poofDuration);
        Instantiate(platformPrefab, transform.position, Quaternion.identity);

        Destroy(poof);
        Destroy(gameObject);
    }
}
