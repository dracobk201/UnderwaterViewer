using UnityEngine;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject normalSetupPanel = default;
    [SerializeField] private GameObject cameraSetupPanel = default;

    public void ChangeSetupPanel (string targetPanel)
    {
        if (targetPanel.Equals(Global.CameraActionMap))
        {
            normalSetupPanel.SetActive(false);
            cameraSetupPanel.SetActive(true);
        }
        else if (targetPanel.Equals(Global.UnderwaterActionMap))
        {
            normalSetupPanel.SetActive(true);
            cameraSetupPanel.SetActive(false);
        }
    }
}
