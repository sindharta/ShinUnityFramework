using UnityEngine;

public class GameBehaviour : MonoBehaviour {

    protected virtual void Awake() {
        m_transform = transform;
    }
    
//---------------------------------------------------------------------------------------------------------------------
    protected Transform m_transform = null;
}
