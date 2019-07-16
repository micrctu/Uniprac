using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public int walkCount;
    protected int currentWalkCount;

    public float moveSpeed;
    public float runSpeed;

    protected bool applyRun = false;
    protected float applyRunSpeed;

    public LayerMask layerMask;

    protected BoxCollider2D theBC;
    protected Animator theAnim;

    protected Vector3 vector;
    
    protected bool CheckCollision()
    {
        RaycastHit2D hit;

        Vector2 start = new Vector2(this.gameObject.transform.position.x + (vector.x * moveSpeed * walkCount), 
                                    this.gameObject.transform.position.y + (vector.y * moveSpeed * walkCount));
        //만약, 게임오브젝트의 위치에서 linecast를 한다면 캐릭터끼리 겹쳤을 때 빠져나갈 수 없다
        Vector2 end = start + new Vector2(vector.x * moveSpeed, vector.y * moveSpeed);

        theBC.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        theBC.enabled = true;

        if (hit.transform != null)
            return true;
        else
            return false;
    }

}
