using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Chest
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Image chestImage;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button chestButton;

        private ChestScriptableObject chestData;
        private ChestType chestType;

        public void Initialize(ChestScriptableObject chestData)
        {
            this.chestData = chestData;
            this.chestType = chestData.chestType;

            statusText.text = "LOCKED";
            chestButton.onClick.AddListener(OnChestClicked);
        }

        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {chestType}");
        }

        public void OnReturnToPool()
        {
            chestButton.onClick.RemoveAllListeners();
        }
    }
}