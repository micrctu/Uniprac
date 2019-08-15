using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private PlayerManager thePlayer;
    private Animator theAnim;

    private Vector2 vector; //플레이어 방향 저장

    public void On_LightMode()
    {
        this.gameObject.SetActive(true);
    }

    public void Off_LightMode()
    {
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theAnim = thePlayer.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = thePlayer.transform.position;

        vector.Set(theAnim.GetFloat("DirX"), theAnim.GetFloat("DirY"));

        if(vector.x == 1.0f)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 90);//라이트 회전 값 설정(플레이어가 바라보는 방향으로)
        }
        else if(vector.x == -1.0f)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (vector.y == 1.0f)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (vector.y == -1.0f)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
