using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Shin.Framework {

[ExecuteInEditMode]
public class Boot : MonoBehaviour {

    public IList<string> GetScenes() {  return m_scenes; }
    public int GetSelectedScene() { return m_selectedScene;  }

//----------------------------------------------------------------------------------------------------------------------
    public void SetSelectedScene(int sceneIndex) {
        if (sceneIndex >= m_scenes.Count) {
            Debug.LogError("[Boot] SetSelectedScene() Invalid scene index: " + sceneIndex.ToString());
            return;
        }
        m_selectedScene = sceneIndex;
    }

//----------------------------------------------------------------------------------------------------------------------

    private void OnEnable() {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        m_scenes = new List<string>(sceneCount);
        for (int i =0;i<sceneCount;++i) {
            m_scenes.Add(SceneUtility.GetScenePathByBuildIndex(i));
        }
        if (sceneCount > 0 && m_selectedScene < 0) {
            m_selectedScene = 0;
        }
    }



//----------------------------------------------------------------------------------------------------------------------

    private void OnDisable() {
#if UNITY_EDITOR
        if (EditorApplication.isCompiling) {
            m_selectedScene = -1;
            m_scenes.Clear();
        }
#endif
        
    }



//----------------------------------------------------------------------------------------------------------------------
    private void Start() {
        if (!Application.isPlaying) {
            return;
        }

        GameManager.GetInstance().LoadLevelAsync(m_selectedScene);
    }


//----------------------------------------------------------------------------------------------------------------------
    [SerializeField] int m_selectedScene = -1;

    List<string> m_scenes;
}

} //end namespace
