using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{

    private PlayerManager thePlayer;
    private CameraManager theCamera;

    public Transform target_point;
    public BoxCollider2D target_bound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            thePlayer.transform.position = target_point.position;
            theCamera.SetBound(target_bound);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
