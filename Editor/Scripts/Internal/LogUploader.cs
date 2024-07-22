using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MegaPint.Editor.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Internal
{

internal static class LogUploader
{
    private const string UploadUrl = "https://tiogiras.games/submitLog.php";
    public static bool hasTriedUploading;

    #region Public Methods

    public static async void TryUploadAllLogs(string persistentPath)
    {
        if (!await Utility.IsValidTesterToken())
        {
            Exit();

            return;
        }

        var files = Directory.GetFiles(persistentPath, "*.json");

        if (files.Length == 0)
            return;

        foreach (var file in files.Where(f => !Path.GetFileName(f).Equals("Task.json")))
            await UploadLog(file);

        Exit();
    }

    #endregion

    #region Private Methods

    private static void Exit()
    {
        hasTriedUploading = true;
        EditorApplication.Exit(0);
    }

    private static async Task UploadLog(string path)
    {
        var logContent = await File.ReadAllTextAsync(path);

        var form = new WWWForm();
        form.AddField("token", SaveValues.BasePackage.TesterToken);
        form.AddField("log", logContent);

        UnityWebRequest www = UnityWebRequest.Post(UploadUrl, form);
        UnityWebRequestAsyncOperation task = www.SendWebRequest();

        while (!task.isDone)
            await Task.Delay(100);

        if (www.result == UnityWebRequest.Result.Success)
            File.Delete(path);
    }

    #endregion
}

}
