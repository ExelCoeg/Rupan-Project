using UnityEngine;

public class TVRemote : PickupableObject
{
    public TV tv;
    public override void Use()
    {
        if(tv.videoPlayer.isPlaying) tv.StopVideo();
        else tv.PlayVideo();
    }
}
