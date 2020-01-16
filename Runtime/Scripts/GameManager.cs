using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameState GetState()     { return m_currentGameState; }
    public bool IsPaused()          { return GameState.PAUSED == m_currentGameState; }
    public int GetCurrentLevel()    { return m_currentLevel; }

//---------------------------------------------------------------------------------------------------------------------

    GameManager() {
        m_loadOperations = new List<AsyncOperation>();
    }

//---------------------------------------------------------------------------------------------------------------------

    private void Start() {
        DontDestroyOnLoad(gameObject);
        InstantiateSystemPrefabs();
    }

//---------------------------------------------------------------------------------------------------------------------
    public void PauseGame(bool pause) {

        if (GameState.PREGAME == m_currentGameState)
            return;

        //This will check if we already have the same state
        if ((GameState.PAUSED == m_currentGameState) == pause) {
            Debug.Log("early return. Pause: " + pause + " Flag: " + (GameState.PAUSED == m_currentGameState).ToString());
            return;
        }

        //[TODO-sin: 2019-10-9] We can probably make this into an interface too
        UpdateState((pause) ? GameState.PAUSED : GameState.RUNNING);
    }

//---------------------------------------------------------------------------------------------------------------------
    public void RestartGame() {
        UpdateState(GameState.PREGAME);
    }

//---------------------------------------------------------------------------------------------------------------------

    public void QuitGame() {

        Application.Quit();
    }

//---------------------------------------------------------------------------------------------------------------------

    public void LoadLevelAsync(int level)    {
        if (level < 0) {
            Debug.LogError("[GameManager] Invalid level: " + level);
            return;
        }

        AsyncOperation ao = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        if (null == ao) {
            Debug.LogError("[GameManager] Unable to load level: " + level);
            return;
        }

        m_currentLevel = level;
        ao.completed += OnLoadCompleted;
        m_loadOperations.Add(ao);
    }

//---------------------------------------------------------------------------------------------------------------------


    public void UnloadLevelAsync(int level)  {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(level);
        ao.completed += OnUnloadCompleted;

    }

//---------------------------------------------------------------------------------------------------------------------

    void OnLoadCompleted(AsyncOperation ao) {
        if (!m_loadOperations.Contains(ao)) {
            Debug.LogError("[GameManager]Load Completed");
            return;
        }

        Debug.Log("Load Completed");
        m_loadOperations.Remove(ao);

        //[TODO-sin: 2019-10-8] We should make this more general perhaps 
        //- by having a callback
        //- by using an interface for the states
        if (m_loadOperations.Count == 0) {
            int mainGameSceneIndex = SceneUtility.GetBuildIndexByScenePath(Constants.MAIN_GAME_SCENE);
            if (m_currentLevel == mainGameSceneIndex) {
                UpdateState(GameState.RUNNING);
            }
            //[TODO-sin: 2019-10-8] The last added scene is regarded as the active one. We should probably specify
            //that in a parameter when calling LoadScne
            SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount-1));

        }
    }

//---------------------------------------------------------------------------------------------------------------------
    void UpdateState(GameState newState) {
        if (m_currentGameState==newState)
            return;

        //[TODO-sin: 2019-10-8] Can we make this game state handling into an interface ?
        switch (newState) {
            case GameState.PREGAME: {
                Time.timeScale = 1.0f;
                break;
            }
            case GameState.RUNNING: {
                Time.timeScale = 1.0f;
                break;
            }
            case GameState.PAUSED: {
                //[TODO-sin: 2019-10-9] I think this will also stop UI animation. Need to check/do something else
                Time.timeScale = 0.0f;
                break;  
            }
            default: break;
        }

        //Dispatch events
        GameState prevState = m_currentGameState;
        m_currentGameState = newState;
        OnGameStateChangedEvent.Invoke(newState, prevState); 

    }

//---------------------------------------------------------------------------------------------------------------------

    void OnUnloadCompleted(AsyncOperation ao) {
        Debug.Log("Unload Completed");
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount-1));
}


//---------------------------------------------------------------------------------------------------------------------
    protected override void OnDestroyInternalV() {
        foreach(GameObject go in m_systemGameObjects) {
            Destroy(go);
        }
        m_systemGameObjects.Clear();
    }

//---------------------------------------------------------------------------------------------------------------------
    void InstantiateSystemPrefabs() {
        Assert.IsNull(m_systemGameObjects);
        m_systemGameObjects = new List<GameObject>();
        foreach(GameObject prefab in m_systemPrefabs) {
            GameObject go = Instantiate(prefab);
            m_systemGameObjects.Add(go);
        }
    }

//---------------------------------------------------------------------------------------------------------------------
    [SerializeField] GameObject[] m_systemPrefabs = null;
    List <GameObject> m_systemGameObjects = null;
    List<AsyncOperation> m_loadOperations;

    int m_currentLevel;

    GameState m_currentGameState = GameState.PREGAME;
    public Events.GameStateChanged OnGameStateChangedEvent;

//---------------------------------------------------------------------------------------------------------------------

}


//Requirements:
//- What level the game is currently in
//- Methods to load and unload game level
//- Keep track of the game state
//- Generate other persistent systems
//- Pause simulation when in pause state

