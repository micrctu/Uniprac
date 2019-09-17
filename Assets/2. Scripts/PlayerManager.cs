using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    public static PlayerManager instance;

    private float input_X;
    private float input_Y;

    public bool notMove = false; //외부에서 플레이어 이동관련 제약을 걸때 사용
    private bool attacking = false;

    public string footStepMusic1, footStepMusic2, footStepMusic3, footStepMusic4;
    public string currentMapName;

    public float attackDelay;
    private float remainTime;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion Singleton
    }

    IEnumerator MoveCoroutine()
    {
        while ((input_X != 0 || input_Y != 0) && !notMove && !attacking)
        {
            if (input_X != 0)
                vector.Set(input_X, 0, transform.position.z);
            else
                vector.Set(0, input_Y, transform.position.z);

            if (base.CheckCollision())
                break;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRun = true;
                applyRunSpeed = runSpeed;
            }
            else
            {
                applyRun = false;
                applyRunSpeed = 0f;
            }

            currentWalkCount = 0;

            theAnim.SetFloat("DirX", vector.x);
            theAnim.SetFloat("DirY", vector.y);
            theAnim.SetBool("Walking", true);

            int temp = Random.Range(1, 5);
            switch(temp)
            {
                case 1:
                    theAudio.Play(footStepMusic1);
                    break;
                case 2:
                    theAudio.Play(footStepMusic2);
                    break;
                case 3:
                    theAudio.Play(footStepMusic3);
                    break;
                case 4:
                    theAudio.Play(footStepMusic4);
                    break;
            }

            theBC.offset = new Vector2(vector.x * moveSpeed * walkCount, vector.y * moveSpeed * walkCount);
            //움직이기 전에 boxCollider 위치를 먼저 옮겨서 다른 이동과 겹쳐지는 것을 방지

            while (currentWalkCount < walkCount)
            {
                this.transform.Translate(vector.x * (moveSpeed + applyRunSpeed), vector.y * (moveSpeed + applyRunSpeed), 0);
                currentWalkCount++;
                if (applyRun)
                    currentWalkCount++; //즉, runSpeed 값은 moveSpeed 값과 같게 설정해야 함(즉, 달릴때 두배의 속력 적용)

                if (currentWalkCount == 12)
                    theBC.offset = Vector2.zero;

                yield return new WaitForSeconds(0.002f);
            }
            input_X = Input.GetAxisRaw("Horizontal");
            input_Y = Input.GetAxisRaw("Vertical");
        }
        canMove = true;
        theAnim.SetBool("Walking", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        theBC = GetComponent<BoxCollider2D>();
        theAnim = GetComponent<Animator>();
        theAudio = FindObjectOfType<AudioManager>();
        remainTime = attackDelay;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !notMove && !attacking)
        {
            input_X = Input.GetAxisRaw("Horizontal");
            input_Y = Input.GetAxisRaw("Vertical");

            if (input_X != 0 || input_Y != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }

        if (attacking)
        {
            remainTime -= Time.deltaTime;
            if(remainTime <= 0f)
            {
                remainTime = attackDelay;
                attacking = false;
            }
        }

        else if (!notMove)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                attacking = true;
                theAnim.SetTrigger("Attacking");
            }
        }
    }
}
