using ChestSystem.Main;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.UI.Core
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private Button chestGenerationButton;

        private void Start()
        {
            AddListenerToButtons();
        }

        private void AddListenerToButtons()
        {
            chestGenerationButton.onClick.AddListener(GameService.Instance.chestService.GenerateRandomChest);
        }
    }
}