using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace ChestSystem.UI.Components
{
    public class NotificationPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button closeButton;

        [Header("Animation Settings")]
        [SerializeField] private float fadeInDuration = 0.3f;
        [SerializeField] private float fadeOutDuration = 0.2f;

        private CanvasGroup canvasGroup;
        private Coroutine fadeCoroutine;

        public static event Action OnNotificationClosed;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            closeButton.onClick.AddListener(CloseNotification);

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void ShowNotification(string title, string message)
        {
            titleText.text = title.ToUpper();
            messageText.text = message;
            gameObject.SetActive(true);

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeIn());
        }

        public void CloseNotification()
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOut());

            OnNotificationClosed?.Invoke();
        }

        private IEnumerator FadeIn()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            float elapsedTime = 0;
            while (elapsedTime < fadeInDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;

            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, fadeInDuration).setEaseOutBack();
        }

        private IEnumerator FadeOut()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float elapsedTime = 0;
            while (elapsedTime < fadeOutDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        private void OnDestroy() => closeButton.onClick.RemoveListener(CloseNotification);
    }
}