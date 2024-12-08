using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManagerScript : MonoBehaviour
{
    public GameObject gameOverScene;
    public Text scoreText;

    public void GameOver(string text)
    {
        gameOverScene.SetActive(true);
        scoreText.text = text;
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
