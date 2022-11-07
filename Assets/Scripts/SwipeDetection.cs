using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private BoolReference photoCameraIsActive = default;
    [SerializeField] private BoolReference isSwipeDetectionActive = default;
    [SerializeField] private FloatReference mininumSwipeDistance = default;
    [SerializeField] private FloatReference maximumSwipeTime = default;
    [SerializeField] private FloatReference directionThreshold = default;
    [SerializeField] private Vector2GameEvent swipeDirection = default;

    private PlayerControls playerControl;
    private Camera gameCamera;
    private Vector2 lastStartPosition;
    private float lastStartTime;

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

    private void Start()
    {
        gameCamera = Camera.main;
        playerControl.Underwater.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControl.Underwater.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

        playerControl.Camera.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControl.Camera.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (!isSwipeDetectionActive.Value)
        {
            return;
        }

        Vector3 worldPosition;
        Vector3 touchPosition;

        if (photoCameraIsActive.Value)
        {
            touchPosition = playerControl.Camera.PrimaryPosition.ReadValue<Vector2>();
        }
        else
        {
            touchPosition = playerControl.Underwater.PrimaryPosition.ReadValue<Vector2>();
        }

        worldPosition = Utils.ScreenToWorld(gameCamera, touchPosition);

        SwipeStart(worldPosition, (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (!isSwipeDetectionActive.Value)
        {
            return;
        }

        Vector3 worldPosition;
        Vector3 touchPosition;

        if (photoCameraIsActive.Value)
        {
            touchPosition = playerControl.Camera.PrimaryPosition.ReadValue<Vector2>();
        }
        else
        {
            touchPosition = playerControl.Underwater.PrimaryPosition.ReadValue<Vector2>();
        }

        worldPosition = Utils.ScreenToWorld(gameCamera, touchPosition);
        SwipeEnd(worldPosition, (float)context.time);
    }

    private void SwipeStart(Vector2 startPosition, float startTime)
    {
        lastStartPosition = startPosition;
        lastStartTime = startTime;
    }

    private void SwipeEnd(Vector2 endPosition, float endTime)
    {
        if (Vector3.Distance(lastStartPosition, endPosition) >= mininumSwipeDistance.Value && endTime - lastStartTime <= maximumSwipeTime.Value)
        {
            //Debug.DrawLine(lastStartPosition, endPosition, Color.red, 5f);

            Vector3 direction = endPosition - lastStartPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            swipeDirection.Raise(direction2D);
        }
    }

    //private void SwipeDirection(Vector2 direction)
    //{
    //    var dottedUp = Vector2.Dot(Vector2.up, direction);
    //    if (dottedUp > directionThreshold.Value)
    //    {
    //        Debug.Log($"Swipe up - {dottedUp} - {direction}");
    //    }
    //    var dottedDown = Vector2.Dot(Vector2.down, direction);
    //    if (Vector2.Dot(Vector2.down, direction) > directionThreshold.Value)
    //    {
    //        Debug.Log($"Swipe down - {dottedDown} - {direction}");
    //    }
    //    var dottedLeft = Vector2.Dot(Vector2.left, direction);
    //    if (Vector2.Dot(Vector2.left, direction) > directionThreshold.Value)
    //    {
    //        Debug.Log($"Swipe left - {dottedLeft} - {direction}");
    //    }
    //    var dottedRight = Vector2.Dot(Vector2.right, direction);
    //    if (Vector2.Dot(Vector2.right, direction) > directionThreshold.Value)
    //    {
    //        Debug.Log($"Swipe right - {dottedRight} - {direction}");
    //    }
    //    swipeDirection
    //}
}
