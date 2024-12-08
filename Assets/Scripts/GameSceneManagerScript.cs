using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManagerScript : MonoBehaviour
{
    public GameObject gameOverScene;
    public Text winText;

    public void GameOver(string text)
    {
        winText.text = text;
        gameOverScene.SetActive(true);
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
