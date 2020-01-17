using UnityEngine;

namespace Shin.Framework {

//Requires that T extends SingletonBehaviour
public class SingletonBehaviour<T> : MonoBehaviour where T: SingletonBehaviour<T>
{
    public static T GetInstance() { return m_instance; }
    public static bool IsInitialized() { return null != m_instance; }

//---------------------------------------------------------------------------------------------------------------------

    protected virtual void Awake() {
        if (null != m_instance) {
            Debug.LogError("[SingletonBehaviour] Duplicate Singleton: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        m_instance = this as T;
    }

//---------------------------------------------------------------------------------------------------------------------

    private void OnDestroy() {
        OnDestroyInternalV();
        if (this == m_instance) {
            m_instance = null;
        }
    }

//---------------------------------------------------------------------------------------------------------------------
    protected virtual void OnDestroyInternalV() {
    }
//---------------------------------------------------------------------------------------------------------------------

    private static T m_instance;

}

} //end namespace
