using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;
using GateGame.UI.HUD;
using GateGame.Game;
using Core.Game;


public class GameOverMenu : MonoBehaviour
{

    private GComponent mainUI;
    GameWinWindow gameWinWindow;
    GameWinWindow1 gameWinWindow1;
    GameLoseWindow gameLoseWindow;

    SceneLoader sceneLoader;


    /// <summary>
    /// Reference to the <see cref="LevelManager" />
    /// </summary>
    protected GateGame.Level.LevelManager m_LevelManager;


    // Start is called before the first frame update
    void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        sceneLoader = GetComponent<SceneLoader>();
        gameWinWindow = new GameWinWindow(this);
        gameLoseWindow = new GameLoseWindow(this);
        gameWinWindow1 = new GameWinWindow1(this);

        LazyLoad();
        m_LevelManager.levelCompleted += Victory;
        m_LevelManager.levelFailed += Defeat;
    }

    /// <summary>
    /// Safely unsubscribes from <see cref="LevelManager" /> events.
    /// Goes to the next scene if valid
    /// </summary>
    public void GoToNextLevel()
    {
        SafelyUnsubscribe();
        if (!GateGameManager.instanceExists)
        {
            return;
        }
        GateGameManager gm = GateGameManager.instance;
        LevelItem item = gm.GetLevelForCurrentScene();
        LevelList list = gm.levelList;
        int levelCount = list.Count;
        int index = -1;
        for (int i = 0; i < levelCount; i++)
        {
            if (item == list[i])
            {
                index = i + 1;
                break;
            }
        }
        if (index < 0 || index >= levelCount)
        {
            return;
        }
        LevelItem nextLevel = gm.levelList[index];
        SceneManager.LoadScene(nextLevel.sceneName);
    }


    /// <summary>
    /// Safely unsubscribes from <see cref="LevelManager" /> events.
    /// Go back to the main menu scene
    /// </summary>
    public void GoToMainMenu()
    {
        SafelyUnsubscribe();
        SceneManager.LoadScene("LevelSelect");
    }

    /// <summary>
    /// Safely unsubscribes from <see cref="LevelManager" /> events.
    /// Reloads the active scene
    /// </summary>
    public void RestartLevel()
    {
        SafelyUnsubscribe();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }



    /// <summary>
    /// Shows the end game screen
    /// </summary>
    protected void OpenEndGameScreen()
    {
        LevelItem level = GateGameManager.instance.GetLevelForCurrentScene();

        if (level != null)
        {
            GateGameManager.instance.CompleteLevel(level.id);
        }

        if (!GameUI.instanceExists)
        {
            return;
        }
        if (GameUI.instance.state == GameUI.State.Building)
        {
            GameUI.instance.CancelGhostPlacement();
        }
        GameUI.instance.GameOver();
    }


    /// <summary>
    /// Ensure that <see cref="LevelManager" /> events are unsubscribed from when necessary
    /// </summary>
    public void SafelyUnsubscribe()
    {
        LazyLoad();
        m_LevelManager.levelCompleted -= Victory;
        m_LevelManager.levelFailed -= Defeat;
    }

    /// <summary>
    /// Ensure <see cref="m_LevelManager" /> is not null
    /// </summary>
    protected void LazyLoad()
    {
        if ((m_LevelManager == null) && GateGame.Level.LevelManager.instanceExists)
        {
            m_LevelManager = GateGame.Level.LevelManager.instance;
        }
    }

    /// <summary>
    /// Occurs when the level is sucessfully completed
    /// </summary>
    protected void Victory()
    {

        OpenEndGameScreen();
        //first check if there are any more levels after this one
        if ( !GateGameManager.instanceExists)
        {
            return;
        }

        GateGameManager gm = GateGameManager.instance;
        LevelItem item = gm.GetLevelForCurrentScene();
        LevelList list = gm.levelList;
        int levelCount = list.Count;
        int index = -1;
        for (int i = 0; i < levelCount; i++)
        {
            if (item == list[i])
            {
                index = i;
                break;
            }
        }
        //if the level does not exist or this is the last level
        //hide the next level button
        if (index < 0 || index == levelCount - 1)
        {
            gameWinWindow1.Show();
        }
        else
        {
            gameWinWindow.Show();
        }

    }


    /// <summary>
    /// Occurs when level is failed
    /// </summary>
    protected void Defeat()
    {
        gameLoseWindow.Show();
        OpenEndGameScreen();
    }


    /// <summary>
    /// Safely unsubscribes from <see cref="LevelManager" /> events.
    /// </summary>
    protected void OnDestroy()
    {
        SafelyUnsubscribe();
        if (GameUI.instanceExists)
        {
            GameUI.instance.Unpause();
        }
    }

}
