#if UNITY_EDITOR
using MegaPint.Editor.Scripts.GUI.Utility;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.Editor.Scripts.Windows
{

/// <summary>
///     Window based on the <see cref="EditorWindowBase" /> to be displayed whenever an window tries to open but the
///     tester token is invalid
/// </summary>
internal class TermsAgreement : EditorWindowBase
{
    private VisualTreeAsset _baseWindow;

    private Button _btnAgree;

    #region Public Methods

    /// <summary> Show the window </summary>
    /// <returns> Window instance </returns>
    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "Terms Agreement";

        minSize = new Vector2(550, 325);
        maxSize = new Vector2(550, 325);

        this.CenterOnMainWin();

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return Constants.BaTesting.UserInterface.TermsAgreement;
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = GUIUtility.Instantiate(_baseWindow, root);
        content.style.flexGrow = 1f;
        content.style.flexShrink = 1f;

        _btnAgree = content.Q <Button>("BTN_Agree");

        RegisterCallbacks();
    }

    protected override bool LoadResources()
    {
        _baseWindow = Resources.Load <VisualTreeAsset>(BasePath());

        return _baseWindow != null;
    }

    protected override void RegisterCallbacks()
    {
        _btnAgree.clicked += OnAgree;
    }

    protected override void UnRegisterCallbacks()
    {
        if (_btnAgree == null)
            return;
        
        _btnAgree.clicked -= OnAgree;
    }

    #endregion

    #region Private Methods

    private void OnAgree()
    {
        SaveValues.BaTesting.AgreedToTerms = true;
        Close();
    }

    #endregion
}

}
#endif
