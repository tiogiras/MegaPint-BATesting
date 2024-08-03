using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class Npc : ValidatableMonoBehaviour
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

        return o.name.StartsWith("[NPC] ") && !string.IsNullOrEmpty(_name) && _health is >= 50 and <= 99;
    }

    #endregion
}

}
