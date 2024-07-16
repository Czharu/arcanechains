using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follower : MonoBehaviour
{
    public Transform target;
    private Queue<Vector3> positions = new Queue<Vector3>();
    private float followDelay = 1f;
    private int delayFrames;

    void Start()
    {
        delayFrames = Mathf.RoundToInt(followDelay / Time.deltaTime); // Initialize delayFrames only once
        if (target != null)
        {
            StartCoroutine(FollowTarget());
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (!isActiveAndEnabled)
        {
        StartCoroutine(FollowTarget());
        }
    }

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            if (target == null)
            {
                Debug.Log("Follower follower target not found");
                yield break; // Exit the coroutine if the target is null
            }
            positions.Enqueue(target.position);

            if (positions.Count > delayFrames)
            {
                Vector3 targetPosition = positions.Dequeue();
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f);
            }

            yield return null;
        }
    }
}