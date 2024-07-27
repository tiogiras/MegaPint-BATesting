using System.Collections.Generic;
using MegaPint.ValidationRequirement;
using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class EnemyResetLogic : ResetObjectLogic
{
    #region Public Methods

    public override void ResetLogic()
    {
        gameObject.GetComponent <Enemy>().SetHealth(0);

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
