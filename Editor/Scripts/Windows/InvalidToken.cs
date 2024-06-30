#if UNITY_EDITOR
using MegaPint.Editor.Scripts;
using MegaPint.Editor.Scripts.GUI.Utility;
using MegaPint.Editor.Scripts.Windows;
using UnityEngine;
using UnityEngine.UIElements;
using ContextMenu = MegaPint.Editor.Scripts.ContextMenu;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Windows
{

/// <summary>
///     Window based on the <see cref="EditorWindowBase" /> to be displayed whenever an window tries to open but the
///     tester token is invalid
/// </summary>
internal class InvalidToken : EditorWindowBase
{
    private VisualTreeAsset _baseWindow;

    #region Public Methods

    /// <summary> Show the window </summary>
    /// <returns> Window instance </returns>
    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "Invalid Token";

        minSize = new Vector2(450, 260);
        maxSize = new Vector2(450, 260);

        this.CenterOnMainWin();

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return Constants.BaTesting.UserInterface.InvalidToken;
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = GUIUtility.Instantiate(_baseWindow, root);
        content.style.flexGrow = 1f;
        content.style.flexShrink = 1f;

        RegisterCallbacks();

        root.ActivateLinks(
            evt =>
            {
                switch (evt.linkID)
                {
                    case "BaseWindow":
                        ContextMenu.Open();
                        Close();

                        break;
                }
            });
    }

    protected override bool LoadResources()
    {
        _baseWindow = Resources.Load <VisualTreeAsset>(BasePath());

        return _baseWindow != null;
    }

    protected override void RegisterCallbacks()
    {
    }

    protected override void UnRegisterCallbacks()
    {
    }

    #endregion
}

}
#endif
