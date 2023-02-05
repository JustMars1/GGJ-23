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

    public List<GameObject> vines = new List<GameObject>();
    public List<GameObject> platforms = new List<GameObject>();
    public List<GameObject> bridges = new List<GameObject>();

    public List<GameObject> seeds = new List<GameObject>();

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

        menu.resumeBt.onClick.AddListener(() =>
        {
            menu.PlayBtClickSound();
            Paused = false;
        });

        menu.restartBt.onClick.AddListener(() =>
        {
            menu.PlayBtClickSound();
            ResetLevel();
            Paused = false;
        });

        menu.mainMenuBt.onClick.AddListener(() =>
        {
            menu.PlayBtClickSound();
            SceneManager.LoadScene("Main");
        });

        Paused = false;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level" + nextLevelIndex);
    }

    public void ResetLevel()
    {
        foreach (GameObject seed in seeds)
        {
            if (seed != null)
            {
                Destroy(seed);
            }
        }

        seeds.Clear();

        foreach (GameObject platform in platforms)
        {
            Destroy(platform);
        }

        platforms.Clear();

        foreach (GameObject vine in vines)
        {
            Destroy(vine);
        }

        vines.Clear();

        foreach (GameObject bridge in bridges)
        {
            Destroy(bridge);
        }

        bridges.Clear();
    }

    void Update()
    {
        if (gameState != GameState.MainMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
        }
    }
}
