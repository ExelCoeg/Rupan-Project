
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public bool isPaused;
    // Start is called before the first frame update
    public void PlayGame(){
        SceneManager.LoadScene("Game");
    }
    public void GameOver(){
        Debug.Log("Game Over");
    }   

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame(){
        Time.timeScale = 0;
        foreach(VideoPlayer videoPlayer in FindObjectsOfType<VideoPlayer>()){
            videoPlayer.Pause();
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
