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
    //public delegate void GameStateChangeHandler(GameState newGameState);
    //public event GameStateChangeHandler OnGameStateChanged;

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;

        CurrentGameState = newGameState;

        // if (OnGameStateChanged != null)
        //     OnGameStateChanged(newGameState);
        //Statement below is simplified version
        OnGameStateChanged?.Invoke(newGameState);
    }

    //Copy this onto any observer
    /*
    private void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {

    }

    private void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    */
}