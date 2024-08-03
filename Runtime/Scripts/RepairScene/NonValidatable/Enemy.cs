using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

/// <summary> Used in the repair a scene 1 task </summary>
[AddComponentMenu("")]
internal class Enemy : MonoBehaviour
{
    public bool IsValid {get; private set;}

    [SerializeField] private float _health;

    #region Unity Event Functions

    private void Awake()
    {
        IsValid = _health is >= 10 and <= 30;
    }

    private void OnValidate()
    {
        var issue = new StringBuilder($"Issues in {this}:\n");
        var hasIssue = false;

        if (_health is < 10 or > 30)
        {
            hasIssue = true;
            issue.Append("- Health is not in range!\n");
        }

        if (hasIssue)
            Debug.LogWarning(issue);
    }

    #endregion
}

}
