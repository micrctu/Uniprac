using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public int enemy_Atk;
    public int enemy_Def;

    public int enemy_Hp;
    public int get_Exp; //잡았을때 플레이어가 얻는 경험치
    private int current_Hp;

    public GameObject HP_Bar;
    public Image HP_Bar_Filled;

    public GameObject prefabs_Floating_Text;
    public GameObject blow_Effect;

    // Start is called before the first frame update
    void Start()
    {
        current_Hp = enemy_Hp;
    }

    public void enemy_Hit(int _playerAtk)
    {
        int dmg = _playerAtk - enemy_Def;
        if (dmg <= 0)
            dmg = 1;

        current_Hp -= dmg;
        if (current_Hp <= 0)
            current_Hp = 0;

        GameObject blowEffect = Instantiate(blow_Effect, transform.position, Quaternion.Euler(Vector3.zero));

        Vector3 textPosition = transform.position + new Vector3(0, 50, 0);
        prefabs_Floating_Text.GetComponent<FloatingText>().FloatingText_Setup(dmg.ToString(), "WHITE", 40);
        GameObject floatingText = Instantiate(prefabs_Floating_Text, textPosition, Quaternion.Euler(Vector3.zero));

        HP_Bar_Filled.fillAmount = (float)current_Hp / enemy_Hp;
        HP_Bar.SetActive(true);

        StopAllCoroutines();

        if (current_Hp <= 0)
        {
            PlayerManager.instance.gameObject.GetComponent<PlayerStats>().Add_Exp(get_Exp);
            Destroy(this.gameObject);
            return;
        }

        StartCoroutine(HP_Bar_RemainCoroutine());
    }

    private IEnumerator HP_Bar_RemainCoroutine()
    {
        yield return new WaitForSeconds(3f);
        HP_Bar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
