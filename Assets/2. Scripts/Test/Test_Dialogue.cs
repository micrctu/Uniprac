using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Dialogue : MonoBehaviour
{

    public Dialogue dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogueManager.instance.ShowDialogue(dialogue);
          //  DialogueManager.instance.ShowText(dialogue);
        }
    }

}
