using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoiint : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("PlayAnimation", true);
        }
    }
}
