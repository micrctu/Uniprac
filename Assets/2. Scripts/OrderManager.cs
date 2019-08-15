using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;

    private PlayerManager thePlayer;
    private List<MovingObject> movingObjects; //모든 MovingObject 관리 list(플레이어, NPC들, ENEMY들 등등)

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

    public void PreLoadMovingObjects() // 모든 MovingObject 들을 찾아와서 리스트에 넣는 함수
    {
        movingObjects = ToList();
    }

    private List<MovingObject> ToList()
    {
        MovingObject[] m_Objects = FindObjectsOfType<MovingObject>();
        List<MovingObject> tempList = new List<MovingObject>();
        for (int i = 0; i < m_Objects.Length; i++)
        {
            tempList.Add(m_Objects[i]);
        }
        return tempList;
    }

    public void SetPlayerMove()
    {
        thePlayer.notMove = false;
    }

    public void SetPlayerNotMove()
    {
        thePlayer.notMove = true;
    }

    public void Move(string name, string dir)
    {
        for (int i = 0; i < movingObjects.Count; i++)
        {
            if(movingObjects[i].objectName == name)
            {
                movingObjects[i].Move(dir); //기본 freq인 5로 dir 방향으로 한번 이동
                break;
            }
        }
    }

    public void Turn(string name, string dir)
    {
        for (int i = 0; i < movingObjects.Count; i++)
        {
            if (movingObjects[i].objectName == name)
            {
                movingObjects[i].theAnim.SetFloat("DirX", 0f);
                movingObjects[i].theAnim.SetFloat("DirY", 1f);

                switch (dir)
                {
                    case "UP":
                        movingObjects[i].theAnim.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        movingObjects[i].theAnim.SetFloat("DirY", -1f);
                        break;
                    case "LEFT":
                        movingObjects[i].theAnim.SetFloat("DirX", -1f);
                        break;
                    case "RIGHT":
                        movingObjects[i].theAnim.SetFloat("DirX", 1f);
                        break;
                }
                break;
            }
        }
    }

    public void SetTransparent(string name)
    {
        for (int i = 0; i < movingObjects.Count; i++)
        {
            if (movingObjects[i].objectName == name)
            {
                Color color = movingObjects[i].gameObject.GetComponent<SpriteRenderer>().color;
                color.a = 0f;
                movingObjects[i].gameObject.GetComponent<SpriteRenderer>().color = color;
                break;
            }
        }
    }

    public void UnSetTransparent(string name)
    {
        for (int i = 0; i < movingObjects.Count; i++)
        {
            if (movingObjects[i].objectName == name)
            {
                Color color = movingObjects[i].gameObject.GetComponent<SpriteRenderer>().color;
                color.a = 1f;
                movingObjects[i].gameObject.GetComponent<SpriteRenderer>().color = color;
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

}
