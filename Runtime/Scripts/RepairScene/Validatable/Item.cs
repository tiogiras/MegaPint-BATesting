using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class Item : ValidatableMonoBehaviour
{
    #region Public Methods

    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[ITEM] ") &&
               LayerMask.LayerToName(o.layer).Equals("Item") &&
               o.GetComponent <Rigidbody>() != null &&
               o.GetComponent(typeof(Collider)) != null;
    }

    #endregion
}

}
