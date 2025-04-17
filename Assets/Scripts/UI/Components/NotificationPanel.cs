using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using ChestSystem.Events;
using ChestSystem.Core;

namespace ChestSystem.UI.Components
{
    public class NotificationPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI buttonText;

        [Header("Undo Button")]
        [SerializeField] private Button undoButton;
        [SerializeField] private TextMeshProUGUI undoButtonText;
        [SerializeField] private GameObject undoButtonContainer;

        [Header("Animation Settings")]
        [SerializeField] private float fadeInDuration = 0.3f;
        [SerializeField] private float fadeOutDuration = 0.2f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private CanvasGroup canvasGroup;
        private Coroutine fadeCoroutine;
        private RectTransform rectTransform;

        public static event Action OnNotificationClosed;
        public static event Action OnUndoButtonClicked;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            button.onClick.AddListener(CloseNotification);

            if (undoButton != null)
                undoButton.onClick.AddListener(HandleUndoClicked);

            if (undoButtonContainer != null)
                undoButtonContainer.SetActive(false);

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            rectTransform.localScale = Vector3.zero;
        }

        public void ShowNotification(string title, string message, string buttonText)
        {
            titleText.text = title.ToUpper();
            messageText.text = message;
            this.buttonText.text = buttonText;

            if (undoButtonContainer != null)
                undoButtonContainer.SetActive(false);

            gameObject.SetActive(true);

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeIn());
            EventService.Instance.OnNotificationShow.InvokeEvent();
        }

        public void ShowNotificationWithUndo(string title, string message, string buttonText, string undoButtonText)
        {
            titleText.text = title.ToUpper();
            messageText.text = message;
            this.buttonText.text = buttonText;

            if (undoButtonContainer != null)
            {
                undoButtonContainer.SetActive(true);
                this.undoButtonText.text = undoButtonText;
            }

            gameObject.SetActive(true);

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeIn());
            EventService.Instance.OnNotificationShow.InvokeEvent();
        }

        public void CloseNotification()
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOut());

            OnNotificationClosed?.Invoke();
            EventService.Instance.OnNotificationClose.InvokeEvent();
        }

        private void HandleUndoClicked()
        {
            OnUndoButtonClicked?.Invoke();

            if (GameService.Instance != null && GameService.Instance.commandInvoker != null)
                GameService.Instance.commandInvoker.Undo();

            CloseNotification();
        }

        private IEnumerator FadeIn()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            rectTransform.localScale = Vector3.zero;

            float elapsedTime = 0;
            while (elapsedTime < fadeInDuration)
            {
                float normalizedTime = elapsedTime / fadeInDuration;
                canvasGroup.alpha = animationCurve.Evaluate(normalizedTime);

                float scaleValue = animationCurve.Evaluate(normalizedTime);
                rectTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1;
            rectTransform.localScale = Vector3.one;
        }

        private IEnumerator FadeOut()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float elapsedTime = 0;
            float initialScale = rectTransform.localScale.x;

            while (elapsedTime < fadeOutDuration)
            {
                float normalizedTime = elapsedTime / fadeOutDuration;
                canvasGroup.alpha = 1 - animationCurve.Evaluate(normalizedTime);

                float scaleValue = Mathf.Lerp(initialScale, 0, animationCurve.Evaluate(normalizedTime));
                rectTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 0;
            rectTransform.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(CloseNotification);
            if (undoButton != null)
                undoButton.onClick.RemoveListener(HandleUndoClicked);
        }
    }
}