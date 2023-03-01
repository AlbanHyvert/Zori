using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Fields
    private static T _instance = null;
    #endregion Fields

    #region Properties
    public static T Instance
    {
        get
        {

            if (_instance == null)
            {
                return _instance = FindObjectOfType<T>();
            }
                    
            if (_instance == null)
            {
                if (_instance != null) return _instance;

                GameObject singletonObject = new GameObject();

                _instance = singletonObject.AddComponent<T>();

                singletonObject.name = typeof(T).ToString() + "(Singleton)";

                DontDestroyOnLoad(singletonObject);
            }
                return _instance;
        }
    }
    #endregion Properties
    
    #region Methods
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    protected virtual void OnDestroy()
    {
        _instance = null;
    }
    #endregion Methods
}
