using System;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayMenuScript : MonoBehaviour
{
    [SerializeField] private Button closeMenu;


    private void Start()
    {
        closeMenu.onClick.AddListener(Hide);
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
