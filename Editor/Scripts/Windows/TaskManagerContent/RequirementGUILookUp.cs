#if UNITY_EDITOR
using System.Collections.Generic;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

/// <summary> Look up for the requirements GUI </summary>
internal static class RequirementGUILookUp
{
    public static readonly Dictionary <string, string> LookUp = new()
    {
        {"Test Requirement", Constants.BaTesting.UserInterface.Requirements.TestRequirement},
        {"MenuItems", Constants.BaTesting.UserInterface.Requirements.MenuItems},
        {"Upload Screenshots I", Constants.BaTesting.UserInterface.Requirements.UploadScreenshots1},
        {"Upload Screenshots II", Constants.BaTesting.UserInterface.Requirements.UploadScreenshots2},
        {"Upload Screenshots III", Constants.BaTesting.UserInterface.Requirements.UploadScreenshots3},
        {"Screenshot Survey", Constants.BaTesting.UserInterface.Requirements.ScreenshotSurvey},
        {"Validators Survey", Constants.BaTesting.UserInterface.Requirements.ValidatorsSurvey},
        {"General Survey", Constants.BaTesting.UserInterface.Requirements.GeneralSurvey}
    };
}

}
#endif
