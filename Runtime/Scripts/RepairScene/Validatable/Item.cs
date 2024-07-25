using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class Item : ValidatableMonoBehaviour
{
    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[ITEM] ") &&
               LayerMask.LayerToName(o.layer).Equals("Item") &&
               o.GetComponent <Rigidbody>() != null &&
               o.GetComponent(typeof(Collider)) != null;
    }
}

}
