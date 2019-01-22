using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Communication Message")]

public class CommunicationMessage : ScriptableObject
{

    [TextArea(10, 14)] [SerializeField] string messageText;
    [SerializeField] CommunicationMessage nextMessage;
    [SerializeField] AudioClip messageSound;

    public string GetMessage() { return messageText; }

    public AudioClip GetMessageAudio() { return messageSound; }

    public CommunicationMessage GetNextMessage() { return nextMessage; }

}
