﻿#if UNITY_EDITOR
using System;
using System.Threading.Tasks;
using MegaPint.Editor.Scripts.GUI.Utility;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.Editor.Scripts.Windows
{

/// <summary>
///     Window based on the <see cref="EditorWindowBase" /> to be displayed whenever an window tries to open but the
///     tester token is invalid
/// </summary>
internal class RequirementInformation : EditorWindowBase
{
    private static VisualElement s_root;

    #region Public Methods

    public static async void AfterGUICreation(VisualTreeAsset template, Action <VisualElement> action)
    {
        while (s_root == null)
            await Task.Delay(100);

        s_root.Clear();

        VisualElement content = GUIUtility.Instantiate(template, s_root);
        content.style.flexGrow = 1f;
        content.style.flexShrink = 1f;

        s_root.schedule.Execute(
            () => {action?.Invoke(content);});
    }

    /// <summary> Show the window </summary>
    /// <returns> Window instance </returns>
    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "Requirement Information";

        minSize = new Vector2(200, 150);

        if (!SaveValues.BaTesting.ApplyPSRequirement)
        {
            this.CenterOnMainWin();

            return this;
        }

        this.CenterOnMainWin(500, 250);
        SaveValues.BaTesting.ApplyPSRequirement = false;

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return null;
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        s_root = rootVisualElement;

        RegisterCallbacks();
    }

    protected override bool LoadResources()
    {
        return true;
    }

    protected override void RegisterCallbacks()
    {
    }

    protected override void UnRegisterCallbacks()
    {
    }

    #endregion
}

}
#endif
