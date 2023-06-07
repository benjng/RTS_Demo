using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // TODO: Add Enemy Unit
    public LayerMask obstacleLayer;
    public List<Obstacle> currentObstHitList = new List<Obstacle>();
    public UnitSO unitSO;
    public GameObject HPBar;
    public int CurrentHP;

    private Camera myCam;
    private Ray camToUnitRay;

    void Start()
    {
        myCam = Camera.main;
        UnitSelection.Instance.unitList.Add(this.gameObject); // add this gameobject to unitList when game start
    }

    void Update(){
        CheckObstacle();
    }

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

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, unitSO.DetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitSO.AttackRange);
    }
}
