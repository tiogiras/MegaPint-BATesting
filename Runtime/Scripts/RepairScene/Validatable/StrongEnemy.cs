using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class StrongEnemy : ValidatableMonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private string _name;
}

}
