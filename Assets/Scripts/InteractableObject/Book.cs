using UnityEngine;

public class Book : InteractableObject
{
    [SerializeField] private float distance;
    
    public override void Interacted()
    {
        SoundManager.instance.PlaySound2D("PlayerInteract");
        UIManager.instance.ShowUI(UI.BOOK);
    }
    protected void Update() {
        distance = Vector3.SqrMagnitude(GameManager.instance.player.transform.position - transform.position);
        if(distance > 3f && UIManager.instance.currentUI == UI.BOOK){
            UIManager.instance.HideUI(UI.BOOK);
        }
    }

}
