#if UNITY_EDITOR
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.PackageManager.Packages;
using UnityEditor;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class used to display the right pane in the BaseWindow </summary>
internal static partial class DisplayContent
{
    #region Private Methods

    // Called by reflection
    // ReSharper disable once UnusedMember.Local
    private static void AlphaButton(DisplayContentReferences refs)
    {
        InitializeDisplayContent(
            refs,
            new TabSettings {info = true, help = true},
            new TabActions
            {
                info = root =>
                {
                    root.ActivateLinks(
                        evt =>
                        {
                            switch (evt.linkID)
                            {
                                case "integration":
                                    Windows.PackageManager.OpenPerLink(PackageKey.AlphaButton);

                                    break;
                            }
                        });
                },
                help = root =>
                {
                    root.ActivateLinks(
                        evt =>
                        {
                            switch (evt.linkID)
                            {
                                case "packageManager":
                                    EditorApplication.ExecuteMenuItem(Constants.BasePackage.Links.PackageManager);

                                    break;
                            }
                        });
                }
            });
    }

    #endregion
}

}
#endif
