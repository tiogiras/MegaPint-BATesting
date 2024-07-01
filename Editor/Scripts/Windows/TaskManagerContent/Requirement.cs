// TODO commenting

#if UNITY_EDITOR
using UnityEngine;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows.TaskManagerContent
{

[CreateAssetMenu(fileName = "Requirement", menuName = "MegaPint/Requirement", order = 0)] // TODO remove this line
internal class Requirement : ScriptableObject
{
    public string requirementName;
    public bool done;
}

}
#endif
