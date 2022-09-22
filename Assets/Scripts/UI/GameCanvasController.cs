using UnityEngine;
using ScriptableObjectArchitecture;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvasGroup = default;
    [Header("Data")]
    [SerializeField] private GameEvent showPauseMenu = default;

    [SerializeField] private GameObject normalSetupPanel = default;
    [SerializeField] private GameObject cameraSetupPanel = default;

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
}
