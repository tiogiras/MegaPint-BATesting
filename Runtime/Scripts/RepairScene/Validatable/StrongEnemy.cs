using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class StrongEnemy : ValidatableMonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private string _name;

    #region Public Methods

    public void SetHealth(int health)
    {
        _health = health;
    }

    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[ELITE] ") && !string.IsNullOrEmpty(_name) && _health is >= 30 and <= 60;
    }

    #endregion
}

}
