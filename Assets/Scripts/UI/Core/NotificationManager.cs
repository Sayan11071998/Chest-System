using ChestSystem.UI.Components;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.UI.Core
{
    public class NotificationManager : GenericMonoSingleton<NotificationManager>
    {
        [SerializeField] private NotificationPanel notificationPanelPrefab;
        private NotificationPanel activePanel;

        private void Start()
        {
            if (notificationPanelPrefab != null)
            {
                activePanel = Instantiate(notificationPanelPrefab, transform);
                activePanel.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Notification panel prefab is not assigned in NotificationManager!");
            }
        }

        public void ShowNotification(string title, string message)
        {
            if (activePanel == null)
            {
                Debug.LogError("No notification panel available!");
                return;
            }

            activePanel.ShowNotification(title, message);
        }
    }
}