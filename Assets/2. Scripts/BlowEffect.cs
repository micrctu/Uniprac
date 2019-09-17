using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowEffect : MonoBehaviour
{
    public float remainTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainTime -= Time.deltaTime;
        if(remainTime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
