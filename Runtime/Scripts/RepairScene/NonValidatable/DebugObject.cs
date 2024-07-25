using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

internal class DebugObject : MonoBehaviour
{
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
}

}
