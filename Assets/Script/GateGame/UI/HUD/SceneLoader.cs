using UnityEngine;
using UnityEngine.SceneManagement;


namespace GateGame.UI.HUD
{
    /// <summary>
    /// Scene loader.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// Name of the scene to load
        /// </summary>
        public string sceneToLoadName = "LevelSelect";

        /// <summary>
        /// Loads the scene from <see cref="sceneToLoadName" />
        /// if a scene with that name exists
        /// </summary>
        public void LoadScene()
        {
            SceneManager.LoadScene(sceneToLoadName);
        }

        /// <summary>
        /// Restarts the current scene
        /// </summary>
        public void RestartCurrentScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.name);
        }
    }
}



