// TODO commenting

#if UNITY_EDITOR
using System;
using System.Linq;
using System.Threading.Tasks;
using MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows.TaskManagerContent;
using MegaPint.Editor.Scripts;
using MegaPint.Editor.Scripts.GUI;
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.Windows;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;
using Task = System.Threading.Tasks.Task;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows
{

/// <summary> Window based on the <see cref="EditorWindowBase" /> to display the current ba testing tasks </summary>
internal class TaskManager : EditorWindowBase
{
    private VisualTreeAsset _baseWindow;
    private Button _btnComplete;

    private Button _btnContinue;
    private Button _btnPause;
    private Button _btnResume;
    private Button _btnStart;

    private Label _chapter;
    private Label _currentTaskIndex;

    private TaskManagerData _data;
    private VisualTreeAsset _goalItem;

    private VisualElement _goalsContainer;
    private ListView _goalsList;

    private Label _lastTaskIndex;

    private bool _pauseTimer;
    private VisualTreeAsset _requirementItem;

    private VisualElement _requirementsContainer;
    private ListView _requirementsList;

    private Label _taskInfo;
    private Label _taskTitle;
    private Label _timer;

    private VisualElement _timerContainer;

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
        _requirementsList = _requirementsContainer.Q <ListView>("Requirements");

        _goalsContainer = content.Q <VisualElement>("GoalsParent");
        _goalsList = _goalsContainer.Q <ListView>("Goals");

        _btnContinue = content.Q <Button>("BTN_Continue");
        _btnStart = content.Q <Button>("BTN_Start");
        _btnComplete = content.Q <Button>("BTN_Complete");

        _timerContainer = content.Q <VisualElement>("TimerContainer");
        _timer = _timerContainer.Q <Label>("Timer");
        _btnPause = _timerContainer.Q <Button>("BTN_Pause");
        _btnResume = _timerContainer.Q <Button>("BTN_Resume");

        UpdateTaskManager();

        RegisterCallbacks();
    }

    protected override bool LoadResources()
    {
        _baseWindow = Resources.Load <VisualTreeAsset>(BasePath());
        _requirementItem = Resources.Load <VisualTreeAsset>(Constants.BaTesting.UserInterface.Requirement);
        _goalItem = Resources.Load <VisualTreeAsset>(Constants.BaTesting.UserInterface.Goal);

        _data = Resources.Load <TaskManagerData>(Constants.BaTesting.TaskManagerData);

        return _baseWindow != null && _requirementItem != null && _data != null && _goalItem != null;
    }

    protected override void RegisterCallbacks()
    {
        GoalsLogic.onGoalDone += OnGoalDone;
        
        _btnContinue.clicked += OnContinue;
        _btnStart.clicked += OnStart;

        _btnPause.clicked += PauseTimer;
        _btnResume.clicked += StartTimer;

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

        _goalsList.makeItem = () => GUIUtility.Instantiate(_goalItem);

        _goalsList.bindItem = (element, i) =>
        {
            var goal = _goalsList.itemsSource[i] as Goal;

            if (goal == null)
                return;

            element.Q <Label>("Title").text = goal.title;
            element.Q <Label>("Hint").tooltip = goal.hint;

            UpdateRequirementContainer(element.Q <VisualElement>("Container"), goal.done);
        };
    }

    private void OnGoalDone(Goal _)
    {
        _goalsList.RefreshItems();
        UpdateGoalButton();
        
        if (_data.CurrentTask().goals.All(goal => goal.done))
            PauseTimer();
    }

    protected override void UnRegisterCallbacks()
    {
        _btnContinue.clicked -= OnContinue;
        _btnStart.clicked -= OnStart;

        _btnPause.clicked -= PauseTimer;
        _btnResume.clicked -= StartTimer;
    }

    #endregion

    #region Private Methods

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

    private void OnStart()
    {
        SceneAsset scene = _data.CurrentTask().scene;

        if (scene != null)
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scene));

        OnContinue();
    }

    private void PauseTimer()
    {
        _pauseTimer = true;

        _timer.style.opacity = .5f;

        _btnPause.style.display = DisplayStyle.None;
        _btnResume.style.display = DisplayStyle.Flex;
    }

    private void StartTimer()
    {
        if (_data.CurrentTask().goals.All(goal => goal.done))
            return;
        
        _pauseTimer = false;

        _timer.style.opacity = 1f;

        _btnPause.style.display = DisplayStyle.Flex;
        _btnResume.style.display = DisplayStyle.None;

        Timer();
    }

    private async void Timer()
    {
        while (this != null)
        {
            Debug.Log("Timer");

            if (!await TryWaitOneSecond())
                break;

            _data.CurrentTask().neededTime++;

            UpdateTimerText();
        }
    }

    private async Task <bool> TryWaitOneSecond()
    {
        for (var i = 0; i < 10; i++)
        {
            if (_pauseTimer)
                return false;

            await Task.Delay(100);
        }

        return true;
    }

    private void UpdateButtons()
    {
        var missingRequirements = _data.CurrentTask().taskRequirements.Any(requirement => !requirement.done);

        _btnStart.pickingMode = missingRequirements ? PickingMode.Ignore : PickingMode.Position;
        _btnStart.style.opacity = missingRequirements ? .5f : 1f;

        _btnContinue.pickingMode = missingRequirements ? PickingMode.Ignore : PickingMode.Position;
        _btnContinue.style.opacity = missingRequirements ? .5f : 1f;
    }

    private void UpdateGoalButton()
    {
        var unCompletedGoals = _data.CurrentTask().goals.Any(goal => !goal.done);

        _btnComplete.pickingMode = unCompletedGoals ? PickingMode.Ignore : PickingMode.Position;
        _btnComplete.style.opacity = unCompletedGoals ? .5f : 1f;
    }

    private void UpdateTaskManager()
    {
        _lastTaskIndex.text = $"/{_data.TasksCount}";
        _currentTaskIndex.text = (_data.currentTaskIndex + 1).ToString();

        TaskManagerContent.Task currentTask = _data.CurrentTask();
        _taskTitle.text = currentTask.taskName;
        _taskInfo.text = currentTask.taskDescription;

        _chapter.text = currentTask.chapter.chapterName;

        var hasRequirements = currentTask.taskRequirements.Count > 0;
        _requirementsContainer.style.display = hasRequirements ? DisplayStyle.Flex : DisplayStyle.None;

        var hasGoals = currentTask.goals.Count > 0;
        _goalsContainer.style.display = hasGoals ? DisplayStyle.Flex : DisplayStyle.None;

        if (!currentTask.hasDoableTask && !hasGoals)
            _btnContinue.style.display = DisplayStyle.Flex;
        else
            _btnContinue.style.display = DisplayStyle.None;

        if (currentTask.hasDoableTask && !hasGoals)
            _btnStart.style.display = DisplayStyle.Flex;
        else
            _btnStart.style.display = DisplayStyle.None;

        _btnComplete.style.display = hasGoals ? DisplayStyle.Flex : DisplayStyle.None;
        _timerContainer.style.display = hasGoals ? DisplayStyle.Flex : DisplayStyle.None;

        UpdateButtons();
        UpdateGoalButton();

        if (hasRequirements)
            _requirementsList.itemsSource = currentTask.taskRequirements;

        if (!hasGoals)
            return;

        _goalsList.itemsSource = currentTask.goals;

        UpdateTimerText();
        StartTimer();
    }

    private void UpdateTimerText()
    {
        _timer.text = TimeSpan.FromSeconds(_data.CurrentTask().neededTime).ToString();
    }

    #endregion
}

}
#endif
