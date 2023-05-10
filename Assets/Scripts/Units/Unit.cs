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

        if (hits.Length > 0){ // Hit(s) detected
            foreach (RaycastHit _hit in hits){
                Debug.Log("Ray hitting: " + _hit.transform.name);
                _hit.transform.gameObject.GetComponentInParent<Obstacle>().IsBlockingRay = true;

                if (!obstacleHitList.Contains(_hit.transform.gameObject.GetComponentInParent<Obstacle>()))
                    obstacleHitList.Add(_hit.transform.gameObject.GetComponentInParent<Obstacle>());
            }
        } else { // No obstacle hit(s) between unit and camera
            if (obstacleHitList.Count > 0) {
                foreach (Obstacle obstacle in obstacleHitList){ // Clear out all list
                    obstacle.IsBlockingRay = false;
                }
                obstacleHitList.Clear();
            }
        }
    }
    
    void OnDestroy() {
        UnitSelection.Instance.unitList.Remove(this.gameObject);
    }
}
