using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    [TextArea(1,2)]
    public string[] sentences;

    public Sprite[] characters;
    public Sprite[] windows;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private List<string> sentences;
    private List<Sprite> characters;
    private List<Sprite> windows;
    
    public SpriteRenderer window_Renderer;
    public SpriteRenderer sprite_Renderer;
    public Text text;

    public Animator window_Animator;
    public Animator sprite_Animator;

    private int count;
    private bool talking;
    private bool keyActivated;
    private bool onlyText;

    public string typeSound;
    public string zSound;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
          //  DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion Singleton
    }

    public void ShowText(Dialogue dl)
    {
        talking = true;
        onlyText = true;

        for (int i = 0; i < dl.sentences.Length; i++)
        {
            sentences.Add(dl.sentences[i]);
        }
        StartCoroutine(TextCoroutine());
    }

    private IEnumerator TextCoroutine()
    {
        keyActivated = true;

        for (int i = 0; i < sentences[count].Length; i++)
        {
            char tmp = sentences[count][i];
            text.text += tmp;

            if (i % 3 == 1)
                AudioManager.instance.Play(typeSound);

            yield return new WaitForSeconds(0.04f);
        }
    }

    public void ShowDialogue(Dialogue dl)
    {
        talking = true;
        onlyText = false;

        for (int i = 0; i < dl.sentences.Length; i++)
        {
            sentences.Add(dl.sentences[i]);
            characters.Add(dl.characters[i]);
            windows.Add(dl.windows[i]);
        }
        StartCoroutine(DialogueCoroutine());
    }

    private IEnumerator DialogueCoroutine()
    {
        if(count > 0)
        {
            if(characters[count] != characters[count-1]) //즉, 대화 대상이 바껴서 sprite, window 다 바꿔야 될때
            {
                window_Animator.SetBool("Appear", false);
                sprite_Animator.SetBool("Change", true);

                window_Renderer.sprite = windows[count];
                sprite_Renderer.sprite = characters[count];

                yield return new WaitForSeconds(0.1f);

                window_Animator.SetBool("Appear", true);
                sprite_Animator.SetBool("Change", false);

                yield return new WaitForSeconds(0.4f);
            }
            else
                yield return new WaitForSeconds(0.05f);
        }
        else //최초 대화시 
        {
            window_Renderer.sprite = windows[count];
            sprite_Renderer.sprite = characters[count];
            window_Animator.SetBool("Appear", true);
            sprite_Animator.SetBool("Appear", true);
        }

        keyActivated = true;

        for (int i = 0; i < sentences[count].Length; i++)
        {
            char tmp = sentences[count][i];
            text.text += tmp;

            if (i % 3 == 1)
                AudioManager.instance.Play(typeSound);

            yield return new WaitForSeconds(0.04f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        text.text = "";
        talking = false;
        keyActivated = false;

        sentences = new List<string>();
        characters = new List<Sprite>();
        windows = new List<Sprite>();
    }

    private void ExitDialogue()
    {
        talking = false;
        keyActivated = false;

        window_Animator.SetBool("Appear", false);
        sprite_Animator.SetBool("Appear", false);

        count = 0;
        text.text = "";
        sentences.Clear();
        characters.Clear();
        windows.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(talking && keyActivated)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                count++;
                text.text = "";
                keyActivated = false;
                AudioManager.instance.Play(zSound);

                StopAllCoroutines();

                if (count == sentences.Count)
                    ExitDialogue();
                else
                {
                    if(onlyText)
                        StartCoroutine(TextCoroutine());
                    else
                        StartCoroutine(DialogueCoroutine());
                }   
            }
        }
        
    }
}
