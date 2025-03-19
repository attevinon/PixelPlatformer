using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace  PixelCrew.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private FilledBarWidget _healthBar;
        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _gameSession.Data.Health.OnChanged += UpdateHealthBar;
            UpdateHealthBar(_gameSession.Data.Health.Value);
        }

        private void UpdateHealthBar(int currentHp)
        {
            Debug.Log("UpdateHealthBar");
            float hpNormalized = (float)currentHp / DefsFacade.I.PlayerDef.MaxHealth;
            _healthBar.SetAmount(hpNormalized);
        }

        private void OnDestroy()
        {
            _gameSession.Data.Health.OnChanged -= UpdateHealthBar;
        }
    }
}
