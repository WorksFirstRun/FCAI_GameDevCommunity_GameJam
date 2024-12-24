using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button audioSettings;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private AudioSettingsUI _audioSettingsUI;
    [SerializeField] private HowToPlayMenuScript _howToPlayMenuScript;
    
    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scenes.MainGameScene);
        } );
        
        howToPlayButton.onClick.AddListener(_howToPlayMenuScript.Show);
        
        audioSettings.onClick.AddListener(_audioSettingsUI.Show);
        
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
