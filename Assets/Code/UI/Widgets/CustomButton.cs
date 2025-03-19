using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class CustomButton : Button
    {
        [SerializeField] private RectTransform _transformToMove;
        [SerializeField] private float _normalY;
        [SerializeField] private float _lowY;

        private Vector3 _normalPosition;
        private Vector3 _lowPosition;

        protected override void Awake()
        {
            base.Awake();
            _normalPosition = new Vector3(0, _normalY);
            _lowPosition = new Vector3(0, _lowY);
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (state == SelectionState.Pressed || state == SelectionState.Disabled)
            {
                _transformToMove.anchoredPosition = _lowPosition;
                return;
            }

            _transformToMove.anchoredPosition = _normalPosition;
        }
    }
}
