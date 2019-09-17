using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int player_Hp;
    public int player_Mp;
    public int recover_Hp;
    public int recover_Mp;
    private float recover_Time;

    public int current_Hp;
    public int current_Mp;

    private float current_Time;

    public int player_Atk;
    public int player_Def;

    public int[] levelUp_Exp;
    private int current_Exp;
    private int current_Lv;

    public GameObject prefabs_Floating_Text;

    public bool isPlayerDie { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        current_Hp = player_Hp;
        current_Mp = player_Mp;
        recover_Time = 3f;
        current_Time = recover_Time;
        current_Exp = 0;
        current_Lv = 1;
        isPlayerDie = false;
    }

    public void player_Hit(int _enemyAttack)
    {
        int dmg = _enemyAttack - player_Def;
        if (dmg <= 0)
            dmg = 1;

        current_Hp -= dmg;
        if (current_Hp <= 0)
        {
            isPlayerDie = true;
            Debug.Log("체력 0미만, 게임오버");
        }

        Vector3 textPosition = PlayerManager.instance.transform.position + new Vector3(0, 50, 0);

        FloatingText ft = prefabs_Floating_Text.GetComponent<FloatingText>();
        ft.FloatingText_Setup(dmg.ToString(), "RED", 36);
        GameObject clone = Instantiate(prefabs_Floating_Text, textPosition, Quaternion.Euler(Vector3.zero));

        StopAllCoroutines();
        StartCoroutine(HitCoroutine());
    }

    private IEnumerator HitCoroutine()
    {
        Color color = this.gameObject.GetComponent<SpriteRenderer>().color;

        color.a = 0f;
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 0f;
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 0f;
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);

        color.a = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public void Recover_Hp(int _hp)
    {
        int recoverPoint;

        if ((current_Hp + _hp) > player_Hp)
        {
            recoverPoint = player_Hp - current_Hp;
            current_Hp = player_Hp;
        }           
        else
        {
            recoverPoint = _hp;
            current_Hp += _hp;
        }            
        //      Debug.Log("HP가" + _hp + " 회복되었습니다");

        Vector3 textPosition = PlayerManager.instance.transform.position + new Vector3(0,50,0);

        FloatingText ft = prefabs_Floating_Text.GetComponent<FloatingText>();
        ft.FloatingText_Setup("HP " + recoverPoint.ToString() + "회복!", "GREEN");
        GameObject clone = Instantiate(prefabs_Floating_Text, textPosition, Quaternion.Euler(Vector3.zero));       
    }

    public void Recover_Mp(int _mp)
    {
        int recoverPoint;

        if ((current_Mp + _mp) > player_Mp)
        {
            recoverPoint = player_Mp - current_Mp;
            current_Mp = player_Mp;
        }
        else
        {
            recoverPoint = _mp;
            current_Mp += _mp;
        }

        Vector3 textPosition = PlayerManager.instance.transform.position + new Vector3(0, 50, 0);

        FloatingText ft = prefabs_Floating_Text.GetComponent<FloatingText>();
        ft.FloatingText_Setup("MP " + recoverPoint.ToString() + "회복!", "BLUE");
        GameObject clone = Instantiate(prefabs_Floating_Text, textPosition, Quaternion.Euler(Vector3.zero));
    }

    public void Add_Exp(int _exp)
    {
        current_Exp += _exp;
        CheckPlayerLvUp();
    }

    private void CheckPlayerLvUp()
    {
        if (current_Lv >= levelUp_Exp.Length)
        {
            current_Lv = levelUp_Exp.Length;
            return;
        }
        else
        {   
            if (current_Exp >= levelUp_Exp[current_Lv])
            {
                current_Lv++;
                player_Atk++;
                player_Def++;
                player_Hp += 10;
                player_Mp += 2;

                current_Hp = player_Hp;
                current_Mp = player_Mp;

                Vector3 textPosition = PlayerManager.instance.transform.position + new Vector3(0, 50, 0);

                FloatingText ft = prefabs_Floating_Text.GetComponent<FloatingText>();
                ft.FloatingText_Setup("Lv UP! [Lv " + current_Lv.ToString() + "]", "RED");
                GameObject clone = Instantiate(prefabs_Floating_Text, textPosition, Quaternion.Euler(Vector3.zero));
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        current_Time -= Time.deltaTime;
        if(current_Time <= 0f)
        {
            current_Time = recover_Time;

            if(current_Hp < player_Hp)
            {
                current_Hp += recover_Hp;
                if (current_Hp > player_Hp)
                    current_Hp = player_Hp;
            }

            if (current_Mp < player_Mp)
            {
                current_Mp += recover_Mp;
                if (current_Mp > player_Mp)
                    current_Mp = player_Mp;
            }
        }
    }
}
