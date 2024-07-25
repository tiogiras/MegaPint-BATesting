using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

internal class Npc : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private string _name;

    public bool IsValid {get; private set;}

    private void Awake()
    {
        IsValid = _health is >= 50 and <= 99 && !string.IsNullOrEmpty(_name);
    }
    
    private void OnValidate()
    {
        var issue = new StringBuilder($"Issues in {this}:\n");
        var hasIssue = false;

        if (_health is < 50 or > 99)
        {
            hasIssue = true;
            issue.Append("- Health is not in range!\n");
        }
        
        if (string.IsNullOrEmpty(_name))
        {
            hasIssue = true;
            issue.Append("- The NPC is missing a name!\n");
        }

        if (hasIssue)
            Debug.LogWarning(issue);
    }
}

}
