using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;
using PixelCrew.Utils;

namespace PixelCrew.Components.LevelManegement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private ScenesNames _scene;

        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.Save();

            SceneManager.LoadScene(_scene.ToString());
        }
    }
}

