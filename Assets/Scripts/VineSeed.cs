using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineSeed : Seed
{
    public GameObject vinePrefab;

    bool exploded = false;

    HashSet<GameObject> collidingObjects = new HashSet<GameObject>();

    protected override void OnExplode() { }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Bridge"))
        {
            collidingObjects.Add(other.gameObject);
        }

        if (!exploded && collidingObjects.Count > 0)
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
        Destroy(rb2D);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        AudioPlayer.Play(poofSounds[Random.Range(0, poofSounds.Length)], isMusic: false, variablePitch: true, variableVolume: true);
        GameObject poof = Instantiate(poofPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(poofDuration);

        GameObject vine = Instantiate(vinePrefab, transform.position, Quaternion.identity);

        if (GameManager.Instance != null) 
        {
            GameManager.Instance.vines.Add(vine);
        }

        Destroy(poof);
        Destroy(gameObject);
    }
}
