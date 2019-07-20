using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private Camera theCamera;

    public GameObject target;
    private Vector3 targetPosition;

    public BoxCollider2D bound;
    private Vector3 maxBound;
    private Vector3 minBound;

    //카메라의 반너비, 반높이 값
    private float halfWidth;
    private float halfHeight;

    public float moveSpeed;

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

    // Start is called before the first frame update
    void Start()
    {
        theCamera = GetComponent<Camera>();
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
        maxBound = bound.bounds.max;
        minBound = bound.bounds.min;
    }


    public void SetBound(BoxCollider2D bc)
    {
        bound = bc;
        maxBound = bound.bounds.max;
        minBound = bound.bounds.min;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            //두 위치벡터 간 선형보간하여 카메라가 서서히 타겟(플레이어)을 따라잡도록 

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }

    }
}
