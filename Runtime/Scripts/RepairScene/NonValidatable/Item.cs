using System;
using System.Text;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

public class Item : MonoBehaviour
{
    public bool HasCollided {get; private set;}

    private void OnValidate()
    {
        var issue = new StringBuilder($"Issues in {this}:\n");
        var hasIssue = false;

        if (!LayerMask.LayerToName(gameObject.layer).Equals("Item"))
        {
            hasIssue = true;
            issue.Append("- Layer is not set to Item!\n");
        }

        if (!gameObject.GetComponent <Rigidbody>())
        {
            hasIssue = true;
            issue.Append("- No Rigidbody found!\n");
        }
        
        if (!gameObject.GetComponent(typeof(Collider)))
        {
            hasIssue = true;
            issue.Append("- No Collider found!\n");
        }
        
        if (hasIssue)
            Debug.LogWarning(issue);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!LayerMask.LayerToName(gameObject.layer).Equals("Item"))
            return;
        
        if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Ground"))
            HasCollided = true;
    }
    
}

}
