
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
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
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }
}
