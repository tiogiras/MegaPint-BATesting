// TODO commenting

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

[CreateAssetMenu(fileName = "Requirement", menuName = "MegaPint/Requirement", order = 0)] // TODO remove this line
internal class Requirement : ScriptableObject
{
    public bool Done
    {
        get => _done;
        set
        {
            _done = value;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }

    public string requirementName;
    [SerializeField] private bool _done;

    #region Public Methods

    public void ResetValues()
    {
        Done = false;
    }

    #endregion
}

}
#endif
