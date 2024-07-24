// TODO commenting

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

internal class Task : ScriptableObject
{
    public static Action<Task> onTaskDoneChange;
    
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

    private void ResetObjects()
    {
        foreach (ResetObjectLogic resetObject in resetObjects)
        {
            resetObject.ResetLogic();
        }
    }

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

    public void SetRequirementDone(int index, bool done)
    {
        taskRequirements[index].Done = done;
    }

    public void SetRequirementsDone(bool done)
    {
        for (var i = 0; i < taskRequirements.Count; i++)
            SetRequirementDone(i, done);
    }

    #endregion
}

}
#endif
