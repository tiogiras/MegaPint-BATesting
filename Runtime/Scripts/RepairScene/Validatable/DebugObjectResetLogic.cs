using System.Collections.Generic;
using MegaPint.ValidationRequirement;

namespace MegaPint.RepairScene.Validatable
{

internal class DebugObjectResetLogic : ResetObjectLogic
{
    public override void ResetLogic()
    {
        gameObject.SetActive(true);

        ValidatableMonoBehaviour[] vmbs = gameObject.GetComponents <ValidatableMonoBehaviour>();
        
        foreach (var vmb in vmbs)
        {
            vmb.SetRequirements(new List <ScriptableValidationRequirement>());
            vmb.ClearImportedSettings();
        }
    }
}

}
