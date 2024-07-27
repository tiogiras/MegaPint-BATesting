using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class Enemy : ValidatableMonoBehaviour
{
    [SerializeField] private float _health;

    #region Public Methods

    public void SetHealth(float health)
    {
        _health = health;
    }

    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[E] ") && _health is >= 10 and <= 30;
    }

    #endregion
}

}
