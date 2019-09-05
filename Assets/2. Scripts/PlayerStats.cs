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


    // Start is called before the first frame update
    void Start()
    {
        current_Hp = player_Hp;
        current_Mp = player_Mp;
        recover_Time = 3f;
        current_Time = recover_Time;
        current_Exp = 0;
        current_Lv = 1;
    }

    public void Recover_Hp(int _hp)
    {
        if ((current_Hp + _hp) > player_Hp)
            current_Hp = player_Hp;
        else
            current_Hp += _hp;

        Debug.Log("HP가" + _hp + " 회복되었습니다");
    }

    public void Recover_Mp(int _mp)
    {
        if ((current_Mp + _mp) > player_Mp)
            current_Mp = player_Mp;
        else
            current_Mp += _mp;

        Debug.Log("MP가" + _mp + " 회복되었습니다");
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
