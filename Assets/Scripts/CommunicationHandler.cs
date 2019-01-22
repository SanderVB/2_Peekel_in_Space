using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CommunicationHandler : MonoBehaviour
{
    bool isActive = false;
    bool isFirstMessage = true;
    [SerializeField] TextMeshProUGUI textToChange;
    [SerializeField] CommunicationMessage firstMessage;
    [SerializeField][Range(0,1)] float messageVolume = .5f;
    [SerializeField] float writeTime = .05f;
    CommunicationMessage messageHolder;
    AudioClip audioHolder;

    private void OnEnable()
    {
        MessageChanger();
    }

    public void MessageChanger()
    {
        if (isFirstMessage)
        {
            isFirstMessage = false;
            messageHolder = firstMessage;
        }
        else
        {
            NextMessage();
        }
        //textToChange.text = messageHolder.GetMessage();
        StartCoroutine(WriteTextOneByOne(messageHolder.GetMessage()));
    }

    private IEnumerator WriteTextOneByOne(string message)
    {
        string textHolder = "";
        for(int i=0; i< message.Length; i++)
        {
            textHolder += message[i];
            textToChange.text = textHolder;
            yield return new WaitForSeconds(writeTime);
        }

    }

    private void NextMessage()
    {
        messageHolder = messageHolder.GetNextMessage();
    }

    //these two methods are obsolete since I'm using timeline now, keeping them around for now
    public IEnumerator MessageDisplayer(string message, float waitTime)
    {
        //MessageChanger(message);
        ActivitySwitcher();
        yield return new WaitForSeconds(waitTime);
        ActivitySwitcher();
    }

    private void ActivitySwitcher()
    {
        this.gameObject.SetActive(!isActive);
        isActive = !isActive;
    }
}
