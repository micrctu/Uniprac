using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC_Move
{
 //   public bool canMove;
    public string[] directions;

    [Range(1, 5)][Tooltip("1 = 천천히, 2 = 조금 천천히, 3 = 보통, 4 = 빠르게, 5 = 연속적으로")]
    public int freq;
}


public class NPCManager : MovingObject
{
    public NPC_Move npcMove;

    // Start is called before the first frame update
    void Start()
    {
        theBC = GetComponent<BoxCollider2D>();
        theAnim = GetComponent<Animator>();
        SetMove();
    }

    public void SetMove()
    {
        canMove = false;
        StartCoroutine(NPCMoveCoroutine());  
    }

    private IEnumerator NPCMoveCoroutine()
    {
        if(npcMove.directions.Length != 0)
        {
            for (int i = 0; i < npcMove.directions.Length; i++)
            {
                base.Move(npcMove.directions[i], npcMove.freq);
                yield return new WaitUntil(() => canMove);

                canMove = false;
                if (i == npcMove.directions.Length - 1)
                    i = -1;
            }
        }
    }

    public void SetNotMove()
    {
        StopAllCoroutines();
    }
}
