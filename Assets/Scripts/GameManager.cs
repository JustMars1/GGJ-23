using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Dialogue,
    Gameplay
}

public class GameManager : MonoBehaviour
{
    public Menu menu;
    public GameplayUI gameplayUI;

    public GameState gameState = GameState.MainMenu;
    public int nextLevelIndex;

    public static GameManager Instance;

    bool paused;
    public bool Paused
    {
        get { return paused; }
        private set
        {
            paused = value;
            Time.timeScale = paused ? 0 : 1;

            if (gameState != GameState.MainMenu)
            {
                menu.pausePanel.gameObject.SetActive(paused);
                menu.optionsBt.gameObject.SetActive(paused);
            }
        }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        bool isMainMenu = GameManager.Instance.gameState == GameState.MainMenu;
        menu.startPanel.gameObject.SetActive(isMainMenu);
        menu.optionsBt.gameObject.SetActive(isMainMenu);

        gameplayUI.gameObject.SetActive(!isMainMenu);
        menu.pausePanel.gameObject.SetActive(false);
        menu.optionsPanel.gameObject.SetActive(false);

        Paused = false;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level" + nextLevelIndex);
    }

    void Update()
    {
        if (gameState != GameState.MainMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
        }
    }
}
