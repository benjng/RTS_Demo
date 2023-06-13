using System.Collections.Generic;
using UnityEngine;

public class ArmyUnit : Unit
{
    [SerializeField] private LayerMask obstacleLayer;
    private List<Obstacle> currentObstHitList = new List<Obstacle>();
    private Camera myCam;
    private Ray camToUnitRay;

    public override void Start()
    {
        base.Start();
        myCam = Camera.main;
        UnitSelection.Instance.unitList.Add(this.gameObject); // add this gameobject to unitList when game start
    }

    public override void Update(){
        base.Update();
        CheckObstacle();
    }

    // TODO: All units should check obstacles?
    void CheckObstacle(){
        if (unitSO.unitType == UnitType.Building) return;
        
        camToUnitRay = myCam.ScreenPointToRay(myCam.WorldToScreenPoint(transform.position));
        RaycastHit[] hits = Physics.RaycastAll(camToUnitRay, Mathf.Infinity, obstacleLayer);

        // No obstacle hit(s) between unit and camera
        if (hits.Length == 0) { 
            // if list is empty
            if (currentObstHitList.Count <= 0) return; 

            // Loop through all existing obstacles
            foreach (Obstacle obstacle in currentObstHitList){ 
                // Release the obstacle (Correct: Release the obstacle that is not blocking)
                obstacle.IsBlockingRay = false;
            }
            // Clear obstacle hit list
            currentObstHitList.Clear();
            return;
        }

        // if any hits
        foreach (RaycastHit obstHit in hits){
            // Debug.Log("Ray hitting: " + obstHit.transform.name);
            Obstacle thisHitObst = obstHit.transform.gameObject.GetComponentInChildren<Obstacle>();
            if (thisHitObst == null) return;
            thisHitObst.IsBlockingRay = true;

            if (!currentObstHitList.Contains(thisHitObst))
                currentObstHitList.Add(thisHitObst);
        }
    }
    
    void OnDestroy() {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
