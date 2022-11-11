using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBehaviour : MonoBehaviour
{
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
        var xPositionVariant = Random.Range(-swimLimits.Value.x, swimLimits.Value.x);
        var yPositionVariant = Random.Range(-swimLimits.Value.y, swimLimits.Value.y);
        var zPositionVariant = Random.Range(-swimLimits.Value.z, swimLimits.Value.z);
        return new Vector3(xPositionVariant, yPositionVariant, zPositionVariant); ;
    }
}
