using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessagesController : MonoBehaviour {

    public Text messagesText;

    private string[] messages;

    public IEnumerator ShowMessageRoutine()
    {
        yield return new WaitForSeconds(3);
        messagesText.text = "";
    }

    public IEnumerator ShowMessagesRoutine()
    {
        foreach(string message in messages)
        {
            messagesText.text = message;
            yield return new WaitForSeconds(2);
            messagesText.text = "";
        }
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        messagesText.text = message;
        StartCoroutine(ShowMessageRoutine());
    }

    public void ShowMessagesSequence(string[] messages)
    {
        this.messages = messages;
        StopAllCoroutines();
        StartCoroutine(ShowMessagesRoutine());
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
