using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Directory = System.IO.Directory;

namespace MegaPint.Editor.Scripts.Internal
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

    public static async Task<bool> TryUploadTaskLog(string persistentPath)
    {
        if (!await Utility.IsValidTesterToken())
            return false;
        
        var file = Path.Combine(persistentPath, "Task.json");
        
        if (!File.Exists(file))
            return false;

        return await UploadLog(file, true);
    }

    #endregion

    #region Private Methods

    private static void Exit()
    {
        hasTriedUploading = true;
        EditorApplication.Exit(0);
    }

    private static async Task<bool> UploadLog(string path, bool suppressDeletion = false)
    {
        var logContent = await File.ReadAllTextAsync(path);

        var form = new WWWForm();
        form.AddField("token", SaveValues.BasePackage.TesterToken);
        form.AddField("log", logContent);

        UnityWebRequest www = UnityWebRequest.Post(UploadUrl, form);
        UnityWebRequestAsyncOperation task = www.SendWebRequest();

        while (!task.isDone)
            await Task.Delay(100);
        
        if (www.result == UnityWebRequest.Result.Success && !suppressDeletion)
            File.Delete(path);

        return www.result == UnityWebRequest.Result.Success;
    }

    #endregion
}

}
