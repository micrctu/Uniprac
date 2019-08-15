using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
    private FadeManager theFade;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    public Canvas canvas;
    public Text text;

    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!flag)
        {
            flag = true;
            theOrder.SetPlayerNotMove();
            theFade.FadeOut(0.1f);
            canvas.sortingOrder = 3; //인풋필드가 포함된 캔버스의 sorting Layer 상 순위를 fadeout에 사용된 이미지보다 높이는 것
        }

        if(text.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            thePlayer.gameObject.name = text.text;
            canvas.sortingOrder = 1;//원래대로 복구
            theFade.FadeIn(0.01f);
            theOrder.SetPlayerMove();
            Destroy(this.gameObject);
        }
    }
}
