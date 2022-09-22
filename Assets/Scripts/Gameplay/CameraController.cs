using UnityEngine;
using ScriptableObjectArchitecture;

public class CameraController : MonoBehaviour
{
    [SerializeField] private FloatReference cameraRotationSpeed = default;
    [SerializeField] private StringGameEvent changeActionMap = default;
    [SerializeField] private GameObject regularCamera = default;
    [SerializeField] private GameObject photoCamera = default;
    [Header("Axis Configuration")]
    [SerializeField] private BoolReference isXCameraInverted = default;
    [SerializeField] private BoolReference isYCameraInverted = default;

    private PlayerControls playerControl;
    private bool photoCameraIsActive;
    private Quaternion photoCameraInitialRotation;

    private void Awake()
    {
        playerControl = new PlayerControls();
        photoCameraIsActive = false;
        changeActionMap.Raise(Utils.UnderwaterActionMap);
        photoCameraInitialRotation = photoCamera.transform.rotation;
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void Update()
    {
        if (playerControl.Underwater.ShowCamera.triggered || playerControl.Camera.ExitCamera.triggered)
        {
            ToogleCamera();
        }
        if (photoCameraIsActive)
        {
            var move = playerControl.Camera.Rotation.ReadValue<Vector2>();
            Rotate(move);
        }
    }

    private void ToogleCamera()
    {
        regularCamera.SetActive(!regularCamera.activeInHierarchy);
        photoCamera.SetActive(!photoCamera.activeInHierarchy);
        photoCameraIsActive = !photoCameraIsActive;
        if (photoCameraIsActive)
        {
            changeActionMap.Raise(Utils.CameraActionMap);
        }
        else
        {
            photoCamera.transform.rotation = photoCameraInitialRotation;
            changeActionMap.Raise(Utils.UnderwaterActionMap);
        }
    }

    private void Rotate(Vector2 direction)
    {
        if (Mathf.Abs(direction.x)  <= 0.05f && Mathf.Abs(direction.y) <= 0.05f)
        {
            return;
        }
        var xInvertedFactor = Utils.AxisCheck(isXCameraInverted.Value);
        var yInvertedFactor = Utils.AxisCheck(isYCameraInverted.Value);

        var currentRotation = photoCamera.transform.rotation;
        var xValue = currentRotation.x + (direction.y * cameraRotationSpeed.Value) * yInvertedFactor;
        var yValue = currentRotation.y + (direction.x * cameraRotationSpeed.Value) * xInvertedFactor;
        var nextRotation = new Vector3(-xValue, yValue, currentRotation.z);
        photoCamera.transform.Rotate(nextRotation);
    }
}
