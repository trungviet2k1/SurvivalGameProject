using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button LoadGame;

    private void Start()
    {
        LoadGame.onClick.AddListener(() =>
        {
            SaveManager.Instance.StartLoadedGame();
        });
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}