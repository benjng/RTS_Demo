using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDistributor : MonoBehaviour
{
    private int childCount;
    void Start()
    {
        childCount = transform.childCount;
        for (int i=0; i<childCount; i++){
            transform.GetChild(i).gameObject.AddComponent<Obstacle>();
        }
    }
}
