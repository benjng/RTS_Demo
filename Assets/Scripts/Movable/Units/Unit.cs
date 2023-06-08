using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{   // TODO: BuildingUnit, ArmyUnit, EnemyUnit
    public int CurrentHP;
    public UnitSO unitSO;
    public GameObject HPBarCanvas;

    public virtual void Start(){
        CurrentHP = unitSO.MaxHP;
    }
}
