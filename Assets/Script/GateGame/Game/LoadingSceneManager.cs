using UnityEngine;
using UnityEngine.SceneManagement;

namespace GateGame.Game
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public string SceneToLoad;

        public void StartGame()
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }
}