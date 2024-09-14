using UnityEngine;

public class Couch : InteractableObject
{
    // private void Update() {
    //     if(FindObjectOfType<Player>().TryGetComponent(out Player player)){
    //         if(player.currentPickupableObject is TVRemote && !isInteractable){
    //             isInteractable = true;
    //         }
    //     }
    // }
    public override void Interacted()
    {
        GameObject.Find("Player").GetComponent<Player>().PlayTimeline("PlayerSit");
    }

}
