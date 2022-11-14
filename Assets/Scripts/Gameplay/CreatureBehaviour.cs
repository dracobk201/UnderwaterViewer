using ScriptableObjectArchitecture;
using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody creatureRigidbody = default;
    [SerializeField] private FloatReference minFishSpeed = default;
    [SerializeField] private FloatReference maxFishSpeed = default;
    [SerializeField] private FloatReference rotationSpeed = default;
    [SerializeField] private FloatReference neighbourDistance = default;
    [SerializeField] private FloatReference resetSpeedProbability = default;
    [SerializeField] private FloatReference applyRulesProbability = default;
    [SerializeField] private Vector3Reference swimLimits = default;

    private GameObject[] fishSchooling;
    private float speed = 0;
    private Vector3 schoolingPosition;
    private Vector3 schoolingGoalPosition;
    private Bounds swimingBoundaries;

    private void Start()
    {
        FlockBehaviour flockBehaviour = transform.parent.GetComponent<FlockBehaviour>();
        schoolingPosition = flockBehaviour.transform.position;
        fishSchooling = flockBehaviour.schoolingCollection;
        schoolingGoalPosition = flockBehaviour.goalPosition;
        speed = Random.Range(minFishSpeed.Value, maxFishSpeed.Value);
    }

    private void Update()
    {
        swimingBoundaries = new Bounds(schoolingPosition, swimLimits.Value * 2);
        if (!swimingBoundaries.Contains(transform.position))
        {
            Vector3 direction = schoolingPosition - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed.Value * Time.deltaTime);
        }
        else
        {
            if (Random.value < resetSpeedProbability.Value)
            {
                speed = Random.Range(minFishSpeed.Value, maxFishSpeed.Value);
            }

            if (Random.value < applyRulesProbability.Value)
            {
                ApplyRules();
            }
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void ApplyRules()
    {
        Vector3 vectorCentre = Vector3.zero;
        Vector3 vectorAvoid = Vector3.zero;
        float groupSpeed = 0.01f;
        float nextNeighbourDistance;
        int groupSize = 0;

        foreach (GameObject fish in fishSchooling)
        {
            if (fish != gameObject)
            {
                nextNeighbourDistance = Vector3.Distance(fish.transform.position, transform.position);
                if (nextNeighbourDistance <= neighbourDistance.Value)
                {
                    vectorCentre += fish.transform.position;
                    groupSize++;

                    if (nextNeighbourDistance < 1.0f)
                    {
                        vectorAvoid += (transform.position - fish.transform.position);
                    }

                    CreatureBehaviour anotherCreature = fish.GetComponent<CreatureBehaviour>();
                    groupSpeed += anotherCreature.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vectorCentre = vectorCentre / groupSize + (schoolingGoalPosition - transform.position);
            
            speed = groupSpeed / groupSize;
            if (speed > maxFishSpeed.Value)
            {
                speed = maxFishSpeed.Value;
            }

            Vector3 direction = (vectorCentre + vectorAvoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed.Value * Time.deltaTime);
            }
        }
    }
}
