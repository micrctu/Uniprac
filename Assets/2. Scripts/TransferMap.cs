using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{

    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private OrderManager theOrder;

    private Animator playerAnim;

    public Transform target_point;
    public BoxCollider2D target_bound;

    public string transferMapname;
    private bool flag = true; //OnTriggerEnter2D 중복 실행 방지

    //door 관련 변수
    [Range(0, 2)]
    public int doorCount;
    public string doorOpenDirection;
    public Animator LeftDoorAnim;
    public Animator RightDoorAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(doorCount == 0)
        {
            if (flag && collision.transform.tag == "Player")
            {
                WeatherManager.instance.Off_WeatherEffect();
                StartCoroutine(TransferCoroutine());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(flag && collision.transform.tag == "Player" && Input.GetKeyDown(KeyCode.Z))
        {
            float dirX = playerAnim.GetFloat("DirX");
            float dirY = playerAnim.GetFloat("DirY");

            switch (doorOpenDirection)
            {
                case "UP":
                    if (dirY == 1.0f)
                        StartCoroutine(TransferCoroutine());
                    break;
                case "DOWN":
                    if (dirY == -1.0f)
                        StartCoroutine(TransferCoroutine());
                    break;
                case "LEFT":
                    if (dirX == -1.0f)
                        StartCoroutine(TransferCoroutine());
                    break;
                case "RIGHT":
                    if (dirX == 1.0f)
                        StartCoroutine(TransferCoroutine());
                    break;
            }
        }
    }

    private IEnumerator TransferCoroutine()
    {
        flag = false;

        theOrder.PreLoadMovingObjects();
        theOrder.SetPlayerNotMove();

        if(doorCount >= 1)
        {
            if(doorCount == 1)
            {
                LeftDoorAnim.SetBool("Open", true);
                yield return new WaitForSeconds(1.0f);
                theOrder.SetTransparent("Player");
                LeftDoorAnim.SetBool("Open", false);
                yield return new WaitForSeconds(1.0f);
            }
            else if (doorCount == 2)
            {
                LeftDoorAnim.SetBool("Open", true);
                RightDoorAnim.SetBool("Open", true);
                yield return new WaitForSeconds(1.0f);
                theOrder.SetTransparent("Player");
                LeftDoorAnim.SetBool("Open", false);
                RightDoorAnim.SetBool("Open", false);
                yield return new WaitForSeconds(1.0f);
            }
        }

        theFade.FadeOut(0.02f);
        //        theFade.FlashOut(0.02f);

        yield return new WaitForSeconds(1f);
       
        thePlayer.transform.position = target_point.position;
        theCamera.SetBound(target_bound);
        thePlayer.currentMapName = transferMapname;

        if (doorCount >= 1)
            theOrder.UnSetTransparent("Player");

        theFade.FadeIn(0.02f);
  //      theFade.FlashIn(0.02f);

        yield return new WaitForSeconds(1f);

        theOrder.SetPlayerMove();
        flag = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();

        playerAnim = thePlayer.GetComponent<Animator>();
    }
}
