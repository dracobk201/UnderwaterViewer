using ScriptableObjectArchitecture;
using System;
using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody creatureRigidbody = default;
    [SerializeField] private FloatReference creatureVelocity = default;
    [SerializeField] private FloatReference minTimeInterval = default;
    [SerializeField] private FloatReference maxTimeInterval = default;
    [SerializeField] private Vector3Reference anchorVector = default;
    private float currentTime = 0;
    private float randomTime = 0;
    private Vector3 topLeft;
    private Vector3 bottomLeft;
    private Vector3 topRight;
    private Vector3 bottomRight;
    private Vector3 lastTargetPosition;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= randomTime)
        {
            Move();
            currentTime = 0;
            randomTime = UnityEngine.Random.Range(minTimeInterval.Value, maxTimeInterval.Value);
        }
    }

    private void Move()
    {
        SetupRotation(lastTargetPosition);

        topLeft = transform.forward + new Vector3(-anchorVector.Value.x, anchorVector.Value.y);
        bottomLeft = transform.forward - new Vector3(anchorVector.Value.x, anchorVector.Value.y);
        topRight = transform.forward + new Vector3(anchorVector.Value.x, anchorVector.Value.y);
        bottomRight = transform.forward + new Vector3(anchorVector.Value.x, -anchorVector.Value.y);

        var xValue = UnityEngine.Random.Range(topLeft.x, topRight.x);
        var yValue = UnityEngine.Random.Range(topLeft.y, bottomRight.y);
        var zValue = UnityEngine.Random.Range(anchorVector.Value.z * 0.05f, anchorVector.Value.z);
        var targetPosition = new Vector3(xValue, yValue, zValue) * creatureVelocity.Value;
        if (!CheckNewPosition(targetPosition))
        {
            return;
        }
        lastTargetPosition = targetPosition;
        SetupRotation(targetPosition);
        creatureRigidbody.AddRelativeForce(targetPosition, ForceMode.Acceleration);
    }

    private void SetupRotation(Vector3 targetPosition)
    {
        Vector3 relativePos = targetPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    private bool CheckNewPosition(Vector3 newPosition)
    {
        var canMove = true;
        Collider[] colliders = Physics.OverlapSphere(newPosition, 1);
        if (Array.Exists(colliders, x => x.CompareTag(Global.CreatureTag) || x.CompareTag(Global.PlayerTag)))
        {
            canMove = false;
        }
        return canMove;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;


    //    Gizmos.DrawRay(transform.position, topLeft);
    //    Gizmos.DrawRay(transform.position, bottomLeft);
    //    Gizmos.DrawRay(transform.position, topRight);
    //    Gizmos.DrawRay(transform.position, bottomRight);
    //}
}
