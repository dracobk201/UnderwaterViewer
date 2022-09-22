using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerControls playerControl;

    private void Awake()
    {
        playerControl = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControl.Enable();
        SwitchActionMap(Utils.UnderwaterActionMap);
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    public void SwitchActionMap(string mapName)
    {
        if (mapName.Equals(Utils.CameraActionMap))
        {
            playerControl.Underwater.Enable();
            playerControl.Camera.Disable();
        }
        else if (mapName.Equals(Utils.UnderwaterActionMap))
        {
            playerControl.Camera.Enable();
            playerControl.Underwater.Disable();
        }
    }
}
