using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSeed : Seed
{
    public GameObject platformPrefab;

    bool exploded = false;

    override protected void OnExplode()
    {
        if (!exploded)
        {
            exploded = true;
            Destroy(rb2D);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Here to instantiate platform grow animation
            StartCoroutine(InstantiatePlatformCo());
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!exploded && other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Bridge"))
        {
            exploded = true;
            Destroy(rb2D);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Here to instantiate platform grow animation
            StartCoroutine(InstantiatePlatformCo());
        }
    }

    IEnumerator InstantiatePlatformCo()
    {
        GameObject poof = Instantiate(poofPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(poofDuration);
        GameObject platform = Instantiate(platformPrefab, transform.position, Quaternion.identity);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.platforms.Add(platform);
        }

        Destroy(poof);
        Destroy(gameObject);
    }
}
