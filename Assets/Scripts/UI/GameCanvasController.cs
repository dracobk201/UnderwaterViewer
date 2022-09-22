using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvasGroup = default;
    [Header("Data")]
    [SerializeField] private GameEvent showPauseMenu = default;

    [SerializeField] private GameObject normalSetupPanel = default;
    [SerializeField] private GameObject cameraSetupPanel = default;
    [SerializeField] private Image lastPhotoTakenImage = default;

    public void ChangeSetupPanel(string targetPanel)
    {
        if (targetPanel.Equals(Utils.CameraActionMap))
        {
            normalSetupPanel.SetActive(false);
            cameraSetupPanel.SetActive(true);
        }
        else if (targetPanel.Equals(Utils.UnderwaterActionMap))
        {
            normalSetupPanel.SetActive(true);
            cameraSetupPanel.SetActive(false);
        }
    }

    public void PauseGame()
    {
        showPauseMenu.Raise();
    }

    public void ShowPanel()
    {
        Utils.ShowPanel(panelCanvasGroup, true);
    }

    public void HidePanel()
    {
        Utils.ShowPanel(panelCanvasGroup, false);
    }

    public void ShowScreenshot(string base64Texture)
    {
        Texture2D lastPhotoTakenTexture = new Texture2D(1, 1);
        lastPhotoTakenTexture.LoadImage(Convert.FromBase64String(base64Texture));
        lastPhotoTakenTexture.Apply();
        StartCoroutine(ShowScreenshotCoroutine(lastPhotoTakenTexture));
    }

    private IEnumerator ShowScreenshotCoroutine(Texture2D lastPhotoTakenTexture)
    {
        lastPhotoTakenImage.gameObject.SetActive(true);
        //lastPhotoTakenImage.material.mainTexture = lastPhotoTakenTexture;
        Rect rect = new Rect(0, 0, lastPhotoTakenTexture.width, lastPhotoTakenTexture.height);
        lastPhotoTakenImage.sprite = Sprite.Create(lastPhotoTakenTexture, rect, new Vector2(0.5f, 0.5f), 100);
        yield return new WaitForSeconds(1);
        lastPhotoTakenImage.gameObject.SetActive(false);
    }
         
}
