using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("tiogiras.megapint.editor")]

namespace MegaPint
{

/// <summary> Abstract class for objects that should be reset when a task resets </summary>
internal abstract class ResetObjectLogic : MonoBehaviour
{
    #region Public Methods

    /// <summary> Called when the object is reset </summary>
    public abstract void ResetLogic();

    #endregion
}

}
