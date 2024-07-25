using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class DebugObject : ValidatableMonoBehaviour
{
    public bool ValidateManually()
    {
        GameObject o = gameObject;

        return o.name.StartsWith("[DEBUG] ") && !o.activeSelf;
    }
}

}
