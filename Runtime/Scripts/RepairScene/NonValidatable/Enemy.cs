using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

internal class Enemy : MonoBehaviour
{
    [SerializeField] private float _health;
    public bool IsValid {get; private set;}

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
}

}
