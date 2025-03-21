using DG.Tweening;
using UnityEngine;

public class NPC1DialogueTrigger : DialogueTrigger
{
    public Door door;
    public float detectDoorRadius = 1f;
    public Transform pointToMove;
    public Transform lookAtTransform;


   
    private void OnEnable() {
        DialogueManager.onDialogueEnd += ObjectiveManager.instance.NextObjective;
    }   
    private void OnDisable() {
        DialogueManager.onDialogueEnd -= ObjectiveManager.instance.NextObjective;
    }
    private void Start() {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectDoorRadius,LayerMask.GetMask("Interactable"));
        foreach(Collider hit in hits){
            if(hit.CompareTag("Door")){
                door = hit.GetComponent<Door>();
            }
        }
    }
    
    public override void TriggerDialogue()
    {

        Sequence sequence = DOTween.Sequence();
        sequence.Append(GameManager.instance.player.transform.DOMove(pointToMove.position,0.5f));
        sequence.Join(GameManager.instance.player.transform.DODynamicLookAt(lookAtTransform.position,1f,AxisConstraint.Y).OnComplete(()=>GameManager.instance.player.DisableControls()));
        

        Transform playerTransform = GameManager.instance.player.transform;
        door.Open(playerTransform.position);
        door.EnableInteractable();

        base.TriggerDialogue();

    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, detectDoorRadius);
        Gizmos.color = Color.red;
    }
    
}
