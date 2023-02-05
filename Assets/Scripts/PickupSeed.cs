using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSeed : MonoBehaviour
{
    public SeedType type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            other.GetComponent<PlayerControl>().grenadeCounts[(int)type]++;
            gameObject.SetActive(false);
        }
    }
}
