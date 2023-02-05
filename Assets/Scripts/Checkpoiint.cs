using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoiint : MonoBehaviour
{
    public Animator animator;
    public GameObject[] checkPoints;

    private void Awake()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("DeGrowth", false);
            animator.SetBool("PlayAnimation", true);

            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (checkPoints[i] == gameObject)
                {
                    continue;
                }

                checkPoints[i].GetComponent<Animator>().SetBool("PlayAnimation", false);
                checkPoints[i].GetComponent<Animator>().SetBool("DeGrowth", true);
            }
        }
    }
}
