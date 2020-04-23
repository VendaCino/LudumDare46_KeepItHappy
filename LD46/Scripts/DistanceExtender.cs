using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class DistanceExtender : MonoBehaviour
{
    public float distanceScale=2;
    public float distanceOffset = 0;

    public bool Contains(Character other)
    {
        var me = GetComponent<Character>();
        return me.CheckDistanceCollider2D.OverlapPoint(other.transform.position);
    }
}
