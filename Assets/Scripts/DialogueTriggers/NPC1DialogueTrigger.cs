using UnityEngine;

public class NPC1DialogueTrigger : DialogueTrigger
{
    public Door door;
    public override bool TriggerDialogue()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        door.Open(playerTransform.position);
        door.gameObject.GetComponent<Collider>().enabled =true;
        return base.TriggerDialogue();
    }
}
