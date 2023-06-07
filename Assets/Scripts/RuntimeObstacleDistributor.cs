using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeObstacleDistributor : ObstacleDistributor
{
    // Start is called before the first frame update
    public override void Start()
    {
        GridBuildingSystem.Instance.OnBuildingBuilt += OnBuildEventTriggered;
    }

    private void OnBuildEventTriggered()
    {
        for (int i=0; i< transform.childCount; i++){
            if (transform.GetChild(i).GetChild(0).gameObject.TryGetComponent<Obstacle>(out Obstacle component)) continue;
            transform.GetChild(i).GetChild(0).gameObject.AddComponent<Obstacle>();
        }
    }
}
