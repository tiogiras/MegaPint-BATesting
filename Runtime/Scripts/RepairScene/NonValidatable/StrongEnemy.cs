using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

internal class StrongEnemy : MonoBehaviour
{
    [SerializeField] private float _health;
    public bool IsValid {get; private set;}

    private void Awake()
    {
        IsValid = _health is >= 30 and <= 60;
    }
    
    private void OnValidate()
    {
        var issue = new StringBuilder($"Issues in {this}:\n");
        var hasIssue = false;

        if (_health is < 30 or > 60)
        {
            hasIssue = true;
            issue.Append("- Health is not in range!\n");
        }

        if (hasIssue)
            Debug.LogWarning(issue);
    }
}

}
