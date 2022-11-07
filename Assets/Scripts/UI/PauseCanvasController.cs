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
    [SerializeField] private BoolReference isSwipeDetectionActive = default;
    [SerializeField] private FloatReference bgmVolume = default;
    [SerializeField] private FloatReference sfxVolume = default;
    [SerializeField] private GameEvent volumeUpdated = default;

    [Header("Canvas")]
    [SerializeField] private Slider bgmVolumeSlider = default;
    [SerializeField] private Slider sfxVolumeSlider = default;
    [SerializeField] private Toggle isXMovementInvertedToggle = default;
    [SerializeField] private Toggle isYMovementInvertedToggle = default;
    [SerializeField] private Toggle isXCameraInvertedToggle = default;
    [SerializeField] private Toggle isYCameraInvertedToggle = default;
    [SerializeField] private Toggle isSwipeDetectionActiveToggle = default;

    private void Start()
    {
        bgmVolumeSlider.value = bgmVolume.Value;
        sfxVolumeSlider.value = sfxVolume.Value;
        isXMovementInvertedToggle.isOn = isXMovementInverted.Value;
        isYMovementInvertedToggle.isOn = isYMovementInverted.Value;
        isXCameraInvertedToggle.isOn = isXCameraInverted.Value;
        isYCameraInvertedToggle.isOn = isYCameraInverted.Value;
        isSwipeDetectionActiveToggle.isOn = isSwipeDetectionActive.Value;
    }

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

    public void OnSwipeDetectionToggle(bool value)
    {
        isSwipeDetectionActive.Value = value;
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
