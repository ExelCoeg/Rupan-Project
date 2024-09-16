using UnityEngine;

public class NPC1DialogueTrigger : DialogueTrigger
{
    public Door door;
    public override void TriggerDialogue()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        door.Open(playerTransform.position);
        door.gameObject.GetComponent<Collider>().enabled =true;
        
        base.TriggerDialogue();
    
    }

    private void Start() {
        door = FindObjectOfType<Door>();
    }
}
