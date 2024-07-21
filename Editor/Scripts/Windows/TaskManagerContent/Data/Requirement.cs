// TODO commenting

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

[CreateAssetMenu(fileName = "Requirement", menuName = "MegaPint/Requirement", order = 0)] // TODO remove this line
internal class Requirement : ScriptableObject
{
    public static Action <string, bool> onDoneChanged;
    public static Action <string> onExecute;
    
    public bool Done
    {
        get => _done;
        set
        {
            _done = value;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            
            onDoneChanged?.Invoke(requirementName, value);
        }
    }

    public string requirementName;
    [SerializeField] private bool _done;

    #region Public Methods

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

    public void ResetValues()
    {
        Done = false;
    }

    #endregion
}

}
#endif
