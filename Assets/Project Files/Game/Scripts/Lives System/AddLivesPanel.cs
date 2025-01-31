using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class AddLivesPanel : MonoBehaviour, IPopupWindow
    {
        [SerializeField] RectTransform panel;
        [SerializeField] Vector3 hidePos;

        [SerializeField] Image backgroundImage;

        [SerializeField] Button button;
        [SerializeField] Button closeButton;

        [SerializeField] TMP_Text livesAmountText;
        [SerializeField] TMP_Text timeText;
        [SerializeField] AudioClip lifeRecievedAudio;

        private Vector3 showPos;
        private Color backColor;

        private static int openedStack;
        public static bool IsPanelOpened => openedStack != 0;

        public bool IsOpened => gameObject.activeSelf;

        public SimpleBoolCallback OnPanelClosedCallback;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            backColor = backgroundImage.color;
            showPos = panel.anchoredPosition;

            button.onClick.AddListener(OnButtonClick);
            closeButton.onClick.AddListener(Hide);
        }

        private void OnEnable()
        {
            LivesManager.AddPanel(this);
        }

        private void OnDisable()
        {
            LivesManager.RemovePanel(this);
        }

        public void Show(SimpleBoolCallback onPanelClosed = null)
        {
            gameObject.SetActive(true);

            backgroundImage.color = Color.clear;
            backgroundImage.DOColor(backColor, 0.3f);

            panel.anchoredPosition = hidePos;
            panel.DOAnchoredPosition(showPos, 0.3f).SetEasing(Ease.Type.SineOut);

            openedStack++;

            OnPanelClosedCallback = onPanelClosed;

            UIController.OnPopupWindowOpened(this);
        }

        public void Hide()
        {
            backgroundImage.DOColor(Color.clear, 0.3f);
            panel.DOAnchoredPosition(hidePos, 0.3f).SetEasing(Ease.Type.SineIn).OnComplete(() => gameObject.SetActive(false));

            openedStack--;

            OnPanelClosedCallback?.Invoke(false);

            UIController.OnPopupWindowClosed(this);
        }

        public void OnButtonClick()
        {
            AdsManager.ShowRewardBasedVideo(success =>
            {
                if (success)
                {
                    LivesManager.AddLife();

                    if (lifeRecievedAudio != null)
                        AudioController.PlaySound(lifeRecievedAudio);

                    OnPanelClosedCallback?.Invoke(true);
                    OnPanelClosedCallback = null;
                }

                Hide();
            });
        }

        public void SetLivesCount(int count)
        {
            livesAmountText.text = count.ToString();
        }

        public void SetTime(string time)
        {
            timeText.text = time;
        }
    }
}