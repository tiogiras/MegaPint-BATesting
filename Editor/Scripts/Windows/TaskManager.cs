#if UNITY_EDITOR
using MegaPint.Editor.Scripts;
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.Windows;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows
{

/// <summary> Window based on the <see cref="EditorWindowBase" /> to display and handle the autoSave </summary>
internal class TaskManager : EditorWindowBase
{
    private VisualTreeAsset _baseWindow;
    private VisualTreeAsset _requirementItem;

    #region Public Methods

    /// <summary> Show the window </summary>
    /// <returns> Window instance </returns>
    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "Task Manager";
        
        // TODO add minSize
        
        if (!SaveValues.BATesting.ApplyPSTaskManager)
            return this;

        this.CenterOnMainWin(); // TODO add preffered size
        SaveValues.BATesting.ApplyPSTaskManager = false;

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return Constants.BATesting.UserInterface.TaskManager;
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = GUIUtility.Instantiate(_baseWindow, root);
        content.style.flexGrow = 1f;
        content.style.flexShrink = 1f;

        RegisterCallbacks();
    }

    protected override bool LoadResources()
    {
        _baseWindow = Resources.Load <VisualTreeAsset>(BasePath());
        _requirementItem = Resources.Load <VisualTreeAsset>(Constants.BATesting.UserInterface.Requirement);

        return _baseWindow != null && _requirementItem != null;
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
