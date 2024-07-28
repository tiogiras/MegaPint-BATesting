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

    private int _autoSaveCount;
    
    public float NeededTime
    {
        get
        {
            if (_neededTimeInitialized)
                return _neededTime;

            _neededTime = SaveValues.TestData.GetValue(taskName, "1", 0f);
            _neededTimeInitialized = true;

            return _neededTime;
        }
        set
        {
            SaveValues.TestData.SetValue(taskName, "1", value, _autoSaveCount < 30);
            _neededTime = value;

            _autoSaveCount++;
            
            if (_autoSaveCount > 30)
                _autoSaveCount = 0;
        }
    }
    
    public void SaveNeededTime()
    {
        SaveValues.TestData.SetValue(taskName, "1", NeededTime);
    }

    public bool Done
    {
        get
        {
            if (_doneInitialized)
                return _done;

            _done = SaveValues.TestData.GetValue(taskName, "0", false);
            _doneInitialized = true;

            return _done;
        }
        set
        {
            SaveValues.TestData.SetValue(taskName, "0", value);
            _done = value;
            onTaskDoneChange?.Invoke(this);
        }
    }

    [SerializeField] private string _guid;
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
    
    private bool _done;
    private bool _doneInitialized;
    
    private float _neededTime;
    private bool _neededTimeInitialized;

    #region Unity Event Functions

    private void OnValidate()
    {
        _doneInitialized = false;
        _neededTimeInitialized = false;

        _autoSaveCount = 0;
        
        if (!string.IsNullOrEmpty(_guid))
            return;
        
        _guid = Guid.NewGuid().ToString();
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }

    #endregion
    
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
