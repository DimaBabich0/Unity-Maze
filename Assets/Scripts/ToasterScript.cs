using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

struct Message
{
    public string text;
    public float time;

    public Message(string text, float time)
    {
        this.text = text;
        this.time = time;
    }
}

public class ToasterScript : MonoBehaviour
{
    private static ToasterScript instance;
    private const float showtime = 3.0f;

    private GameObject content1;
    private TMPro.TextMeshProUGUI messageBox1;
    private CanvasGroup contentGroup1;
    private float timeout1 = 0.0f;
    private float messageTimeout1 = 0.0f;

    private GameObject content2;
    private TMPro.TextMeshProUGUI messageBox2;
    private CanvasGroup contentGroup2;
    private float timeout2 = 0.0f;
    private float messageTimeout2 = 0.0f;

    private Queue<Message> messages = new Queue<Message>();

    void Start()
    {
        instance = this;

        Transform t = this.transform.Find("Context #1");
        content1 = t.gameObject;
        contentGroup1 = content1.GetComponent<CanvasGroup>();
        messageBox1 = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        content1.SetActive(false);

        t = this.transform.Find("Context #2");
        content2 = t.gameObject;
        contentGroup2 = content2.GetComponent<CanvasGroup>();
        messageBox2 = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        content2.SetActive(false);

        GameState.AddListener(onGameStateChanged);
    }

    void Update()
    {
        if (messages.Count > 0 && timeout1 <= 0)
        {
            Message m = messages.Dequeue();

            content1.SetActive(true);
            messageBox1.text = m.text;
            messageTimeout1 = timeout1 = m.time;
        }

        if (messages.Count > 0 && timeout2 <= 0)
        {
            Message m = messages.Dequeue();

            content2.SetActive(true);
            messageBox2.text = m.text;
            messageTimeout2 = timeout2 = m.time;
        }

        if (timeout1 > 0)
        {
            timeout1 -= Time.deltaTime;

            if (messageTimeout1 - timeout1 < 1f) //fade in
                contentGroup1.alpha = Mathf.Clamp01((messageTimeout1 - timeout1) * 5.0f);
            else if (messageTimeout1 - timeout1 > 1f) //fade out
                contentGroup1.alpha = Mathf.Clamp01(timeout1 * 5.0f);

            if (timeout1 <= 0)
            {
                content1.SetActive(false);
                timeout1 = 0.0f;

                if (timeout2 > 0)
                {
                    messageBox1.text = messageBox2.text;
                    messageTimeout1 = messageTimeout2;
                    timeout1 = timeout2;
                    contentGroup1.alpha = contentGroup2.alpha;
                    content1.SetActive(true);

                    messageBox2.text = "";
                    timeout2 = messageTimeout2 = 0.0f;
                    contentGroup2.alpha = 0;
                    content2.SetActive(false);

                    if (messages.Count > 0)
                    {
                        Message m = messages.Dequeue();

                        content2.SetActive(true);
                        messageBox2.text = m.text;
                        messageTimeout2 = timeout2 = m.time;
                        contentGroup1.alpha = 0;
                    }
                }
            }
        }

        if (timeout2 > 0)
        {
            timeout2 -= Time.deltaTime;

            if (messageTimeout2 - timeout2 < 1f) //fade in
                contentGroup2.alpha = Mathf.Clamp01((messageTimeout2 - timeout2) * 5.0f);
            else if (messageTimeout2 - timeout2 > 2f) //fade out
                contentGroup2.alpha = Mathf.Clamp01(timeout2 * 5.0f);

            if (timeout2 <= 0)
            {
                content2.SetActive(false);
                timeout2 = 0.0f;
            }
        }
    }

    public static void Toast(string text, float time = 0.0f)
    {
        instance.messages.Enqueue(new Message(
            text,
            time > 0.0f ? showtime : time
        ));
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isKeyRedCollected))
            Toast("You find a red key.\nYou can now open red gates", 2f);
        else if (fieldName == nameof(GameState.isKeyBlueCollected))
            Toast("You find a blue key.\nYou can now open blue gates", 2f);
        else if (fieldName == nameof(GameState.isKeyGreenCollected))
            Toast("You find a green key.\nYou can now open green gates", 2f);
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(onGameStateChanged);
    }
}
