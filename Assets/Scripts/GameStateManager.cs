using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    private static GameStateManager instance;
    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameStateManager();
            }
            return instance;
        }
    }

    public GameState CurrentGameState { get; private set; }

    public static event System.Action<GameState> OnGameStateChanged;

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;

        CurrentGameState = newGameState;

        // if (OnGameStateChanged != null)
        //     OnGameStateChanged(newGameState);
        //Statement below is simplified version

        OnGameStateChanged?.Invoke(newGameState);

        Debug.Log($"New State is {newGameState}");
    }
}

//Helpful Hint:
//Copy the text below into any observer
/*

private void OnEnable()
{
    GameStateManager.OnGameStateChanged += OnGameStateChanged;
}

private void OnGameStateChanged()
{
    currentGameState = GameStateManager.Instance.CurrentGameState
}

private void OnDisable()
{
    GameStateManager.OnGameStateChanged -= OnGameStateChanged;
}

*/