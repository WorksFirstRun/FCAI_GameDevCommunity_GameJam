using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gamePause;
    public event Action OnShowPauseUi;
    public event Action OnHideAnyOtherUi;
    
    [SerializeField] private Player playerLogicReference;
    [SerializeField] private BossHealth bossHealthReference;
    private bool win, lose;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.onEscapeAction += TogglePauseGame;
    }

    public void TogglePauseGame() // some buttons use it I can't make it private
    {
        if (lose || win) return;
        
        gamePause = !gamePause;
        if (gamePause)
        {
            Time.timeScale = 0f;
            OnHideAnyOtherUi?.Invoke();
            OnShowPauseUi?.Invoke();
            if (playerLogicReference.gameObject !=  null)
            {
                playerLogicReference.DisablePlayerBehaviour();
            }
        }
        else
        {
            Time.timeScale = 1f;
            OnHideAnyOtherUi?.Invoke();
            if (playerLogicReference.gameObject !=  null)
            {
                playerLogicReference.EnableThePlayerBehaviour();
            }
        }

    }
    

    /*public void UnPauseGame()
    {
        if (gamePause)
        {
            gamePause = false;
        }
        Time.timeScale = 1f;
        onHidePauseUi?.Invoke();
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.EnableStateManager();
        }
    }*/

    /*public void PauseGame() // idk if i need it or not 
    {
        if (!gamePause)
        {
            gamePause = true;
        }
        Time.timeScale = 0f;
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.DisableStateManager();
        }
    }*/

    public void DeclarePlayerLost()
    {
        lose = true;
        Time.timeScale = 0;
        OnHideAnyOtherUi?.Invoke();
        InputManager.Instance.DisableInput();
        Time.timeScale = 1; // have to set the timescale to 1 in order for transitions to work
        SceneLoader.Load(SceneLoader.Scenes.LoseScene);
    }

    public void DeclarePlayerWon()
    {
        win = true;
        Time.timeScale = 0;
        OnHideAnyOtherUi?.Invoke();
        InputManager.Instance.DisableInput();
        Time.timeScale = 1; // have to set the timescale to 1 in order for transitions to work
        SceneLoader.Load(SceneLoader.Scenes.WinScene);
    }


    private void OnDestroy()
    {
        OnShowPauseUi = null;
        OnHideAnyOtherUi = null;
    }
}