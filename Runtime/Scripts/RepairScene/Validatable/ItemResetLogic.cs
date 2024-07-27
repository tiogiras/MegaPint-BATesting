using System.Collections.Generic;
using MegaPint.ValidationRequirement;
using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class ItemResetLogic : ResetObjectLogic
{
    #region Public Methods

    public override void ResetLogic()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        var col = gameObject.GetComponent <Collider>();

        if (col != null)
            DestroyImmediate(col, true);

        var rigid = gameObject.GetComponent <Rigidbody>();

        if (rigid != null)
            DestroyImmediate(rigid, true);

        ValidatableMonoBehaviour[] vmbs = gameObject.GetComponents <ValidatableMonoBehaviour>();

        foreach (ValidatableMonoBehaviour vmb in vmbs)
        {
            vmb.SetRequirements(new List <ScriptableValidationRequirement>());
            vmb.ClearImportedSettings();
        }
    }

    #endregion
}

}
