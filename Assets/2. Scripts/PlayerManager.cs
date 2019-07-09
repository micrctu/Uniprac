using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int walkCount;
    private int currentWalkCount;
    public float moveSpeed;

    public bool notMove = false; //외부에서 플레이어 이동관련 제약을 걸때 사용
    private bool canMove = true; //이동 시 마다 일정 단위로 이동 보장을 위한 flag 
    private bool attacking = false;

    private Vector3 vector;

    IEnumerator MoveCoroutine()
    {
        currentWalkCount = 0;
        while(currentWalkCount < walkCount)
        {
            this.transform.Translate(vector.x * moveSpeed, vector.y * moveSpeed, vector.z);
            currentWalkCount++;
            yield return new WaitForSeconds(0.002f);
        }
        canMove = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!notMove && canMove && !attacking)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (x != 0 || y != 0)
            {
                if (x != 0)
                    vector.Set(x, 0, 0);
                else
                    vector.Set(0, y, 0);

                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
        
    }
}
