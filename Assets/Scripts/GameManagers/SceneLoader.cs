using MaskTransitions;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    public enum Scenes
    {
        MainGameScene,
        MainMenu,
        WinScene,
        LoseScene
    }


    public static void Load(Scenes sceneName)
    {
       TransitionManager.Instance.LoadLevel(sceneName.ToString());
    }
}
