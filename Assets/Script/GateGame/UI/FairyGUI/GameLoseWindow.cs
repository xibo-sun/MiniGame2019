using FairyGUI;

public class GameLoseWindow : Window
{


    GameOverMenu _gameOverMenu;

    public GameLoseWindow(GameOverMenu gameOverMenu)
    {
        _gameOverMenu = gameOverMenu;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("MainGameUI", "GameLoseWindow").asCom;

        // restart button
        contentPane.GetChild("n1").onClick.Add(() => {
            GRoot.inst.CloseAllWindows();
            _gameOverMenu.RestartLevel();
        });

        // mainMenu button
        contentPane.GetChild("n2").onClick.Add(() => {
            GRoot.inst.CloseAllWindows();
            _gameOverMenu.GoToMainMenu();
        });
    }
}
