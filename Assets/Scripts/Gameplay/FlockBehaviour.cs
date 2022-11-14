using ScriptableObjectArchitecture;
using UnityEngine;

public class FlockBehaviour : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private BoolReference isGizmosActivated = default;

    [Header("Data")]
    [SerializeField] private GameObject fishPrefab = default;
    [SerializeField] private IntReference minfishSchoolingSize = default;
    [SerializeField] private IntReference maxfishSchoolingSize = default;
    [SerializeField] private FloatReference changeGoalProbability = default;
    [SerializeField] private Vector3Reference swimLimits = default;
    [HideInInspector] public Vector3 goalPosition;
    [HideInInspector] public GameObject[] schoolingCollection;

    private void Start()
    {
        schoolingCollection = new GameObject[Random.Range(minfishSchoolingSize.Value, maxfishSchoolingSize.Value)];
        for (int i = 0; i < schoolingCollection.Length; i++)
        {

            Vector3 bornPosition = transform.position + SetPositionBetweenSwimLimits();
            Quaternion bornRotation = Quaternion.identity;
            schoolingCollection[i] = Instantiate(fishPrefab, bornPosition, bornRotation, transform);
        }
        goalPosition = transform.position;
    }

    private void Update()
    {
        if (Random.Range(0,100) < changeGoalProbability.Value)
        {
            goalPosition = SetPositionBetweenSwimLimits();
        }
    }

    private Vector3 SetPositionBetweenSwimLimits()
    {
        var xPositionVariant = Random.Range(transform.position.x - swimLimits.Value.x, transform.position.x + swimLimits.Value.x);
        var yPositionVariant = Random.Range(transform.position.y - swimLimits.Value.y, transform.position.y + swimLimits.Value.y);
        var zPositionVariant = Random.Range(transform.position.z - swimLimits.Value.z, transform.position.z + swimLimits.Value.z);
        return new Vector3(xPositionVariant, yPositionVariant, zPositionVariant); ;
    }

    public void OnDrawGizmos()
    {
        if (!isGizmosActivated.Value)
        {
            return;
        }


        Gizmos.color = Color.red;
        Vector3 topLeftFront = new Vector3(transform.position.x - swimLimits.Value.x, transform.position.y + swimLimits.Value.y, transform.position.z - swimLimits.Value.z);
        Vector3 topRightFront = new Vector3(transform.position.x - swimLimits.Value.x, transform.position.y + swimLimits.Value.y, transform.position.z + swimLimits.Value.z);
        Vector3 bottomLeftFront = new Vector3(transform.position.x - swimLimits.Value.x, transform.position.y - swimLimits.Value.y, transform.position.z - swimLimits.Value.z);
        Vector3 bottomRightFront = new Vector3(transform.position.x - swimLimits.Value.x, transform.position.y - swimLimits.Value.y, transform.position.z + swimLimits.Value.z);
        Vector3 topLeftBack = new Vector3(transform.position.x + swimLimits.Value.x, transform.position.y + swimLimits.Value.y, transform.position.z - swimLimits.Value.z);
        Vector3 topRightBack = new Vector3(transform.position.x + swimLimits.Value.x, transform.position.y + swimLimits.Value.y, transform.position.z + swimLimits.Value.z);
        Vector3 bottomLeftBack = new Vector3(transform.position.x + swimLimits.Value.x, transform.position.y - swimLimits.Value.y, transform.position.z - swimLimits.Value.z);
        Vector3 bottomRightBack = new Vector3(transform.position.x + swimLimits.Value.x, transform.position.y - swimLimits.Value.y, transform.position.z + swimLimits.Value.z);

        Gizmos.DrawLine(topLeftFront, topRightFront);
        Gizmos.DrawLine(bottomLeftFront, bottomRightFront);
        Gizmos.DrawLine(topLeftBack, topRightBack);
        Gizmos.DrawLine(bottomLeftBack, bottomRightBack);

        Gizmos.DrawLine(topLeftFront, bottomLeftFront);
        Gizmos.DrawLine(topRightFront, bottomRightFront);
        Gizmos.DrawLine(topLeftBack, bottomLeftBack);
        Gizmos.DrawLine(topRightBack, bottomRightBack);

        Gizmos.DrawLine(topLeftFront, topLeftBack);
        Gizmos.DrawLine(topRightFront, topRightBack);
        Gizmos.DrawLine(bottomLeftFront, bottomLeftBack);
        Gizmos.DrawLine(bottomRightFront, bottomRightBack);

        Gizmos.DrawLine(topLeftFront, bottomRightBack);
        Gizmos.DrawLine(topRightFront, bottomLeftBack);

    }

}
