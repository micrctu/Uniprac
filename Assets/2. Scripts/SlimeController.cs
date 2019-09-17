using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MovingObject
{
    public float wait_Time;
    private float remain_Time;

    public float attackDelay;
    
    // Start is called before the first frame update
    void Start()
    {
        remain_Time = wait_Time;
        theBC = GetComponent<BoxCollider2D>();
    }

    private bool CanAttack()
    {
        Vector3 playerPos = PlayerManager.instance.transform.position;

        if(Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(transform.position.x)) <= walkCount * moveSpeed * 1.01f)
        {
            if (Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(transform.position.y)) <= walkCount * moveSpeed * 0.5f)
            {
                return true;
            }
        }

        if (Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(transform.position.y)) <= walkCount * moveSpeed * 1.01f)
        {
            if (Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(transform.position.x)) <= walkCount * moveSpeed * 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    private void Attack()
    {
        Vector3 flip = transform.localScale;
        if (PlayerManager.instance.transform.position.x - transform.position.x > 0f) //자연스러운 attack 애니메이션 연출을 위해
        {
            flip.x = flip.x * -1.0f;
            transform.localScale = flip;
        }
           
        theAnim.SetTrigger("Attacking");
        StartCoroutine(AttackWaiting());
    }

    private IEnumerator AttackWaiting()
    {
        yield return new WaitForSeconds(attackDelay);

        if(CanAttack())
        {
            PlayerManager.instance.GetComponent<PlayerStats>().player_Hit(GetComponent<EnemyStats>().enemy_Atk);
        }

        yield return new WaitForSeconds(1f - attackDelay);

        Vector3 flip = transform.localScale;
        if(flip.x < 0f)
        {
            flip.x = flip.x * -1.0f;
            transform.localScale = flip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        remain_Time -= Time.deltaTime;
        if(remain_Time <= 0f)
        {
            remain_Time = wait_Time;
            if(CanAttack())
            {
                Attack();
                return;
            }
            base.Move("RANDOM");
        }
    }
}
