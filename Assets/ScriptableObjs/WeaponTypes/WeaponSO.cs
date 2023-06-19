using UnityEngine;

[CreateAssetMenu(menuName = "Weapon", fileName = "New Weapon")]
public class WeaponSO : ScriptableObject
{
    public bool isRange;
    public int damage; 
}
