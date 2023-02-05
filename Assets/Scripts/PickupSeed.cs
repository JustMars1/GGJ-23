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
            gameObject.SetActive(false);
            PlayerControl player = other.GetComponent<PlayerControl>();
            player.grenadeCounts[(int)type]++;
            GameManager.Instance.pickups.Add(gameObject);
            player.UpdateUICounters();
        }
    }
}
