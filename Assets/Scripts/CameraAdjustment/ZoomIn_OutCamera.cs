using Cinemachine;
using UnityEngine;

public class ZoomIn_OutCamera : MonoBehaviour
{
   [SerializeField] private CinemachineVirtualCamera zoomOut;
   [SerializeField] private CinemachineVirtualCamera zoomInCamera;
   [SerializeField] private Texture2D aimCursor;
   
   public void ZoomIn()
   {
      if (zoomInCamera.gameObject.activeSelf) return;
      zoomInCamera.gameObject.SetActive(true);
      Vector2 hotspot = new Vector2(aimCursor.width / 2.0f, aimCursor.height / 2.0f);
      Cursor.SetCursor(aimCursor,hotspot,CursorMode.Auto);
   }

   public void ZoomOut()
   {
      if (!zoomInCamera.gameObject.activeSelf) return;
      zoomInCamera.gameObject.SetActive(false);
      Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
   }
   
}
