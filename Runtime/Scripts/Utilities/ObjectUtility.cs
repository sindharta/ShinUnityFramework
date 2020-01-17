using UnityEngine;

namespace Shin.Framework {

    public static class ObjectUtility {
    public static void Destroy(Object obj) {
        if (obj == null) return;

        if (Application.isPlaying)
            UnityEngine.Object.Destroy(obj);
        else
            UnityEngine.Object.DestroyImmediate(obj);

    }
}

} //end namespace
