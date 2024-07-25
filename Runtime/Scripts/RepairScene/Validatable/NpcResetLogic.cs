using System.Collections.Generic;
using MegaPint.ValidationRequirement;
using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class NpcResetLogic : ResetObjectLogic
{
    #region Public Methods

    public override void ResetLogic()
    {
        gameObject.GetComponent <Npc>().SetHealth(0);
        
        ValidatableMonoBehaviour[] vmbs = gameObject.GetComponents <ValidatableMonoBehaviour>();
        
        foreach (var vmb in vmbs)
        {
            vmb.SetRequirements(new List <ScriptableValidationRequirement>());
            vmb.ClearImportedSettings();
        }
    }

    #endregion
}

}
