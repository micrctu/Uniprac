using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!activated && collision.gameObject.tag == "Enemy")
        {
            activated = true;
            int playerAtk = PlayerManager.instance.gameObject.GetComponent<PlayerStats>().player_Atk;
            collision.gameObject.GetComponent<EnemyStats>().enemy_Hit(playerAtk);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        activated = false;
    }
    
    // Update is called once per frame
    void Update()
    {            
    }
}
