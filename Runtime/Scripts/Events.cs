using System;   //Serializable
using UnityEngine.Events;

namespace ShinFramework {

public static class Events {
    //Fadeout: true. Fadein: false
    [Serializable] public class MenuFadeComplete : UnityEvent<bool> { }

    [Serializable] public class GameStateChanged : UnityEvent<GameState, GameState> { }
}

} //end namespace
