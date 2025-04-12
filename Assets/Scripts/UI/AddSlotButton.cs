using ChestSystem.Main;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.UI
{
    public class AddSlotButton : MonoBehaviour
    {
        [SerializeField] private int slotIncreaseAmount = 1;

        private Button addSlotButton;

        private void Awake() => addSlotButton = GetComponent<Button>();

        private void Start()
        {
            addSlotButton.onClick.AddListener(OnAddSlotButtonClicked);
            transform.SetAsLastSibling();
        }

        private void OnAddSlotButtonClicked()
        {
            GameService.Instance.chestService.IncreaseMaxChestSlots(slotIncreaseAmount);
        }

        private void OnDestroy()
        {
            addSlotButton.onClick.RemoveListener(OnAddSlotButtonClicked);
        }
    }
}