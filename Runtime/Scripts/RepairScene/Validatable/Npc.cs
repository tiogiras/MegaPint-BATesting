using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class Npc : ValidatableMonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private string _name;
}

}
