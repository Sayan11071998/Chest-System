using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.UI.Components
{
    public class EmptySlotView : MonoBehaviour
    {
        [SerializeField] private Image slotBackground;

        public void Initialize()
        {
            slotBackground.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
}