using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class DistanceExtenderFounder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var me = GetComponent<Character>();
        foreach (Transform o in transform.parent)
        {
            var de = o.GetComponent<DistanceExtender>();
            if (de == null) continue;
            if (de.gameObject == this.gameObject) continue;
            if (!de.Contains(me)) continue;
            me.SetCheckDistanceObject(me.CheckDistance* de.distanceScale+de.distanceOffset);
            return;
        }
        me.SetCheckDistanceObject(me.CheckDistance);
    }
}
