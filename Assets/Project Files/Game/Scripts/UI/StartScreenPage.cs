using UnityEngine;

namespace Watermelon
{
    public class StartScreenPage : UIPage
    {
        public float displayTime = 3f; // Time before transitioning

        private void Start()
        {
            Invoke(nameof(ProceedToMainMenu), displayTime);
        }

        private void ProceedToMainMenu()
        {
            UIController.HidePage<StartScreenPage>();
            GameController.gameController.StartGame();
        }

        public override void Init()
        {
            // Initialize any necessary elements
        }

        public override void PlayShowAnimation()
        {
            // Implement animation logic when the screen appears (if needed)
            EnableCanvas();
        }

        public override void PlayHideAnimation()
        {
            // Implement animation logic when the screen disappears (if needed)
            // UIController.HidePage<StartScreenPage>();
            // UIController.ShowPage<UIMainMenu>();
            DisableCanvas();

            UIController.OnPageClosed(this);
        }
    }
}
