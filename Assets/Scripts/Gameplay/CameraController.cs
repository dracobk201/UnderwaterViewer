using UnityEngine;
using ScriptableObjectArchitecture;
using System;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private BoolReference photoCameraIsActive = default;
    [SerializeField] private FloatReference cameraRotationSpeed = default;
    [SerializeField] private StringGameEvent changeActionMap = default;
    [SerializeField] private StringGameEvent screenshotTaken = default;

    [SerializeField] private GameObject regularCamera = default;
    [SerializeField] private GameObject photoCamera = default;

    [Header("Axis Configuration")]
    [SerializeField] private BoolReference isXCameraInverted = default;
    [SerializeField] private BoolReference isYCameraInverted = default;

    private PlayerControls playerControl;
    
    private Quaternion photoCameraInitialRotation;

    private void Awake()
    {
        playerControl = new PlayerControls();
        photoCameraIsActive.Value = false;
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
        if (photoCameraIsActive.Value)
        {
            var move = playerControl.Camera.Rotation.ReadValue<Vector2>();
            Rotate(move);
        }
        if (playerControl.Camera.Shoot.triggered)
        {
            CaptureSnapshot();
        }
    }

    private void ToogleCamera()
    {
        regularCamera.SetActive(!regularCamera.activeInHierarchy);
        photoCamera.SetActive(!photoCamera.activeInHierarchy);
        photoCameraIsActive.Value = !photoCameraIsActive.Value;
        if (photoCameraIsActive.Value)
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
        if (Mathf.Abs(direction.x) <= 0.05f && Mathf.Abs(direction.y) <= 0.05f)
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

    public void CaptureSnapshot()
    {
        int captureWidth = Screen.width;
        int captureHeight = Screen.height;
        Rect rect = new Rect(0, 0, captureWidth, captureHeight);
        var renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
        Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

        var targetCamera = Camera.main;
        targetCamera.targetTexture = renderTexture;
        targetCamera.Render();

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);

        targetCamera.targetTexture = null;
        RenderTexture.active = null;

        string filename = $"Capture-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png";
        byte[] fileHeader = null;
        byte[] fileData = null;

        fileData = screenShot.EncodeToPNG();
        string base64ImageRepresentation = Convert.ToBase64String(fileData);
        screenshotTaken.Raise(base64ImageRepresentation);

        new System.Threading.Thread(() =>
        {
            var file = System.IO.File.Create(filename);
            if (fileHeader != null)
            {
                file.Write(fileHeader, 0, fileHeader.Length);
            }
            file.Write(fileData, 0, fileData.Length);
            file.Close();
            Debug.Log($"Wrote screenshot {filename} of size {fileData.Length}");
        }).Start();

        Destroy(renderTexture);
    }
}
