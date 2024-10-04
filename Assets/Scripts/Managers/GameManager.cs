
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public bool isPaused;
    public Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
    }
    // Start is called before the first frame update
    public void PlayGame(){
        SceneManager.LoadScene("Game");
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame(){
        Time.timeScale = 0;
        foreach(VideoPlayer videoPlayer in FindObjectsOfType<VideoPlayer>()){
            if(videoPlayer.isPlaying){
                videoPlayer.Pause();
            }
        }
        isPaused = true;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        isPaused = false;
        foreach(VideoPlayer videoPlayer in FindObjectsOfType<VideoPlayer>()){
            if(videoPlayer.isPaused){
                videoPlayer.Play();
            }
        }
    }
}
