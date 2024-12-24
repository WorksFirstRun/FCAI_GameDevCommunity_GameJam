using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
   [SerializeField] private Button musicVolume;
   [SerializeField] private Button sfxVolume;
   [SerializeField] private Button exitMenu;
   [SerializeField] private TextMeshProUGUI musicVolumeText;
   [SerializeField] private TextMeshProUGUI sfxVolumeText;
   [SerializeField] private GameManager _gameManager;
   
   private void Start()
   {
      UpdateMusicText();
      UpdateSoundText();
      
      musicVolume.onClick.AddListener(() =>
      {
         MusicPlayer.ChangeVolume();
         UpdateMusicText();
      });
      
      sfxVolume.onClick.AddListener(() =>
      {
         SoundManager.ChangeVolume();
         sfxVolumeText.text = Mathf.Round(SoundManager.GetVolume() * 10f).ToString();
         UpdateSoundText();
      });
      
      exitMenu.onClick.AddListener(Hide);

      if (_gameManager != null)
      {
         _gameManager.OnHideAnyOtherUi += Hide;
      }
      
      Hide();
   }

   public void Show()
   {
      gameObject.SetActive(true);
   }

   public void Hide()
   {
      gameObject.SetActive(false);
   }

   private void UpdateMusicText()
   {
      musicVolumeText.text = "Music Volume \n" + Mathf.Round(MusicPlayer.GetVolume() * 10f);
   }

   private void UpdateSoundText()
   {
      sfxVolumeText.text = "SFX Volume \n" + Mathf.Round(SoundManager.GetVolume() * 10f);
   }
   
   
   
}
