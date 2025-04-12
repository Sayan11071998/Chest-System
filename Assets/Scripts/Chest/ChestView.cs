using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Chest
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Image chestImage;
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Button chestButton;

        [SerializeField] private GameObject lockIcon;
        [SerializeField] private GameObject unlockingIcon;
        [SerializeField] private GameObject readyToCollectIcon;

        private ChestModel chestModel;
        private Action<ChestView> onChestClicked;

        private void Awake()
        {
            if (chestButton != null)
                chestButton.onClick.AddListener(OnChestButtonClicked);
        }

        public void Initialize(ChestModel model, Action<ChestView> clickCallback)
        {
            chestModel = model;
            onChestClicked = clickCallback;

            if (chestImage != null && model.ChestScriptableObject.chestSprite != null)
                chestImage.sprite = model.ChestScriptableObject.chestSprite;

            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            if (chestModel == null) return;

            if (timerText != null)
            {
                if (chestModel.CurrentState == ChestState.UNLOCKING)
                {
                    int minutes = Mathf.FloorToInt(chestModel.RemainingTimeInSeconds / 60);
                    int seconds = Mathf.FloorToInt(chestModel.RemainingTimeInSeconds % 60);
                    timerText.text = $"{minutes:00}:{seconds:00}";
                    timerText.gameObject.SetActive(true);
                }
                else
                {
                    timerText.gameObject.SetActive(false);
                }
            }

            if (stateText != null)
            {
                stateText.text = chestModel.CurrentState.ToString();
            }

            if (lockIcon != null)
                lockIcon.SetActive(chestModel.CurrentState == ChestState.LOCKED);

            if (unlockingIcon != null)
                unlockingIcon.SetActive(chestModel.CurrentState == ChestState.UNLOCKING);

            if (readyToCollectIcon != null)
                readyToCollectIcon.SetActive(chestModel.CurrentState == ChestState.UNLOCKED);
        }

        private void OnChestButtonClicked() => onChestClicked?.Invoke(this);

        public ChestModel GetChestModel() => chestModel;

        public void SetActive(bool active) => gameObject.SetActive(active);

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}