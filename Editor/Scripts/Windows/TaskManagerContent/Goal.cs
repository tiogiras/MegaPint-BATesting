// TODO Commenting

#if UNITY_EDITOR
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

[CreateAssetMenu(fileName = "Goal", menuName = "MegaPint/Goal", order = 0)] // TODO remove this line
internal class Goal : ScriptableObject
{
    public string title;
    [TextArea] public string hint;
    public bool done;
}

}
#endif
