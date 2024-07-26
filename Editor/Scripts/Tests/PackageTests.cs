#if UNITY_EDITOR
#if UNITY_INCLUDE_TESTS
using System.Collections;
using System.Threading.Tasks;
using MegaPint.Editor.Scripts.PackageManager.Packages;
using MegaPint.Editor.Scripts.Tests.Utility;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Tests
{

/// <summary> Unit tests regarding the general structure and settings of the package </summary>
internal class PackageTests
{
    private static bool s_initialized;
    
    [UnityTest] [Order(0)]
    public IEnumerator InitializePackageCache()
    {
        Task <bool> task = TestsUtility.CheckCacheInitialization();
        yield return task.AsIEnumeratorReturnNull();

        s_initialized = task.Result;
        Assert.IsTrue(task.Result);
    }

    [Test] [Order(1)]
    public void PackageStructure()
    {
        if (!s_initialized)
            Assert.Fail("FAILED ===> Missing packageCache initialization!");
        
        TestsUtility.CheckStructure(PackageKey.BATesting);
    }
    
    [Test] [Order(1)]
    public void Resources()
    {
        var isValid = true;

        TestsUtility.ValidateResource <VisualTreeAsset>(ref isValid, Constants.BaTesting.UserInterface.Overview);
        TestsUtility.ValidateResource <VisualTreeAsset>(ref isValid, Constants.BaTesting.UserInterface.TermsAgreement);
        TestsUtility.ValidateResource <VisualTreeAsset>(ref isValid, Constants.BaTesting.UserInterface.Task);
        TestsUtility.ValidateResource <VisualTreeAsset>(ref isValid, Constants.BaTesting.UserInterface.InvalidToken);
        TestsUtility.ValidateResource <VisualTreeAsset>(ref isValid, Constants.BaTesting.UserInterface.TaskManager);
        TestsUtility.ValidateResource <VisualTreeAsset>(ref isValid, Constants.BaTesting.UserInterface.Requirement);
        TestsUtility.ValidateResource <VisualTreeAsset>(ref isValid, Constants.BaTesting.UserInterface.Goal);

        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.TestRequirement);

        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.MenuItems);
        
        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.UploadScreenshots1);
        
        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.UploadScreenshots2);
        
        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.UploadScreenshots3);
        
        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.ScreenshotSurvey);
        
        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.ValidatorsSurvey);
            
        TestsUtility.ValidateResource <VisualTreeAsset>(
            ref isValid,
            Constants.BaTesting.UserInterface.Requirements.GeneralSurvey);

        Assert.IsTrue(isValid);
    }
    
}
}
#endif
#endif
