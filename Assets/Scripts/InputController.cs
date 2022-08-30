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
        SwitchActionMap(Global.UnderwaterActionMap);
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    public void SwitchActionMap(string mapName)
    {
        if (mapName.Equals(Global.CameraActionMap))
        {
            playerControl.Underwater.Enable();
            playerControl.Camera.Disable();
        }
        else if (mapName.Equals(Global.UnderwaterActionMap))
        {
            playerControl.Camera.Enable();
            playerControl.Underwater.Disable();
        }
    }
}
