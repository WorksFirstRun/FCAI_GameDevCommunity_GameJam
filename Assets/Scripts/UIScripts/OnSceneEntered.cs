using System;
using UnityEngine;


public class OnSceneEntered : MonoBehaviour
{
   private bool oneFrame;
   [SerializeField] private ClipName soundTypeToPlay;
   [SerializeField] private bool disableOrEnalbeMusic;
   [SerializeField] private bool playSoundClip;
   
   private void Update()
   {
      if (!oneFrame)
      {
         oneFrame = true;
         if (disableOrEnalbeMusic)
         {
            MusicPlayer.ActivateTheMusic();
         }
         else
         {
            MusicPlayer.DisableTheMusic();
         }
         if (playSoundClip)
         {
            SoundManager.PlaySound(soundTypeToPlay,Camera.main.transform.position);
         }
      }
   }
}
