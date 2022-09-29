using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class BubbleBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem bubbleParticles = default;
    [SerializeField] private FloatReference minTimeToSpawn = default;
    [SerializeField] private FloatReference maxTimeToSpawn = default;

    private void Start()
    {
        StartCoroutine(ShowingBubbles());
    }

    private IEnumerator ShowingBubbles()
    {
        while (true)
        {
            var timeToWait = Random.Range(minTimeToSpawn.Value, maxTimeToSpawn.Value);
            yield return new WaitForSeconds(timeToWait);
            bubbleParticles.Play();
        }
    }
}
