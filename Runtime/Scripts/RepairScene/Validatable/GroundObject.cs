using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class GroundObject : ValidatableMonoBehaviour
{
    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[GR] ") &&
               LayerMask.LayerToName(o.layer).Equals("Ground") &&
               o.GetComponent <Rigidbody>() == null &&
               o.GetComponent(typeof(Collider)) != null;
    }
}

}
