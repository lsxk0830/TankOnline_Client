using UnityEngine;

public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
}