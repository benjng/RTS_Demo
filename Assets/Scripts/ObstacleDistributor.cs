using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDistributor : MonoBehaviour
{
    public virtual void Start()
    {
        for (int i=0; i< transform.childCount; i++){
            transform.GetChild(i).gameObject.AddComponent<Obstacle>();
        }
    }
}
