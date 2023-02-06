using System;
using System.Collections;
using System.Collections.Generic;
using Micosmo.SensorToolkit;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class EnemyController : MonoBehaviour
{

    [Serializable]
    public class PatrollingPoint
    {
        public Transform Point;
        public PatrolType action;
        public float WaitTime;
        public enum PatrolType
        {
            Move,
            RotateAndMove,
            Look,
        }
    }

    public LOSSensor sensor;
    public List<PatrollingPoint> PatrollingPoints;

    private float mWaitTime;
    private PatrollingPoint mCurrentPoint;
    private int mCurrentPointIndex;
    private NavMeshAgent mNavMeshAgent;
    public float waitBeforePatrol = 1f;

    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();

        mCurrentPoint = PatrollingPoints[0];
        mCurrentPointIndex = 0;

        transform.position = PatrollingPoints[0].Point.position;
        mCoroutine = StartCoroutine(GoToNextPoint());
    }
    Coroutine mCoroutine;


    GameObject playerTarget;
    public void Detecting()
    {
        mNavMeshAgent.isStopped = true;
        StartCoroutine(DetectingCoroutine());
    }
    IEnumerator DetectingCoroutine()
    {
        do
        {
            var playerTarget = sensor.GetDetections().Find(x=>x.gameObject.CompareTag("Player"));
            if (playerTarget != null)
            {
                isPatrolling = false;
                if(mCoroutine != null) StopCoroutine(mCoroutine);
                mCoroutine = StartCoroutine(LookAtPos(playerTarget.transform.position, 0.3f));
            }
            yield return new WaitForSeconds(0.1f);
        } while (playerTarget == null);
        
    }
    public void NotDetecting()
    {
        if (!isPatrolling)
        {
            isPatrolling = true;
            if(mCoroutine != null) StopCoroutine(mCoroutine);
            mCoroutine = StartCoroutine(WaitAndGoToNextPoint(waitBeforePatrol));
        }
    }
    IEnumerator WaitAndGoToNextPoint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mNavMeshAgent.isStopped = false;
        mCoroutine = StartCoroutine(GoToNextPoint(0));
    }

    bool isMoving()
    {
        
        if (!mNavMeshAgent.pathPending)
        {
            if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
            {
                if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }
        return true;
    }
    bool isPatrolling = false;
    IEnumerator GoToNextPoint(int add = 1)
    {
        isPatrolling = true;
        mCurrentPointIndex += add;
        if (mCurrentPointIndex >= PatrollingPoints.Count)
            mCurrentPointIndex = 0;

        mCurrentPoint = PatrollingPoints[mCurrentPointIndex];
        if (mCurrentPoint.action == PatrollingPoint.PatrolType.Move)
        {
            yield return MoveToPos(mCurrentPoint.Point.position);
        }
        else if(mCurrentPoint.action == PatrollingPoint.PatrolType.RotateAndMove)
        {
            yield return LookAtPos(mCurrentPoint.Point.position);
            yield return MoveToPos(mCurrentPoint.Point.position);
        }
        else if(mCurrentPoint.action == PatrollingPoint.PatrolType.Look)
        {
            yield return LookAtPos(mCurrentPoint.Point.position);
        }

        yield return new WaitForSeconds(mCurrentPoint.WaitTime);
        mCoroutine = StartCoroutine(GoToNextPoint()); 
    }

    
    private IEnumerator MoveToPos(Vector3 pos)
    {
        mNavMeshAgent.destination = pos;
        while (isMoving())
        {
            yield return null;
        }
    }
    int lookId = 0;
    IEnumerator LookAtPos(Vector3 pos, float time = 1f)
    {
        float angle = Vector3.SignedAngle(transform.forward, pos - transform.position, Vector3.up);
        if (lookId != 0)
            LeanTween.cancel(lookId);
        
        lookId = LeanTween.rotateAround(gameObject, Vector3.up, angle, time).id;
        yield return new WaitForSeconds(1);
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (PatrollingPoints.Count > 0)
        {
            for (int i = 1; i < PatrollingPoints.Count; ++i)
            {
                Gizmos.DrawLine(PatrollingPoints[i - 1].Point.position, PatrollingPoints[i].Point.position);
            }

            Gizmos.DrawLine(PatrollingPoints[PatrollingPoints.Count - 1].Point.position,
                PatrollingPoints[0].Point.position);
        }
    }
}
