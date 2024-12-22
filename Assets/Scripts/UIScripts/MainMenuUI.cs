using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button audioSettings;
    [SerializeField] private Button exitButton;


    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Load(SceneLoader.Scenes.MainGameScene);
        } );
        
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
