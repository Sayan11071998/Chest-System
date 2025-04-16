using ChestSystem.UI.Components;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.UI.Core
{
    public class NotificationManager : GenericMonoSingleton<NotificationManager>
    {
        [SerializeField] private NotificationPanel notificationPanelPrefab;
        private NotificationPanel activePanel;

        protected override void Awake()
        {
            base.Awake();
            InitializeNotificationPanel();
        }

        private void InitializeNotificationPanel()
        {
            if (notificationPanelPrefab != null)
            {
                activePanel = Instantiate(notificationPanelPrefab, transform);
                activePanel.gameObject.SetActive(false);
            }
        }

        public void ShowNotification(string title, string message)
        {
            if (activePanel == null) return;

            activePanel.ShowNotification(title, message);
        }
    }
}