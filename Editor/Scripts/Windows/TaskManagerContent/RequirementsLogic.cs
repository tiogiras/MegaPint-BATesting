// TODO Commenting

#if UNITY_EDITOR
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class RequirementsLogic
{
    #region Public Methods

    public static void ExecuteRequirement(Requirement requirement)
    {
        RequirementLogicLookUp.Logic logic = RequirementLogicLookUp.LookUp[requirement.requirementName];

        if (logic.openRequirementInformation)
        {
            ContextMenu.OpenRequirementInformation();

            RequirementInformation.AfterGUICreation(
                Resources.Load <VisualTreeAsset>(RequirementGUILookUp.LookUp[requirement.requirementName]),
                logic.action);
        }
        else
            logic.action?.Invoke(null);
    }

    #endregion
}

}
#endif
