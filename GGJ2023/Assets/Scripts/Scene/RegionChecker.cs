using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegionChecker : MonoBehaviour
{
    public GameObject CollisionRoot;
    public GameObject CollisionDetect;
    public GameObject CinemachineTargetGroup;
    public Vector3 HitOffset;

    // Start is called before the first frame update
    void Start()
	{
        gFixedUpdateBaseTime = DateTime.Now;

    }

    private DateTime gFixedUpdateBaseTime;
    // Update is called once per frame
    void FixedUpdate()
    {
        TimeSpan aInterval = DateTime.Now - gFixedUpdateBaseTime;
        if (aInterval.TotalMilliseconds > 100)
        {
            gFixedUpdateBaseTime = DateTime.Now;

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(new Vector3(1, 0, 0)), 100);
            foreach (RaycastHit aHit in hits)
            {
                Vector3 aHitPosition = 
                    transform.position + 
                    (transform.TransformDirection(new Vector3(1, 0, 0)) * aHit.distance) +
                    HitOffset;
                GameObject aDetectedObj = Instantiate(CollisionDetect, aHitPosition, new Quaternion());
                aDetectedObj.transform.parent = CollisionRoot.transform;
            }
        }
    }

}
