#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

/// <summary> Holds data about a task </summary>
internal class Task : ScriptableObject
{
    public static Action <Task> onTaskDoneChange;

    public float NeededTime
    {
        get => _neededTime;
        set
        {
            _neededTime = value;
            EditorUtility.SetDirty(this);
        }
    }

    public bool Done
    {
        get => _done;
        set
        {
            _done = value;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);

            onTaskDoneChange?.Invoke(this);
        }
    }

    public Chapter chapter;
    public string taskName;
    [TextArea] public string taskDescription;
    public List <Requirement> taskRequirements;
    public bool hasDoableTask;
    public bool cannotBeFinishedAutomatically;
    public SceneAsset scene;
    public bool startInPlayMode;
    public List <Goal> goals;
    public List <ResetObjectLogic> resetObjects;

    [SerializeField] private bool _done;
    [SerializeField] private float _neededTime;

    #region Public Methods

    /// <summary> Reset all values of the task </summary>
    public void ResetValues()
    {
        foreach (Requirement requirement in taskRequirements)
            requirement.ResetValues();

        foreach (Goal goal in goals)
            goal.ResetValues();

        if (scene != null)
            RecreateSceneFromBackup();

        if (resetObjects.Count > 0)
            ResetObjects();

        NeededTime = 0;
        Done = false;
    }

    #endregion

    #region Private Methods

    /// <summary> Reset the scene based on the stored backup </summary>
    private void RecreateSceneFromBackup()
    {
        var path = AssetDatabase.GetAssetPath(scene);
        var fileName = Path.GetFileName(path);

        var instanceFileName = fileName[2..];
        var instancePath = path.Replace(fileName, instanceFileName);

        AssetDatabase.DeleteAsset(instancePath);
        AssetDatabase.CopyAsset(path, instancePath);
        AssetDatabase.Refresh();
    }

    /// <summary> Reset all objects based on their reset behaviour </summary>
    private void ResetObjects()
    {
        foreach (ResetObjectLogic resetObject in resetObjects)
            resetObject.ResetLogic();
    }

    #endregion
}

}
#endif
