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
    private static GameObject content;
    private static TMPro.TextMeshProUGUI messageBox;
    private static List<Message> messages = new List<Message>();
    
    private static float timeout = 0.0f;
    private static float showtime = 3.0f;

    void Start()
    {
        Transform t = this.transform.Find("Toaster");
        content = t.gameObject;
        messageBox = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();

        content.SetActive(false);
    }

    void Update()
    {
        if (messages.Count > 0 && timeout <= 0)
        {
            Message m = messages[0];
            messages.RemoveAt(0);

            content.SetActive(true);
            messageBox.text = m.text;
            timeout = m.time;
        }

        if (timeout > 0)
        {
            timeout -= Time.deltaTime;
            if (timeout <= 0)
            {
                content.SetActive(false);
                timeout = 0.0f;
            }
        }
    }

    public static void Toast(string text, float time = 0.0f)
    {
        messages.Add(new Message(text, time != 0.0f ? showtime : time));
    }
}
