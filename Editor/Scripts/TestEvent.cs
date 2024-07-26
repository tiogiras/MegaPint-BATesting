using System;
using UnityEngine;

namespace MegaPint.Editor.Scripts
{

public static class TestEvent
{
    public static Action<GameObject> onValidate;
    public static Action<GameObject> onFixed;
}

}
