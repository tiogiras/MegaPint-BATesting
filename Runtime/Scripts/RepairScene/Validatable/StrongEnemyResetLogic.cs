using System.Collections.Generic;
using MegaPint.ValidationRequirement;
using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class StrongEnemyResetLogic : ResetObjectLogic
{
    public override void ResetLogic()
    {
        gameObject.GetComponent<StrongEnemy>().SetHealth(0);
        
        ValidatableMonoBehaviour[] vmbs = gameObject.GetComponents <ValidatableMonoBehaviour>();
        
        foreach (var vmb in vmbs)
        {
            vmb.SetRequirements(new List <ScriptableValidationRequirement>());
            vmb.ClearImportedSettings();
        }
    }
}

}
