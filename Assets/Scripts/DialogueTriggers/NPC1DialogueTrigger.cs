using UnityEngine;

public class NPC1DialogueTrigger : DialogueTrigger
{
    public Door door;
    private void OnEnable() {
        DialogueManager.onDialogueEnd += ObjectiveManager.instance.NextObjective;
    }
    private void OnDisable() {
        DialogueManager.onDialogueEnd -= ObjectiveManager.instance.NextObjective;
    }
    private void Start() {
        door = FindObjectOfType<Door>();
    }
    public override void TriggerDialogue()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        door.Open(playerTransform.position);    
        door.gameObject.GetComponent<Collider>().enabled = true;

        base.TriggerDialogue();    
    }

    
    
}