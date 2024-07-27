using System;
using UnityEngine;

namespace MegaPint.Editor.Scripts
{

/// <summary> Used to detect if the task Create Your Own Requirement is finished </summary>
public static class TestEvent
{
    public static Action <GameObject> onValidate;
    public static Action <GameObject> onFixed;
}

}
