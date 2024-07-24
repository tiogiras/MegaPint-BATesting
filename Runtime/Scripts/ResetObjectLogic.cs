using System.Runtime.CompilerServices;
using UnityEngine;


[assembly: InternalsVisibleTo("tiogiras.megapint.editor")]
namespace MegaPint
{

internal abstract class ResetObjectLogic : MonoBehaviour
{
    public abstract void ResetLogic();
}

}
