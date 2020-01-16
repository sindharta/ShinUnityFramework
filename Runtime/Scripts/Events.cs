using System;   //Serializable
using UnityEngine.Events;

public static class Events {
    //Fadeout: true. Fadein: false
    [Serializable] public class MenuFadeComplete : UnityEvent<bool> { }

    [Serializable] public class GameStateChanged : UnityEvent<GameState, GameState> { }
}
