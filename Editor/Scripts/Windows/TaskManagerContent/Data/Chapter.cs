// TODO Commenting

#if UNITY_EDITOR
using UnityEngine;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data
{

[CreateAssetMenu(fileName = "Chapter", menuName = "MegaPint/Chapter", order = 0)] // TODO remove this line
internal class Chapter : ScriptableObject
{
    public string chapterName;
}

}
#endif
