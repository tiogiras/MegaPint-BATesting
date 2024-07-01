#if UNITY_EDITOR
using System.IO;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial lookup table for constants containing AlphaButton values  </summary>
internal static partial class Constants
{
    public static class BaTesting
    {
        public static class Links
        {
            private static readonly string s_baTestingItems =
                Utility.CombineMenuItemPath(ContextMenu.MenuItemPackages, "BA Testing");

            public static readonly string Overview = Utility.CombineMenuItemPath(s_baTestingItems, "Overview");
            public static readonly string TaskManager = Utility.CombineMenuItemPath(s_baTestingItems, "Task Manager");
        }

        public static class UserInterface
        {
            public static class Requirements
            {
                private static readonly string s_requirements = Path.Combine(TaskManager, "Requirements");
                public static readonly string TestRequirement = Path.Combine(s_requirements, "Test Requirement");
            }

            private static readonly string s_windows = Path.Combine(s_userInterface, "Windows");
            public static readonly string Overview = Path.Combine(s_windows, "Overview");
            public static readonly string InvalidToken = Path.Combine(s_windows, "Invalid Token");
            public static readonly string TaskManager = Path.Combine(s_windows, "Task Manager");
            public static readonly string Requirement = Path.Combine(TaskManager, "Requirement");
        }

        private static readonly string s_base = Path.Combine("MegaPint", "BATesting");
        private static readonly string s_scriptableObjects = Path.Combine(s_base, "Scriptable Objects");
        public static readonly string TaskManagerData = Path.Combine(s_scriptableObjects, "TaskManagerData");

        private static readonly string s_userInterface = Path.Combine(s_base, "User Interface");
    }
}

}
#endif
