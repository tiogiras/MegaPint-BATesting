﻿#if UNITY_EDITOR
using MegaPint.Editor.Scripts.PackageManager.Packages;
using MegaPint.Editor.Scripts.Settings;
using MegaPint.Editor.Scripts.Windows;
using UnityEditor;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class used to store MenuItems </summary>
internal static partial class ContextMenu
{
    public static class BATesting
    {
        private static readonly MenuItemSignature s_requirementInformationSignature = new()
        {
            package = PackageKey.BATesting, signature = "Requirement Information"
        };

        private static readonly MenuItemSignature s_overviewSignature = new()
        {
            package = PackageKey.BATesting, signature = "Task Overview"
        };

        private static readonly MenuItemSignature s_taskManagerSignature = new()
        {
            package = PackageKey.BATesting, signature = "Task Manager"
        };

        private static readonly MenuItemSignature s_invalidTokenSignature = new()
        {
            package = PackageKey.BATesting, signature = "Invalid Token"
        };

        #region Public Methods

        [MenuItem(MenuItemPackages + "/BA Testing" + "/Task Overview &o", false, 100)]
        public static void OpenOverview()
        {
            TryToOpenWithValidToken <Overview>(false, s_overviewSignature);
        }

        /// <summary> Open the requirement information window </summary>
        public static void OpenRequirementInformation()
        {
            TryToOpenWithValidToken <RequirementInformation>(false, s_requirementInformationSignature);
        }

        [MenuItem(MenuItemPackages + "/BA Testing" + "/Task Manager &t", false, 101)]
        public static void OpenTaskManager()
        {
            TryToOpenWithValidToken <TaskManager>(false, s_taskManagerSignature);
        }

        /// <summary> Opens the TermsAgreement editor window </summary>
        public static void OpenTermsAgreement()
        {
            EditorWindow.GetWindow <TermsAgreement>(true, "").ShowWindow();
        }

        #endregion

        #region Private Methods

        /// <summary> Open the editor window when the tester token is valid if not open the invalid token window </summary>
        /// <param name="utility"> Targeted utility state of the window </param>
        /// <param name="menuItemSignature"> Signature to identify this menuItem </param>
        /// <param name="title"> Title of the window </param>
        /// <typeparam name="T"> Targeted window type </typeparam>
        private static async void TryToOpenWithValidToken <T>(
            bool utility,
            MenuItemSignature menuItemSignature,
            string title = "") where T : EditorWindowBase
        {
            if (!MegaPintMainSettings.Exists())
            {
                EditorWindow.GetWindow <FirstSteps>(true, title).ShowWindow();

                return;
            }

            if (!SaveValues.BaTesting.AgreedToTerms)
            {
                OpenTermsAgreement();

                return;
            }

            if (await Utility.IsValidTesterToken())
                TryOpen <T>(utility, menuItemSignature, title);
            else
                TryOpen <InvalidToken>(true, s_invalidTokenSignature);
        }

        #endregion
    }
}

}
#endif
