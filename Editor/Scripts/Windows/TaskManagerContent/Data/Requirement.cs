#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

/// <summary> Holds data about a requirement </summary>
public class Requirement : ScriptableObject
{
    public static Action <string, bool> onDoneChanged;
    public static Action <string> onExecute;

    public bool Done
    {
        get
        {
            if (_doneInitialized)
                return _done;

            _done = SaveValues.TestData.GetValue(requirementName, "0", false);
            _doneInitialized = true;

            return _done;
        }
        set
        {
            SaveValues.TestData.SetValue(requirementName, "0", value);

            _done = value;
            onDoneChanged?.Invoke(requirementName, value);
        }
    }

    [SerializeField] private string _guid;
    public string requirementName;

    private bool _done;
    private bool _doneInitialized;

    #region Unity Event Functions

    private void OnValidate()
    {
        _doneInitialized = false;
        
        if (!string.IsNullOrEmpty(_guid))
            return;
        
        _guid = Guid.NewGuid().ToString();
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }

    #endregion

    #region Public Methods

    /// <summary> Execute the requirement </summary>
    /// <param name="requirement"> Targeted requirement </param>
    public static void ExecuteRequirement(Requirement requirement)
    {
        onExecute?.Invoke(requirement.requirementName);

        RequirementLogicLookUp.Logic logic = RequirementLogicLookUp.LookUp[requirement.requirementName];

        if (logic.openRequirementInformation)
        {
            ContextMenu.BATesting.OpenRequirementInformation();

            RequirementInformation.AfterGUICreation(
                Resources.Load <VisualTreeAsset>(RequirementGUILookUp.LookUp[requirement.requirementName]),
                logic.action);
        }
        else
            logic.action?.Invoke(null);
    }

    /// <summary> Reset all values of the requirement </summary>
    public void ResetValues()
    {
        Done = false;
    }

    #endregion
}

}
#endif
