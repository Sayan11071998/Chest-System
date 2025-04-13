using UnityEngine;

namespace ChestSystem.Utilities
{
    public class GenericMonoSingleton<T> : MonoBehaviour where T : GenericMonoSingleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var instances = Object.FindObjectsByType<T>(FindObjectsSortMode.None);

                    if (instances.Length > 0)
                    {
                        instance = instances[0];

                        if (instances.Length > 1)
                        {
                            Debug.LogWarning($"Multiple instances of {typeof(T).Name} found. Using the first one.");

                            for (int i = 1; i < instances.Length; i++)
                                Object.Destroy(instances[i].gameObject);
                        }
                    }
                    else
                    {
                        Debug.LogError($"No instance of {typeof(T).Name} found in the scene!");
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else if (instance != this)
            {
                Debug.LogWarning($"Multiple instances of {typeof(T).Name} found! Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}