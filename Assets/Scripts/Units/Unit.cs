using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Camera myCam;
    public LayerMask obstacle;
    private Ray camToUnitRay;
    public List<Obstacle> currentObstHitList = new List<Obstacle>();

    void Start()
    {
        myCam = Camera.main;
        UnitSelection.Instance.unitList.Add(this.gameObject);
    }

    void Update(){
        CheckIfObstacleBlocking();
    }

    void CheckIfObstacleBlocking(){
        camToUnitRay = myCam.ScreenPointToRay(myCam.WorldToScreenPoint(transform.position));
        RaycastHit[] hits = Physics.RaycastAll(camToUnitRay, Mathf.Infinity, obstacle); // When hitting obstacle

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
            Debug.Log("Ray hitting: " + obstHit.transform.name);
            Obstacle thisHitObst = obstHit.transform.gameObject.GetComponent<Obstacle>();
            thisHitObst.IsBlockingRay = true;

            if (!currentObstHitList.Contains(thisHitObst))
                currentObstHitList.Add(thisHitObst);
        }
    }
    
    void OnDestroy() {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
