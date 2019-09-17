using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public string objectName;

    public int walkCount;
    protected int currentWalkCount;

    public float moveSpeed;
    public float runSpeed;

    protected bool applyRun = false;
    protected float applyRunSpeed;

    public LayerMask layerMask;

    protected BoxCollider2D theBC;
    public Animator theAnim; //OrderManager를 통한 이동제어시 애니메이션 접근 필요하므로 
    protected AudioManager theAudio;

    protected Vector3 vector;

    public bool canMove = true; //이미 이동중에 또다른 이동 코루틴 동작 방지
    
    protected bool CheckCollision()
    {
        RaycastHit2D hit;

        Vector2 start = new Vector2(this.gameObject.transform.position.x + (vector.x * moveSpeed * walkCount), 
                                    this.gameObject.transform.position.y + (vector.y * moveSpeed * walkCount));
        //만약, 게임오브젝트의 위치에서 linecast를 한다면 캐릭터끼리 겹쳤을 때 빠져나갈 수 없다

        //Vector2 end = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        Vector2 end = start + new Vector2(vector.x * moveSpeed, vector.y * moveSpeed);

        theBC.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        theBC.enabled = true;

        if (hit.transform != null)
            return true;
        else
            return false;
    }

    public void Move(string direction, int freq = 5)
    {
        StartCoroutine(MoveCoroutine(direction, freq));
    }

    private IEnumerator MoveCoroutine(string direction, int freq)
    {
        switch (direction)
        {
            case "UP":
                vector.Set(0, 1, transform.position.z);
                break;
            case "DOWN":
                vector.Set(0, -1, transform.position.z);
                break;
            case "LEFT":
                vector.Set(-1, 0, transform.position.z);
                break;
            case "RIGHT":
                vector.Set(1, 0, transform.position.z);
                break;
            case "RANDOM":
                int temp = Random.Range(1, 5);
                if(temp == 1)
                    vector.Set(0, 1, transform.position.z);
                else if (temp == 2)
                    vector.Set(0, -1, transform.position.z);
                else if (temp == 3)
                    vector.Set(-1, 0, transform.position.z);
                else if (temp == 4)
                    vector.Set(1, 0, transform.position.z);
                break;
        }

        switch (freq)
        {
            case 1:
                yield return new WaitForSeconds(4f);
                break;
            case 2:
                yield return new WaitForSeconds(3f);
                break;
            case 3:
                yield return new WaitForSeconds(2f);
                break;
            case 4:
                yield return new WaitForSeconds(1f);
                break;
            case 5:
                break;
        }

        currentWalkCount = 0;

        theAnim.SetFloat("DirX", vector.x);
        theAnim.SetFloat("DirY", vector.y);
        
        if (!CheckCollision())
        {
            theBC.offset = new Vector2(vector.x * moveSpeed * walkCount, vector.y * moveSpeed * walkCount);
            //움직이기 전에 boxCollider 위치를 먼저 옮겨서 다른 이동과 겹쳐지는 것을 방지

            theAnim.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                this.transform.Translate(vector.x * (moveSpeed + applyRunSpeed), vector.y * (moveSpeed + applyRunSpeed), 0);
                currentWalkCount++;

                if (currentWalkCount == 12)
                    theBC.offset = Vector2.zero;

                yield return new WaitForSeconds(0.001f);
            }
        }

        theAnim.SetBool("Walking", false);
        canMove = true;
    }
}
