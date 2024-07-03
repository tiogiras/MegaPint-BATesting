#if UNITY_EDITOR
#if UNITY_INCLUDE_TESTS
using MegaPint.Editor.Scripts.Tests.Utility;
using MegaPint.Editor.Scripts.Windows;
using NUnit.Framework;

namespace MegaPint.Editor.Scripts.Tests
{

/// <summary> Unit tests regarding the menuItems of the package </summary>
internal class MenuItemTests
{
    #region Tests

    [Test]
    public void Overview()
    {
        TestsUtility.ValidateMenuItemLink(Constants.BaTesting.Links.Overview, typeof(Overview));
    }

    [Test]
    public void TaskManager()
    {
        TestsUtility.ValidateMenuItemLink(Constants.BaTesting.Links.TaskManager, typeof(TaskManager));
    }
    
    #endregion
}

}
#endif
#endif
