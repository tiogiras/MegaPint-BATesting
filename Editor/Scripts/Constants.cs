﻿using System.IO;

#if UNITY_EDITOR
namespace MegaPint.Editor.Scripts
{

/// <summary> Partial lookup table for constants containing AlphaButton values  </summary>
internal static partial class Constants
{
    public static class BATesting
    {
        public static class Links
        {

        }

        public static class UserInterface
        {
            private static readonly string s_windows = Path.Combine(s_userInterface, "Windows");
            public static readonly string Overview = Path.Combine(s_windows, "Overview");
            public static readonly string TaskManager = Path.Combine(s_windows, "Task Manager");
            public static readonly string Requirement = Path.Combine(TaskManager, "Requirement");
        }

        private static readonly string s_base = Path.Combine("MegaPint", "BATesting");
        private static readonly string s_userInterface = Path.Combine(s_base, "User Interface");
    }
}

}
#endif
