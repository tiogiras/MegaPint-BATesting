// TODO Commenting

#if UNITY_EDITOR
using System.Collections.Generic;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class RequirementGUILookUp
{
    public static readonly Dictionary <string, string> LookUp = new()
    {
        {
            "Test Requirement",
            Constants.BaTesting.UserInterface.Requirements.TestRequirement
        }
    };
}

}
#endif
