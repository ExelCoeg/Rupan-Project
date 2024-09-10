using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TV : InteractableObject
{
    private VideoPlayer videoPlayer;
    
    public override void Awake() {
        base.Awake();
        videoPlayer = GetComponentInChildren<VideoPlayer>();
    }
    public override void Interacted(){
        if(videoPlayer.isPlaying) StopVideo();
        else PlayVideo();
    }

    public void PlayVideo(){
        videoPlayer.Play();
    }
    public void StopVideo(){
        videoPlayer.Stop();
    }
}
