using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit", fileName = "New Unit")]
public class UnitSO : ScriptableObject
{
    public string unitName;
    public Material material;
    public Sprite unitIcon;
    public UnitType unitType;
    public WeaponSO weaponSO;
    public int MaxHP;
    public float DetectRadius;
    public float AttackRadius;
    public float ShootingInterval;
    public float BulletSpeed = 50f;
}
