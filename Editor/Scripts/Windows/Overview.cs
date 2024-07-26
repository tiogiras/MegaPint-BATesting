// TODO commenting

#if UNITY_EDITOR
using System;
using System.Linq;
using MegaPint.Editor.Scripts.GUI;
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.Editor.Scripts.Windows
{

internal class Overview : EditorWindowBase
{
    public static Action onOpen;
    public static Action onClose;
    public static Action<float[]> onSend;
    public static Action onSendComplete;
    
    private VisualTreeAsset _baseWindow;
    private Button _btnResetAll;
    private Button _btnSend;

    private TaskManagerData _data;
    private ProgressBar _progress;
    private VisualTreeAsset _taskItem;

    private ListView _tasksView;

    #region Public Methods

    /// <summary> Show the window </summary>
    /// <returns> Window instance </returns>
    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "Test Overview";

        // TODO minsize
        // TODO preferred size

        onOpen?.Invoke();
        
        this.CenterOnMainWin();

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return Constants.BaTesting.UserInterface.Overview;
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = GUIUtility.Instantiate(_baseWindow, root);
        content.style.flexGrow = 1f;
        content.style.flexShrink = 1f;

        _tasksView = content.Q <ListView>("Tasks");
        _progress = content.Q <ProgressBar>("Progress");
        _btnResetAll = content.Q <Button>("BTN_ResetAll");
        _btnSend = content.Q <Button>("BTN_Send");

        RegisterCallbacks();

        _tasksView.itemsSource = _data.Tasks;

        Refresh(null);
    }

    protected override bool LoadResources()
    {
        _baseWindow = Resources.Load <VisualTreeAsset>(BasePath());
        _taskItem = Resources.Load <VisualTreeAsset>(Constants.BaTesting.UserInterface.Task);
        _data = Resources.Load <TaskManagerData>(Constants.BaTesting.TaskManagerData);

        return _baseWindow != null && _taskItem != null && _data != null;
    }

    protected override void RegisterCallbacks()
    {
        Task.onTaskDoneChange += Refresh;

        _btnResetAll.clicked += OnResetAll;
        _btnSend.clicked += OnSend;

        _tasksView.makeItem = () => GUIUtility.Instantiate(_taskItem);

        _tasksView.bindItem = (element, i) =>
        {
            var task = _tasksView.itemsSource[i] as Task;

            if (task == null)
                return;

            var container = element.Q <VisualElement>("Container");

            element.Q <Label>("Title").text = task.taskName;

            var btnReset = element.Q <Button>("BTN_Reset");

            if (task.Done || _data.CurrentTaskIndex == i)
            {
                btnReset.style.display = DisplayStyle.Flex;

                btnReset.clickable = new Clickable(
                    () =>
                    {
                        var index = _data.Tasks.IndexOf(task);

                        if (index < _data.CurrentTaskIndex)
                            _data.CurrentTaskIndex = index;

                        if (!EditorUtility.DisplayDialog(
                                "Reset Task",
                                "Are you sure you want to reset this task?\nThis action may be permanent and you will all of your progress regarding this task.",
                                "Yes",
                                "Cancel"))
                            return;
                        
                        task.ResetValues();
                        UpdateListElementContainer(container, task.Done, i);
                    });
            }
            else
                btnReset.style.display = DisplayStyle.None;
            
            UpdateListElementContainer(container, task.Done, i);
        };

        onSendComplete += ReenableSendButton;
    }

    private void ReenableSendButton()
    {
        _btnSend.pickingMode = PickingMode.Position;
        _btnSend.style.opacity = 1f;
    }

    protected override void UnRegisterCallbacks()
    {
        Task.onTaskDoneChange -= Refresh;

        _btnResetAll.clicked -= OnResetAll;
        _btnSend.clicked -= OnSend;
        
        onClose?.Invoke();
        
        onSendComplete -= ReenableSendButton;
    }

    #endregion

    #region Private Methods

    private void OnResetAll()
    {
        if (EditorUtility.DisplayDialog(
                "Reset All Tasks",
                "Are you sure you want to reset all tasks?\nThis action may be permanent and you will lose all of your progress.",
                "Yes",
                "Cancel"))
            _data.ResetValues();
    }

    private void OnSend()
    {
        var times = new float[_data.TasksCount];

        for (var i = 0; i < _data.Tasks.Count; i++)
        {
            Task task = _data.Tasks[i];
            times[i] = task.NeededTime;
        }

        onSend?.Invoke(times);
        
        _btnSend.pickingMode = PickingMode.Ignore;
        _btnSend.style.opacity = 0.5f;
    }

    private void Refresh(Task _)
    {
        _tasksView.RefreshItems();
        
        UpdateProgressBar();
        UpdateSendButton();
    }

    private void UpdateListElementContainer(VisualElement element, bool done, int index)
    {
        if (done)
        {
            element.AddToClassList(StyleSheetClasses.Background.Color.Green);
            element.RemoveFromClassList(StyleSheetClasses.Background.Color.Red);
            element.RemoveFromClassList(StyleSheetClasses.Background.Color.Orange);
        }
        else if (index == _data.CurrentTaskIndex)
        {
            element.AddToClassList(StyleSheetClasses.Background.Color.Orange);
            element.RemoveFromClassList(StyleSheetClasses.Background.Color.Red);
            element.RemoveFromClassList(StyleSheetClasses.Background.Color.Green);
        }
        else
        {
            element.AddToClassList(StyleSheetClasses.Background.Color.Red);
            element.RemoveFromClassList(StyleSheetClasses.Background.Color.Orange);
            element.RemoveFromClassList(StyleSheetClasses.Background.Color.Green);
        }
    }

    private void UpdateProgressBar()
    {
        var completedTasks = _data.Tasks.Count(task => task.Done);
        var progress = (float)completedTasks / _data.Tasks.Count;

        _progress.value = progress;
    }

    private void UpdateSendButton()
    {
        var hasUncompletedTasks = _data.Tasks.ToArray()[..^1].Any(task => !task.Done);

        _btnSend.pickingMode = hasUncompletedTasks ? PickingMode.Ignore : PickingMode.Position;
        _btnSend.style.opacity = hasUncompletedTasks ? 0.5f : 1f;
    }

    #endregion
}

}
#endif
