using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}
 
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
 
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
 
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool isPlayInStart = false;
    public bool isPlayeOnce = true;
    public bool isPlayed = false;
    private void Start() {
        if (isPlayInStart)
        {
            TriggerDialogue();
        }
    }
 
    public bool TriggerDialogue()
    {
        return DialogueManager.Instance.StartDialogue(dialogue);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && !isPlayInStart)
        {
            if (isPlayeOnce && !isPlayed)
            {
                isPlayed = TriggerDialogue();
            }else if(!isPlayeOnce){
                TriggerDialogue();
            }
            
        }
    }
}