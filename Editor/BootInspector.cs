using System.Collections.Generic;
using UnityEditor;

namespace Shin.Framework {
[CustomEditor(typeof(Boot))]
public class BootInspector : Editor {
    
    void OnEnable() {
        m_boot = target as Boot;

        //Populating the dropdown menu. Replace "/" because it is used for creating submenus.
        Refresh();
    }
   

//----------------------------------------------------------------------------------------------------------------------
    public override void OnInspectorGUI()   {
        IList<string> scenes = m_boot.GetScenes();
        if (null == scenes || m_boot.GetScenes().Count<=0 || null==m_sceneNames) {
            Refresh();
            return;
        }

        m_boot.SetSelectedScene(EditorGUILayout.Popup("Boot Scene: ", m_boot.GetSelectedScene(), m_sceneNames));
        EditorUtility.SetDirty(m_boot);

    }

//----------------------------------------------------------------------------------------------------------------------

    private void Refresh() {
        IList<string> scenes = m_boot.GetScenes();
        if (null == scenes)
            return;

        int sceneCount = scenes.Count;
        if (sceneCount <=0)
            return;

        m_sceneNames = new string[sceneCount];
        for (int i=0;i<sceneCount;++i) {
            m_sceneNames[i] = scenes[i].Replace("/","\\");
        }
    }

//----------------------------------------------------------------------------------------------------------------------

    string[] m_sceneNames = null;
    Boot m_boot = null;

}

} //end namespace