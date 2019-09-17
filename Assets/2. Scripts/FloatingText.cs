using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float existTime;
    public float floatingSpeed;

    private Text text;
    private Vector3 vector;

    public void FloatingText_Setup(string _text, string _color = "RED", int _fontSize = 32)
    {
        text = this.gameObject.GetComponent<Text>();

        text.text = _text;
        switch(_color)
        {
            case "BLUE":
                text.color = Color.blue;
                break;
            case "RED":
                text.color = Color.red;
                break;
            case "GREEN":
                text.color = Color.green;
                break;
            case "BLACK":
                text.color = Color.black;
                break;
            case "WHITE":
                text.color = Color.white;
                break;
            default:
                text.color = Color.red;
                break;
        }
        text.fontSize = _fontSize;
    }
  
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("SystemCanvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        vector.Set(this.gameObject.transform.position.x, this.gameObject.transform.position.y + (floatingSpeed * Time.deltaTime), 
            this.gameObject.transform.position.z);
        this.gameObject.transform.position = vector;

        existTime -= Time.deltaTime;

        if(existTime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
