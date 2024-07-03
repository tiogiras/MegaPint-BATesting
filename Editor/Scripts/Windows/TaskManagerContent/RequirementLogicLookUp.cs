// TODO Commenting

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using MegaPint.Editor.Scripts.GUI.Utility;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Windows.TaskManagerContent
{

internal static class RequirementLogicLookUp
{
    public class Logic
    {
        public readonly Action <VisualElement> action;
        public readonly bool openRequirementInformation;

        public Logic(bool arg, Action <VisualElement> action)
        {
            openRequirementInformation = arg;
            this.action = action;
        }
    }

    public static readonly Dictionary <string, Logic> LookUp = new()
    {
        {
            "Test Requirement", new Logic(
                true,
                element =>
                {
                    element.ActivateLinks(
                        evt =>
                        {
                            switch (evt.linkID)
                            {
                                case "BaseWindow":
                                    ContextMenu.BasePackage.OpenBaseWindow();

                                    break;
                            }
                        });
                })
        },
        {
            "MenuItems", new Logic(
                true,
                element =>
                {

                })
        },
        {
            "Shortcuts", new Logic(
                false,
                _ =>
                {
                    ContextMenu.BasePackage.OpenBaseWindowPerLink("Info/Help/How To's/How To: Shortcuts");
                })
        },
    };
}

}
#endif
