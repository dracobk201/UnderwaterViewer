using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [SerializeField] private BoolReference photoCameraIsActive = default;
    [SerializeField] private Rigidbody shipRigidbody = default;
    [SerializeField] private GameObject shipLantern = default;
    [SerializeField] private FloatReference shipForwardAcceleration = default;
    [SerializeField] private FloatReference shipVerticalAcceleration = default;
    [SerializeField] private FloatReference shipRotationAcceleration = default;
    [Header("Axis Configuration")]
    [SerializeField] private BoolReference isXMovementInverted = default;
    [SerializeField] private BoolReference isYMovementInverted = default;

    private PlayerControls playerControl;

    private void Awake()
    {
        playerControl = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void FixedUpdate()
    {
        var move = playerControl.Underwater.Move.ReadValue<Vector2>();

        MoveAndRotate(move);

        if (playerControl.Underwater.Light.triggered)
        {
            shipLantern.SetActive(!shipLantern.activeInHierarchy);
        }

        Propulsion(playerControl.Underwater.Propulsion.ReadValue<float>());
    }

    public void GettingSwipeDirection(Vector2 directionValue)
    {
        if (photoCameraIsActive.Value)
        {
            return;
        }
        MoveAndRotate(directionValue);
    }

    private void MoveAndRotate(Vector2 axisValue)
    {
        Move(axisValue.y);
        Rotate(axisValue.x);
    }

    private void Rotate(float xValue)
    {
        var invertedFactor = Utils.AxisCheck(isYMovementInverted.Value);
        var newXRotation = transform.up * shipRotationAcceleration.Value * xValue * invertedFactor;
        transform.Rotate(newXRotation);
    }

    private void Move(float yValue)
    {
        var force = Vector3.zero;
        var invertedFactor = Utils.AxisCheck(isXMovementInverted.Value);
        force.y = yValue * shipVerticalAcceleration.Value * invertedFactor;
        transform.Translate(force);
    }

    private void Propulsion(float throllingValue)
    {
        if (throllingValue < 0.05f)
        {
            return;
        }
        var force = transform.forward * shipForwardAcceleration.Value;
        shipRigidbody.AddForce(force, ForceMode.Acceleration);
    }

}
