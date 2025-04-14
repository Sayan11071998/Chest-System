using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ChestSystem.UI.Components
{
    public class EmptySlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image slotFrame;
        [SerializeField] private Image slotSilhouette;

        [Header("Colors")]
        [SerializeField] private Color normalBackgroundColor = new Color(0.831f, 0.706f, 0.557f);
        [SerializeField] private Color hoverBackgroundColor = new Color(0.875f, 0.745f, 0.592f);
        [SerializeField] private Color frameColor = new Color(0.553f, 0.431f, 0.388f);
        [SerializeField] private Color silhouetteColor = new Color(0.361f, 0.251f, 0.149f, 0.3f);

        [Header("Animation")]
        [SerializeField] private float hoverScaleFactor = 1.05f;
        [SerializeField] private float animationSpeed = 5f;

        private Vector3 originalScale;
        private bool isHovering = false;

        private void Awake() => originalScale = transform.localScale;

        public void Initialize()
        {
            if (slotBackground != null)
            {
                slotBackground.color = normalBackgroundColor;

                var shadow = slotBackground.gameObject.GetComponent<Shadow>() ?? slotBackground.gameObject.AddComponent<Shadow>();
                shadow.effectColor = new Color(0, 0, 0, 0.3f);
                shadow.effectDistance = new Vector2(2, 2);
            }

            if (slotFrame != null)
                slotFrame.color = frameColor;

            if (slotSilhouette != null)
                slotSilhouette.color = silhouetteColor;

            StartCoroutine(IdlePulseAnimation());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;

            if (slotBackground != null)
                slotBackground.color = hoverBackgroundColor;

            transform.localScale = originalScale * hoverScaleFactor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;

            if (slotBackground != null)
                slotBackground.color = normalBackgroundColor;

            transform.localScale = originalScale;
        }

        private System.Collections.IEnumerator IdlePulseAnimation()
        {
            float time = 0f;
            float pulseMagnitude = 0.02f;

            while (true)
            {
                if (!isHovering)
                {
                    time += Time.deltaTime;
                    float pulse = 1f + Mathf.Sin(time * animationSpeed) * pulseMagnitude;
                    transform.localScale = originalScale * pulse;
                }

                yield return null;
            }
        }

        public void SetLockedAppearance(bool locked)
        {
            if (locked)
            {
                if (slotBackground != null)
                    slotBackground.color = new Color(0.7f, 0.7f, 0.7f);
            }
            else
            {
                if (slotBackground != null)
                    slotBackground.color = normalBackgroundColor;
            }
        }
    }
}