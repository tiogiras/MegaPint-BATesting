// TODO commenting

#if UNITY_EDITOR
using System.Linq;
using MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows.TaskManagerContent;
using MegaPint.Editor.Scripts;
using MegaPint.Editor.Scripts.GUI;
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.Windows;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows
{

/// <summary> Window based on the <see cref="EditorWindowBase" /> to display the current ba testing tasks </summary>
internal class TaskManager : EditorWindowBase
{
    private VisualTreeAsset _baseWindow;

    private Button _btnContinue;
    private Button _btnStart;
    private Label _chapter;
    private Label _currentTaskIndex;

    private TaskManagerData _data;

    private Label _lastTaskIndex;
    private VisualTreeAsset _requirementItem;

    private VisualElement _requirementsContainer;
    private ListView _requirementsList;
    private Label _taskInfo;
    private Label _taskTitle;

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

        _requirementsList.makeItem = () => GUIUtility.Instantiate(_requirementItem);

        _requirementsList.bindItem = (element, i) =>
        {
            var requirement = _requirementsList.itemsSource[i] as Requirement;

            if (requirement == null)
                return;

            element.Q <Label>("Title").text = requirement.requirementName;

            var container = element.Q <VisualElement>("Container");
            UpdateRequirementContainer(container, requirement.done);
            
            var toggle = element.Q <Toggle>("MarkReady");
            toggle.SetValueWithoutNotify(requirement.done);

            toggle.RegisterValueChangedCallback(
                evt =>
                {
                    requirement.done = evt.newValue;
                    UpdateRequirementContainer(container, evt.newValue);
                    UpdateButtons();

                    EditorUtility.SetDirty(requirement);
                });

            element.Q <Button>("BTN_GoTo").clickable = new Clickable(
                () => {RequirementsLogic.ExecuteRequirement(requirement);});
        };
    }

    private void UpdateButtons()
    {
        var missingRequirements = _data.CurrentTask().taskRequirements.Any(requirement => !requirement.done);

        _btnStart.pickingMode = missingRequirements ? PickingMode.Ignore : PickingMode.Position;
        _btnStart.style.opacity = missingRequirements ? .5f : 1f;
        
        _btnContinue.pickingMode = missingRequirements ? PickingMode.Ignore : PickingMode.Position;
        _btnContinue.style.opacity = missingRequirements ? .5f : 1f;
    }
    
    private static void UpdateRequirementContainer(VisualElement container, bool done)
    {
        if (done)
        {
            container.AddToClassList(StyleSheetClasses.Background.Color.Green);
            container.RemoveFromClassList(StyleSheetClasses.Background.Color.Red);
        }
        else
        {
            container.AddToClassList(StyleSheetClasses.Background.Color.Red);
            container.RemoveFromClassList(StyleSheetClasses.Background.Color.Green);
        }
    }

    protected override void UnRegisterCallbacks()
    {
        _btnContinue.clicked -= OnContinue;
    }

    #endregion

    #region Private Methods

    private void OnContinue()
    {
        if (_data.currentTaskIndex >= _data.TasksCount - 1)
        {
            Debug.Log("No More Tasks");
            return;
        }

        _data.currentTaskIndex++;
        UpdateTaskManager();
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
        
        UpdateButtons();

        if (hasRequirements)
            _requirementsList.itemsSource = currentTask.taskRequirements;
    }

    #endregion
}

}
#endif
