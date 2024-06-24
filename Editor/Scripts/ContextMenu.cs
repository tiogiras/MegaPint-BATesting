#if UNITY_EDITOR
using MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows;
using UnityEditor;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class used to store MenuItems </summary>
internal static partial class ContextMenu
{
    #region Private Methods

    [MenuItem(MenuItemPackages + "/BA Testing" + "/Task Manager", false, 100)]
    private static void OpenTaskManager()
    {
        TryOpen <TaskManager>(false);
    }

    #endregion
}

}
#endif
