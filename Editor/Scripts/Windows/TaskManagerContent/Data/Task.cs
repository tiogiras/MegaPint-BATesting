#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using MegaPint.RepairScene;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

/// <summary> Holds data about a task </summary>
public class Task : ScriptableObject
{
    public static Action <Task> onTaskDoneChange;

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
    public bool resetSinglePrefab;
    public bool resetPrefabs;

    private int _autoSaveCount;

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

    /// <summary> Open the task scene </summary>
    public void OpenScene()
    {
        GetSceneFromBackUp(false, out var path);

        EditorSceneManager.OpenScene(path);
    }

    /// <summary> Reset all values of the task </summary>
    public void ResetValues()
    {
        foreach (Requirement requirement in taskRequirements)
            requirement.ResetValues();

        foreach (Goal goal in goals)
            goal.ResetValues();

        if (scene != null)
            GetSceneFromBackUp(true, out var _);

        if (resetPrefabs)
            PrefabRepair.ResetPrefabs();

        if (resetSinglePrefab)
            PrefabRepair.ResetSinglePrefab();

        NeededTime = 0;
        Done = false;
    }

    /// <summary> Force the needed time to save the settings </summary>
    public void SaveNeededTime()
    {
        SaveValues.TestData.SetValue(taskName, "1", NeededTime);
    }

    #endregion

    #region Private Methods

    /// <summary> Reset the scene based on the stored backup </summary>
    private void GetSceneFromBackUp(bool resetScene, out string path)
    {
        var directoryPath = Path.Join(Application.dataPath, "MegaPint Test Scenes");
        
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var sceneName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(scene));
        sceneName = sceneName.Replace("B_", "");

        path = Path.Join("Assets", "MegaPint Test Scenes", sceneName + ".unity");
        
        if (!AssetDatabase.LoadAssetAtPath <Object>(path))
        {
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(scene), path);
            AssetDatabase.Refresh();
        }
        else
        {
            if (!resetScene)
                return;

            AssetDatabase.DeleteAsset(path);
            
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(scene), path);
            AssetDatabase.Refresh();
        }
    }

    #endregion
}

}
#endif
