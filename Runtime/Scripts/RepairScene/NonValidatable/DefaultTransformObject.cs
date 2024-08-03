using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

/// <summary> Used in the repair a scene 1 task </summary>
[AddComponentMenu("")]
public class DefaultTransformObject : MonoBehaviour
{
    public bool HasDefaultTransform {get; private set;}

    #region Unity Event Functions

    private void Awake()
    {
        HasDefaultTransform = HasDt();
    }

    private void OnValidate()
    {
        var issue = new StringBuilder($"Issues in {this}:\n");
        var hasIssue = false;

        if (!HasDt())
        {
            hasIssue = true;
            issue.Append("- Has no default transform!\n");
        }

        if (hasIssue)
            Debug.LogWarning(issue);
    }

    #endregion

    #region Private Methods

    private bool HasDt()
    {
        Transform myTransform = transform;

        return myTransform.localPosition == Vector3.zero &&
               myTransform.localRotation == Quaternion.identity &&
               myTransform.localScale == Vector3.one;
    }

    #endregion
}

}
