using FairyGUI;

public class GameWinWindow1 : Window
{

    GameOverMenu _gameOverMenu;

    public GameWinWindow1(GameOverMenu gameOverMenu)
    {
        _gameOverMenu = gameOverMenu;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("MainGameUI", "GameWinWindow1").asCom;

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