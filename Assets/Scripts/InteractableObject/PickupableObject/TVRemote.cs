using UnityEngine;

public class TVRemote : PickupableObject
{
    public TV tv;
    public override void Use()
    {
        
        CutsceneManager.instance.PlayCutscene(0);

    }
}
