using UnityEngine;

namespace Jun
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        // public bool IsDontDestroy = true;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        Debug.LogError("======" + typeof(T).ToString() + "====================");
                        Debug.Break();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                //     if (IsDontDestroy)
                //         DontDestroyOnLoad(gameObject);
                // }
                // else
                // {
                //     Destroy(gameObject);
            }
        }
    }
}