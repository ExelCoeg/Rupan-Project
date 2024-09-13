using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    
    public void Awake() {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
    }


    public void PlayVideo(){
        videoPlayer.Play();
    }
    public void StopVideo(){
        videoPlayer.Stop();
    }
}
