using UnityEngine;
using UnityEngine.UI;

public class WinLoseMenuUI : MonoBehaviour
{
   [SerializeField] private Button playAgainButton;
   [SerializeField] private Button exitToMainMenuButton;

   private void Start()
   {
      playAgainButton.onClick.AddListener(() =>
      {
         SceneLoader.Load(SceneLoader.Scenes.MainGameScene);
      });
      
      exitToMainMenuButton.onClick.AddListener(() =>
      {
         SceneLoader.Load(SceneLoader.Scenes.MainMenu);
      });
   }
}
