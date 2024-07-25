// TODO commenting

#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MegaPint.Editor.Scripts.GUI;
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;
using Object = UnityEngine.Object;
using Task = MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data.Task;

namespace MegaPint.Editor.Scripts.Windows
{

/// <summary> Window based on the <see cref="EditorWindowBase" /> to display the current ba testing tasks </summary>
internal class TaskManager : EditorWindowBase
{
    public static Action onOpen;
    public static Action onClose;

    public static Action <string> onNext;
    public static Action onStartTimer;
    public static Action onStopTimer;
    public static Action <string> onHint;
    public static Action <string> onStartTask;

    public static Action <string> onStartTaskLogging;
    public static Action onStopTaskLogging;

    public static Action onForceFinishTask;

    private VisualTreeAsset _baseWindow;
    private Button _btnComplete;

    private Button _btnContinue;
    private Button _btnFinishTask;
    private Button _btnPause;
    private Button _btnReloadTaskScene;
    private Button _btnResume;
    private Button _btnStart;

    private Label _chapter;
    private Label _currentTaskIndex;

    private TaskManagerData _data;
    private VisualTreeAsset _goalItem;

    private VisualElement _goalsContainer;
    private ListView _goalsList;

    private ScrollView _infoField;

    private Label _lastTaskIndex;

    private bool _pauseTimer;
    private VisualTreeAsset _requirementItem;

    private VisualElement _requirementsContainer;
    private ListView _requirementsList;

    private Label _taskInfo;
    private Label _taskTitle;
    private Label _timer;

    private VisualElement _timerContainer;

    #region Unity Event Functions

    private void OnEnable()
    {
        if (docked)
            Focus();
    }

    #endregion

    #region Public Methods

    /// <summary> Show the window </summary>
    /// <returns> Window instance </returns>
    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "Task Manager";

        // TODO add minSize

        onOpen?.Invoke();

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
        _btnReloadTaskScene = content.Q <Button>("BTN_ReloadTaskScene");
        _btnFinishTask = content.Q <Button>("BTN_FinishTask");
        _btnComplete = content.Q <Button>("BTN_Complete");

        _timerContainer = content.Q <VisualElement>("TimerContainer");
        _timer = _timerContainer.Q <Label>("Timer");
        _btnPause = _timerContainer.Q <Button>("BTN_Pause");
        _btnResume = _timerContainer.Q <Button>("BTN_Resume");

        _infoField = content.Q <ScrollView>("InfoField");

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
        Goal.onGoalDone += OnGoalDone;

        _btnContinue.clicked += OnContinue;
        _btnComplete.clicked += OnContinue;
        _btnReloadTaskScene.clicked += LoadTaskScene;
        _btnFinishTask.clicked += FinishTask;
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
            UpdateRequirementContainer(container, requirement.Done);

            var toggle = element.Q <Toggle>("MarkReady");
            toggle.SetValueWithoutNotify(requirement.Done);

            toggle.RegisterValueChangedCallback(
                evt =>
                {
                    requirement.Done = evt.newValue;
                    UpdateRequirementContainer(container, evt.newValue);
                    UpdateButtons();
                });

            element.Q <Button>("BTN_GoTo").clickable = new Clickable(
                () => {Requirement.ExecuteRequirement(requirement);});
        };

        _goalsList.makeItem = () => GUIUtility.Instantiate(_goalItem);

