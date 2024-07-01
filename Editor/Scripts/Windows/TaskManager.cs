// TODO commenting

#if UNITY_EDITOR
using System.Collections.Generic;
using MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows.TaskManagerContent;
using MegaPint.Editor.Scripts;
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.Windows;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows
{

/// <summary> Window based on the <see cref="EditorWindowBase" /> to display the current ba testing tasks </summary>
internal class TaskManager : EditorWindowBase
{
    private VisualTreeAsset _baseWindow;
    private VisualTreeAsset _requirementItem;

    private TaskManagerData _data;

    private Label _lastTaskIndex;
    private Label _currentTaskIndex;
    private Label _taskTitle;
    private Label _taskInfo;
    private Label _chapter;
    
    private VisualElement _requirementsContainer;
    private ListView _requirementsList;

    private Button _btnContinue;
    private Button _btnStart;

    #region Public Methods

    /// <summary> Show the window </summary>
    /// <returns> Window instance </returns>
    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "Task Manager";
        
        // TODO add minSize
        
        if (!SaveValues.BaTesting.ApplyPSTaskManager)
            return this;

        this.CenterOnMainWin(); // TODO add preffered size
        SaveValues.BaTesting.ApplyPSTaskManager = false;

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return Constants.BaTesting.UserInterface.TaskManager;
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = GUIUtility.Instantiate(_baseWindow, root);
        content.style.flexGrow = 1f;
        content.style.flexShrink = 1f;

        _lastTaskIndex = content.Q <Label>("LastTaskIndex");
        _currentTaskIndex = content.Q <Label>("CurrentTaskIndex");
        _taskTitle = content.Q <Label>("TaskTitle");
        _taskInfo = content.Q <Label>("TaskInfo");
        _chapter = content.Q <Label>("Chapter");
        
        _requirementsContainer = content.Q <VisualElement>("RequirementsParent");
        _requirementsList = content.Q <ListView>("Requirements");

        _btnContinue = content.Q <Button>("BTN_Continue");
        _btnStart = content.Q <Button>("BTN_Start");

        UpdateTaskManager();

        RegisterCallbacks();
    }

    private void UpdateTaskManager()
    {
        _lastTaskIndex.text = $"/{_data.TasksCount}";
        _currentTaskIndex.text = (_data.currentTaskIndex + 1).ToString();

        Task currentTask = _data.CurrentTask();
        _taskTitle.text = currentTask.taskName;
        _taskInfo.text = currentTask.taskDescription;

        _chapter.text = currentTask.chapter.chapterName;

        var hasRequirements = currentTask.taskRequirements.Count > 0;
        _requirementsContainer.style.display = hasRequirements ? DisplayStyle.Flex : DisplayStyle.None;

        _btnContinue.style.display = currentTask.hasDoableTask ? DisplayStyle.None : DisplayStyle.Flex;
        _btnStart.style.display = currentTask.hasDoableTask ? DisplayStyle.Flex : DisplayStyle.None;
    }

    protected override bool LoadResources()
    {
        _baseWindow = Resources.Load <VisualTreeAsset>(BasePath());
        _requirementItem = Resources.Load <VisualTreeAsset>(Constants.BaTesting.UserInterface.Requirement);

        _data = Resources.Load <TaskManagerData>(Constants.BaTesting.TaskManagerData);

        return _baseWindow != null && _requirementItem != null && _data != null;
    }

    protected override void RegisterCallbacks()
    {
        _btnContinue.clicked += OnContinue;
    }

    protected override void UnRegisterCallbacks()
    {
        _btnContinue.clicked -= OnContinue;
    }

    private void OnContinue()
    {
        _data.currentTaskIndex++;
        
        UpdateTaskManager();
    }

    #endregion
}

}
#endif
