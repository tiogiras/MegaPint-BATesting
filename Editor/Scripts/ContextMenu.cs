#if UNITY_EDITOR
using MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows;
using MegaPint.Editor.Scripts.Windows;
using UnityEditor;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class used to store MenuItems </summary>
internal static partial class ContextMenu
{
    #region Private Methods

    [MenuItem(MenuItemPackages + "/BA Testing" + "/Task Manager &t", false, 101)]
    private static void OpenTaskManager()
    {
        TryToOpenWithValidToken<TaskManager>(false);
    }
    
    [MenuItem(MenuItemPackages + "/BA Testing" + "/Test Overview &o", false, 100)]
    private static void OpenOverview()
    {
        TryToOpenWithValidToken<Overview>(false);
    }

    // TODO commenting
    public static void OpenRequirementInformation()
    {
        TryToOpenWithValidToken<RequirementInformation>(false);
    }

    /// <summary> Open the editor window when the tester token is valid if not open the invalid token window </summary>
    /// <param name="utility"> Targeted utility state of the window </param>
    /// <typeparam name="T"> Targeted window type </typeparam>
    private static void TryToOpenWithValidToken <T>(bool utility) where T : EditorWindowBase
    {
        if (Utility.ValidateTesterToken())
            TryOpen <T>(utility);
        else
            TryOpen <InvalidToken>(true);
    }

    #endregion
}

}
#endif
