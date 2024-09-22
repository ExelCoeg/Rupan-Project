using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
    public PlayableDirector playableDirectorTrigger;
}
 
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool isPlayInStart = false;
    public bool isPlayed = false;
    public bool enableNextDialogueLine;
    public bool enableUIAfterDialogueLine;
    private void Start() {
        if (isPlayInStart)
        {
            TriggerDialogue();
        }
        
    }
 
    public virtual void TriggerDialogue()
    {
        if(enableUIAfterDialogueLine)
        {
            DialogueManager.instance.EnableUIAfterDialogue();
        }
        else{
            DialogueManager.instance.DisableUIAfterDialogue();
        }

        if(enableNextDialogueLine)
        {
            DialogueManager.instance.EnableNextDialogueAction();
        }
        else{
            DialogueManager.instance.DisableNextDialogueAction();
        }
        isPlayed = true;
        DialogueManager.instance.StartDialogue(dialogue);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            if(!isPlayed){
                TriggerDialogue();
            }
        }
    }
}