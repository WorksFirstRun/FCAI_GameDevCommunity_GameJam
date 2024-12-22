using System;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button AudioSettings;
    [SerializeField] private Button ExitToMainMenu;


    private void Start()
    {
        GameManager.Instance.OnShowPauseUi += ShowPauseMenu;
        GameManager.Instance.OnHideAnyOtherUi += HidePauseMenu;
        ResumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
            
        });
        
        ExitToMainMenu.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneLoader.Load(SceneLoader.Scenes.MainMenu);
        });
        
        HidePauseMenu();
    }


    private void ShowPauseMenu()
    {
        gameObject.SetActive(true);
    }


    private void HidePauseMenu()
    {
        gameObject.SetActive(false);
    }
}