        _goalsList.bindItem = (element, i) =>
        {
            var goal = _goalsList.itemsSource[i] as Goal;

            if (goal == null)
                return;

            goal.SetActive();

            element.Q <Label>("Title").text = goal.title;

            var hint = element.Q <Label>("Hint");
            hint.tooltip = goal.hint;

            hint.RegisterCallback <MouseEnterEvent>(_ => onHint?.Invoke(goal.hint));

            UpdateRequirementContainer(element.Q <VisualElement>("Container"), goal.Done);
        };
    }

    protected override void UnRegisterCallbacks()
    {
        onClose?.Invoke();

        Goal.onGoalDone -= OnGoalDone;

        _btnContinue.clicked -= OnContinue;
        _btnComplete.clicked -= OnContinue;
        _btnReloadTaskScene.clicked -= LoadTaskScene;
        _btnFinishTask.clicked -= FinishTask;
        _btnStart.clicked -= OnStart;

        _btnPause.clicked -= PauseTimer;
        _btnResume.clicked -= StartTimer;
    }

    #endregion

    #region Private Methods

    private static void TryInitializeGoals(Task task, bool initialize)
    {
        if (task.goals.Count == 0)
            return;

        Goal[] initializeGoals = task.goals.Where(goal => goal.hasInitializationLogic).ToArray();

        if (!initializeGoals.Any())
            return;

        foreach (Goal goal in initializeGoals)
        {
            if (initialize)
                GoalInitializationLogicLookUp.InitializationLookUp[goal.title]?.Invoke();
            else
                GoalInitializationLogicLookUp.DeInitializationLookUp[goal.title]?.Invoke();
        }
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

    private void FinishTask()
    {
        onForceFinishTask?.Invoke();

        _btnFinishTask.style.display = DisplayStyle.None;
    }

    private void LoadTaskScene()
    {
        if (EditorApplication.isPlaying)
            return;

        Task currentTask = _data.CurrentTask();
        SceneAsset scene = currentTask.scene;

        if (scene != null)
            OpenScene(currentTask.scene);

        if (currentTask.startInPlayMode && !currentTask.Done)
            EditorApplication.EnterPlaymode();
    }

    private void OnContinue()
    {
        if (_data.CurrentTaskIndex >= _data.TasksCount - 1)
        {
            Debug.Log("No More Tasks"); // TODO remove

            // TODO onFinish all event

            return;
        }

        Task task = _data.CurrentTask();

        _data.CurrentTaskIndex++;
        task.Done = true;
        UpdateTaskManager();

        onNext?.Invoke(task.taskName);
    }

    private void OnGoalDone(Goal _)
    {
        _goalsList.RefreshItems();
        UpdateGoalButton();

        if (_data.CurrentTask().goals.All(goal => goal.Done))
            PauseTimer();
    }

    private void OnStart()
    {
        Task nextTask = _data.NextTask();
        SceneAsset scene = nextTask.scene;

        if (scene != null)
        {
            OpenScene(scene);

            if (nextTask.startInPlayMode && !nextTask.Done)
                EditorApplication.EnterPlaymode();
        }

        OnContinue();

        onStartTask?.Invoke(_data.CurrentTask().taskName);
    }

    private void OpenScene(Object scene)
    {
        var path = AssetDatabase.GetAssetPath(scene);
        var fileName = Path.GetFileName(path);
        var newFileName = fileName[2..];

        EditorSceneManager.OpenScene(path.Replace(fileName, newFileName));
    }

    private void PauseTimer()
    {
        Task task = _data.CurrentTask();

        AssetDatabase.SaveAssetIfDirty(task);

        TryInitializeGoals(task, false);

        _pauseTimer = true;

        _timer.style.opacity = .5f;

        _btnPause.style.display = DisplayStyle.None;

        _btnResume.style.display =
            _data.CurrentTask().goals.All(goal => goal.Done) ? DisplayStyle.None : DisplayStyle.Flex;
    }

    private void StartTimer()
    {
        Task task = _data.CurrentTask();

        if (task.goals.All(goal => goal.Done))
            return;

        TryInitializeGoals(task, true);

        _pauseTimer = false;

        _timer.style.opacity = 1f;

        _btnPause.style.display = DisplayStyle.Flex;
        _btnResume.style.display = DisplayStyle.None;

        Timer();
    }

    private async void Timer()
    {
        onStartTimer?.Invoke();
        onStartTaskLogging?.Invoke(_data.CurrentTask().taskName);

        while (this != null)
        {
            if (!await TryWaitOneSecond())
                break;

            _data.CurrentTask().NeededTime++;

            UpdateTimerText();
        }

        Task task = _data.CurrentTask();

        AssetDatabase.SaveAssetIfDirty(task);

        TryInitializeGoals(task, false);

        onStopTaskLogging?.Invoke();
        onStopTimer?.Invoke();
    }

    private async Task <bool> TryWaitOneSecond()
    {
        for (var i = 0; i < 10; i++)
        {
            if (_pauseTimer)
                return false;

            await System.Threading.Tasks.Task.Delay(100);
        }

        return true;
    }

    private void UpdateButtons()
    {
        var missingRequirements = _data.CurrentTask().taskRequirements.Any(requirement => !requirement.Done);

        _btnStart.pickingMode = missingRequirements ? PickingMode.Ignore : PickingMode.Position;
        _btnStart.style.opacity = missingRequirements ? .5f : 1f;

        _btnContinue.pickingMode = missingRequirements ? PickingMode.Ignore : PickingMode.Position;
        _btnContinue.style.opacity = missingRequirements ? .5f : 1f;
    }

    private void UpdateGoalButton()
    {
        var unCompletedGoals = _data.CurrentTask().goals.Any(goal => !goal.Done);

        _btnComplete.pickingMode = unCompletedGoals ? PickingMode.Ignore : PickingMode.Position;
        _btnComplete.style.opacity = unCompletedGoals ? .5f : 1f;
    }

    private void UpdateTaskManager()
    {
        _lastTaskIndex.text = $"/{_data.TasksCount}";
        _currentTaskIndex.text = (_data.CurrentTaskIndex + 1).ToString();

        _infoField.scrollOffset = Vector2.zero;

        Task currentTask = _data.CurrentTask();
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

        _btnReloadTaskScene.style.display = currentTask.scene != null ? DisplayStyle.Flex : DisplayStyle.None;

        _btnFinishTask.style.display =
            currentTask.goals.Any(goal => !goal.Done) && currentTask.cannotBeFinishedAutomatically
                ? DisplayStyle.Flex
                : DisplayStyle.None;

        _btnComplete.style.display = hasGoals ? DisplayStyle.Flex : DisplayStyle.None;
        _timerContainer.style.display = hasGoals ? DisplayStyle.Flex : DisplayStyle.None;

        UpdateButtons();
        UpdateGoalButton();

        if (hasRequirements)
            _requirementsList.itemsSource = currentTask.taskRequirements;

        if (!hasGoals)
        {
            PauseTimer();

            return;
        }

        _goalsList.itemsSource = currentTask.goals;

        if (currentTask.goals.All(goal => goal.Done))
            PauseTimer();

        UpdateTimerText();
        StartTimer();
    }

    private void UpdateTimerText()
    {
        _timer.text = TimeSpan.FromSeconds(_data.CurrentTask().NeededTime).ToString();
    }

    #endregion
}

}
#endif
