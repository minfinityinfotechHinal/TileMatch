using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace Watermelon
{
    public class LoadingGraphics : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI loadingText;
        [SerializeField] Image backgroundImage;
        [SerializeField] CanvasScaler canvasScaler;
        [SerializeField] Camera loadingCamera;
        [SerializeField] StartScreenAnimation startScreenAnimation; // Reference to animation script

        private bool isFadingOut = false; // Prevent multiple fade calls

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            canvasScaler.MatchSize();
            OnLoading(0.0f, "Loading...");
        }

        private void OnEnable()
        {
            GameLoading.OnLoading += OnLoading;
            GameLoading.OnLoadingFinished += OnLoadingFinished;
        }

        private void OnDisable()
        {
            GameLoading.OnLoading -= OnLoading;
            GameLoading.OnLoadingFinished -= OnLoadingFinished;
        }

        private void OnLoading(float progress, string message)
        {
            if (isFadingOut) return; // Prevent updates during fade-out

            //loadingText.text = message;

            // Sync animation with loading progress
            if (startScreenAnimation != null)
            {
                startScreenAnimation.UpdateAnimation(progress);
            }
        }

        private void OnLoadingFinished()
        {
            if (!isFadingOut) // Prevent multiple fade-out calls
            {
                StartCoroutine(FadeOutAndDestroy());
            }
        }

        private IEnumerator FadeOutAndDestroy()
        {
            isFadingOut = true; // Prevents multiple fades

            // Stop animations **before fading**
            if (startScreenAnimation != null)
            {
                startScreenAnimation.StopAnimations();
            }
            if (startScreenAnimation != null)
            {
                Destroy(startScreenAnimation.gameObject);
            }

            // float duration = 0.6f;
            // float elapsed = 0f;
            // Color textColor = loadingText.color;
            // Color bgColor = backgroundImage.color;

            // while (elapsed < duration)
            // {
            //     elapsed += Time.deltaTime;
            //     float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            //     loadingText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            //     backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, alpha);
            //     yield return null;
            // }

            // // Ensure objects are fully transparent before destroying
            // loadingText.color = new Color(textColor.r, textColor.g, textColor.b, 0f);
            // backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f);

            yield return new WaitForSeconds(0.01f); // Small delay to ensure fade-out
            Destroy(gameObject); // **Remove loading screen fully**
        }
    }
}
