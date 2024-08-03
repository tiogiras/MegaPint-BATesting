#if UNITY_EDITOR
using MegaPint.Editor.Scripts.GUI.Utility;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class used to display the right pane in the BaseWindow </summary>
internal static partial class DisplayContent
{
    #region Private Methods

    // Called by reflection
    // ReSharper disable once UnusedMember.Local
    private static void BATesting(DisplayContentReferences refs)
    {
        InitializeDisplayContent(
            refs,
            new TabSettings {info = true},
            new TabActions
            {
                info = root =>
                {
                    root.ActivateLinks(
                        evt =>
                        {
                            switch (evt.linkID)
                            {
                                case "taskManager":
                                    ContextMenu.BATesting.OpenTaskManager();

                                    break;

                                case "taskOverview":
                                    ContextMenu.BATesting.OpenOverview();

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
