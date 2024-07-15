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
        if (target != null)
        {
            delayFrames = Mathf.RoundToInt(followDelay / Time.deltaTime);
            StartCoroutine(FollowTarget());
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        delayFrames = Mathf.RoundToInt(followDelay / Time.deltaTime);
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (true)
        {
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