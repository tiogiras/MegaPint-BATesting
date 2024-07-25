using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

internal class StrongEnemy : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private string _name;
    
    public bool IsValid {get; private set;}

    private void Awake()
    {
        IsValid = _health is >= 30 and <= 60 && !string.IsNullOrEmpty(_name);
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
        
        if (string.IsNullOrEmpty(_name))
        {
            hasIssue = true;
            issue.Append("- The enemy is missing a name!\n");
        }

        if (hasIssue)
            Debug.LogWarning(issue);
    }
}

}
