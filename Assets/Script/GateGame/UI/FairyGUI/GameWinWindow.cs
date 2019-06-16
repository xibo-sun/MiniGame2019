using FairyGUI;

public class GameWinWindow : Window
{

    GameOverMenu _gameOverMenu;

    public GameWinWindow(GameOverMenu gameOverMenu)
    {
        _gameOverMenu = gameOverMenu;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("MainGameUI", "GameWinWindow").asCom;
        // next level button
        contentPane.GetChild("n0").onClick.Add(() => {
            GRoot.inst.CloseAllWindows();
            _gameOverMenu.GoToNextLevel();
        });

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
