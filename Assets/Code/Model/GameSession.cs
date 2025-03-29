using PixelCrew.Model.Data.Inventory;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _save;
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        public QuickInventoryData QuickInventory { get; private set; }

        void Awake()
        {
            LoadHUD();
            
            if(IsSessionExists())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                LoadModel();
                DontDestroyOnLoad(this);
            }
        }

        private void LoadModel()
        {
            QuickInventory = new QuickInventoryData(_data);
        }

        private void LoadHUD()
        {
            SceneManager.LoadScene(ScenesNames.HUD.ToString(), LoadSceneMode.Additive);
        }

        private bool IsSessionExists()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                    return true;
            }

            return false;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
        }
    }
}

