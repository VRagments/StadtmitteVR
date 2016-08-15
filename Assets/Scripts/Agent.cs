using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public List<Transform> PrimaryTargets;
    public List<Transform> SecondaryTargets;

    private HashSet<LookAt> primaryTargetDisplays;
    private HashSet<LookAt> secondaryTargetDisplays;
    private NavMeshAgent navAgent;
    private NavMeshPath navPath;
    private float timePrimary = 0.0f;
    private float timeSecondary = 0.0f;

    void Awake ()
    {
        navAgent = GetComponent<NavMeshAgent> ();
        navPath = new NavMeshPath ();
        primaryTargetDisplays = new HashSet<LookAt> ();
        secondaryTargetDisplays = new HashSet<LookAt> ();
    }

    void Start ()
    {
        LookAt[] all = GetComponentsInChildren<LookAt> ();
        foreach (LookAt l in all)
        {
            if (l.name.ToLower ().Contains ("primary"))
            {
                primaryTargetDisplays.Add (l);
            }
            else
            {
                secondaryTargetDisplays.Add (l);
            }
        }
        Recalculate (PrimaryTargets, primaryTargetDisplays);
        Recalculate (SecondaryTargets, secondaryTargetDisplays);
    }

    void Update ()
    {
        timePrimary += Time.deltaTime;
        timeSecondary += Time.deltaTime;
        if (timePrimary > 0.030f)
        {
            Recalculate (PrimaryTargets, primaryTargetDisplays);
            timePrimary = 0.0f;
        }
        if (timeSecondary > 0.045f)
        {
            Recalculate (SecondaryTargets, secondaryTargetDisplays);
            timeSecondary = 0.0f;
        }
    }

    public void Recalculate (List<Transform> trans, IEnumerable<LookAt> displays)
    {
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        float shortestDist = float.MaxValue;
        bool success = false;
        foreach (Transform t in trans)
        {
            bool arrivable = navAgent.CalculatePath (t.position, navPath);
            if (arrivable && navPath.corners.Length > 1)
            {
                Vector3 tmpPos, tmpDir;
                float dist = Calculate (navPath.corners, 5.0f, out tmpPos, out tmpDir);
                if (dist < shortestDist)
                {
                    dir = tmpDir;
                    pos = tmpPos;
                    success = true;
                    shortestDist = dist;
                }
            }
        }
        if (success)
        {
            foreach (LookAt d in displays)
            {
                d.DoPosition (pos);
                d.DoLookAt (dir);
                d.MarkDistance (shortestDist);
            }
        }
    }

    private float Calculate (Vector3[] vecs, float distance, out Vector3 pos, out Vector3 dir)
    {
        pos = Vector3.zero;
        dir = Vector3.zero;
        bool setVals = true;
        float totalDist = 0.0f;
        for (int i = 1; i < vecs.Length; ++i)
        {
            float distBefore = totalDist;
            totalDist += Vector3.Distance (vecs [i - 1], vecs [i]);
            if (setVals && totalDist > distance)
            {
                pos = Vector3.MoveTowards (vecs [i - 1], vecs [i], distance - distBefore);
                dir = vecs [i];
                setVals = false;
            }
        }
        if (setVals)
        {
            // if non found return last
            dir = vecs [vecs.Length - 1];
            pos = vecs [vecs.Length - 2];
        }
        return totalDist;
    }
}