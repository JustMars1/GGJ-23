using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DialogueSceneEnd : MonoBehaviour
{
    public GameObject textBox;
    public Animator animator;
    public float speed;
    public TextBoxManager tbManager;

    bool villageReached = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!villageReached)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.position += new Vector3(1 * speed * Time.deltaTime, 0, 0);
            animator.SetBool("Run", true);
        }


        if (!textBox.activeInHierarchy && villageReached)
        {
            UnityEvent afterFade = new UnityEvent();
            afterFade.AddListener(delegate { SceneManager.LoadScene("Main"); });
            GameManager.Instance.fade.FadeOut(afterFade);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tree"))
        {
            villageReached = true;
            animator.SetBool("Run", false);
            tbManager.EnableTextBox();
        }
    }
}
