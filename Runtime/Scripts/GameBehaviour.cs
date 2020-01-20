using UnityEngine;

namespace Shin.Framework {

[ExecuteInEditMode]
public class GameBehaviour : MonoBehaviour {

    public Transform GetTransform() { return m_transform; }

//----------------------------------------------------------------------------------------------------------------------

    protected virtual void Awake() {
        m_transform = transform;
    }

//----------------------------------------------------------------------------------------------------------------------
    protected Transform m_transform = null;
}

} //end namespace
