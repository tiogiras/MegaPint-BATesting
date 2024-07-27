using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class DebugObject : ValidatableMonoBehaviour
{
    #region Public Methods

    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[DEBUG] ") && !o.activeSelf;
    }

    #endregion
}

}
