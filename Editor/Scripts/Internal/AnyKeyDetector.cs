using System;
using System.Linq;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Internal
{

[InitializeOnLoad]
internal class AnyKeyDetector : IObserver <InputEventPtr>
{
    private const int MouseInputCooldown = 30;
    public static Action <bool> onInput;

    private static bool s_active;
    private static int s_mouseInputCooldown;

    static AnyKeyDetector()
    {
        InputSystem.onEvent.Subscribe(new AnyKeyDetector());
        EditorApplication.update += ReduceCooldown;
    }

    #region Public Methods

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    // ReSharper disable once CognitiveComplexity
    public void OnNext(InputEventPtr value)
    {
        if (!s_active)
            return;

        foreach (InputControl inputControl in value.GetAllButtonPresses())
        {
            InputDevice t = inputControl.device;
            FourCC eventType = value.type;

            if (eventType == StateEvent.Type)
            {
                if (!value.EnumerateChangedControls(t, 0.0001f).Any())
                    continue;
            }

            switch (t)
            {
                case Mouse:
                    if (s_mouseInputCooldown == 0)
                        onInput?.Invoke(false);

                    s_mouseInputCooldown += MouseInputCooldown;

                    if (s_mouseInputCooldown > MouseInputCooldown)
                        s_mouseInputCooldown = MouseInputCooldown;

                    break;

                case Keyboard:
                    onInput?.Invoke(true);

                    break;
            }

            break;
        }
    }

    public static void Disable()
    {
        s_active = false;
    }

    public static void Enable()
    {
        s_active = true;
    }

    #endregion

    #region Private Methods

    private static void ReduceCooldown()
    {
        if (s_mouseInputCooldown > 0)
            s_mouseInputCooldown--;
    }

    #endregion
}

}
