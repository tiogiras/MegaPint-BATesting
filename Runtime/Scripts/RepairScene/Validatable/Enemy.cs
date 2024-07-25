using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class Enemy : ValidatableMonoBehaviour
{
    [SerializeField] private float _health;
    
    public void SetHealth(float health)
    {
        _health = health;
    }
    
    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[E] ") && _health is >= 10 and <= 30;
    }
}

}
