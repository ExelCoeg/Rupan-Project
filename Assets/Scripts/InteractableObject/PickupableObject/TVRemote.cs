using UnityEngine;

public class TVRemote : PickupableObject
{
    public TV tv;
    public Couch couch;
    public override void Interacted()
    {
        base.Interacted();
        couch.EnableInteractable();
    }
    public override void Use()
    {
        
        CutsceneManager.instance.PlayCutscene(0);

    }
}
