using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class StrongEnemy : ValidatableMonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private string _name;
    
    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[ELITE] ") && !string.IsNullOrEmpty(_name) && _health is >= 30 and <= 60;
    }

    public void SetHealth(int health)
    {
        _health = health;
    }
}

}
