using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

/// <summary> Used in the repair a scene 1 task </summary>
[AddComponentMenu("")]
internal class DebugObject : MonoBehaviour
{
    #region Unity Event Functions

    private void OnValidate()
    {
        var issue = new StringBuilder($"Issues in {this}:\n");
        var hasIssue = false;

        if (gameObject.activeSelf)
        {
            hasIssue = true;
            issue.Append("- Is not set to inactive!\n");
        }

        if (hasIssue)
            Debug.LogWarning(issue);
    }

    #endregion
}

}
