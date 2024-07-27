using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

/// <summary> Used in the repair a scene 1 task </summary>
[AddComponentMenu("")]
internal class GroundObject : MonoBehaviour
{
    public bool IsValid {get; private set;}

    #region Unity Event Functions

    private void Awake()
    {
        IsValid = LayerMask.LayerToName(gameObject.layer).Equals("Ground") &&
                  !gameObject.GetComponent <Rigidbody>() &&
                  gameObject.GetComponent(typeof(Collider));
    }

    private void OnValidate()
    {
        var issue = new StringBuilder($"Issues in {this}:\n");
        var hasIssue = false;

        if (!LayerMask.LayerToName(gameObject.layer).Equals("Ground"))
        {
            hasIssue = true;
            issue.Append("- Layer is not set to Ground!\n");
        }

        if (gameObject.GetComponent <Rigidbody>())
        {
            hasIssue = true;
            issue.Append("- Rigidbody found!\n");
        }

        if (!gameObject.GetComponent(typeof(Collider)))
        {
            hasIssue = true;
            issue.Append("- No Collider found!\n");
        }

        if (hasIssue)
            Debug.LogWarning(issue);
    }

    #endregion
}

}
