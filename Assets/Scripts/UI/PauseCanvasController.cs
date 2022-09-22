using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class PauseCanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvasGroup = default;

    [Header("Data")]
    [SerializeField] private BoolReference isGamePaused = default;
    [SerializeField] private GameEvent showGameUI = default;

    [SerializeField] private BoolReference isXMovementInverted = default;
    [SerializeField] private BoolReference isYMovementInverted = default;
    [SerializeField] private BoolReference isXCameraInverted = default;
    [SerializeField] private BoolReference isYCameraInverted = default;
    [SerializeField] private FloatReference bgmVolume = default;
    [SerializeField] private FloatReference sfxVolume = default;
    [SerializeField] private GameEvent volumeUpdated = default;

    public void Continue()
    {
        isGamePaused.Value = false;
        showGameUI.Raise();
    }

    public void OnBGMVolumeChange(float value)
    {
        bgmVolume.Value = value;
        volumeUpdated.Raise();
    }

    public void OnSFXVolumeChange(float value)
    {
        sfxVolume.Value = value;
        volumeUpdated.Raise();
    }

    public void OnInvertedXMovementToggle (bool value)
    {
        isXMovementInverted.Value = value;
    }

    public void OnInvertedYMovementToggle(bool value)
    {
        isYMovementInverted.Value = value;
    }

    public void OnInvertedXCameraToggle(bool value)
    {
        isXCameraInverted.Value = value;
    }

    public void OnInvertedYCameraToggle(bool value)
    {
        isYCameraInverted.Value = value;
    }

    public void ShowPanel()
    {
        Utils.ShowPanel(panelCanvasGroup, true);
        isGamePaused.Value = true;
    }

    public void HidePanel()
    {
        Utils.ShowPanel(panelCanvasGroup, false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
