using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Camera myCam;
    public LayerMask obstacle;
    private Ray camToUnitRay;
    public List<Obstacle> obstacleHitList = new List<Obstacle>();

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

        if (hits.Length == 0) { // No obstacle hit(s) between unit and camera
            if (obstacleHitList.Count <= 0) return; // if list is empty

            foreach (Obstacle obstacle in obstacleHitList){ 
                obstacle.IsBlockingRay = false;
            }
            obstacleHitList.Clear();
            return;
        }

        // if any hits
        foreach (RaycastHit obstHit in hits){
            Debug.Log("Ray hitting: " + obstHit.transform.name);
            obstHit.transform.gameObject.GetComponent<Obstacle>().IsBlockingRay = true;

            if (!obstacleHitList.Contains(obstHit.transform.gameObject.GetComponent<Obstacle>()))
                obstacleHitList.Add(obstHit.transform.gameObject.GetComponent<Obstacle>());
        }
    }
    
    void OnDestroy() {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
