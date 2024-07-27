#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace MegaPint.Editor.Scripts.Internal
{

/// <summary> Handles the upload of the logs </summary>
internal static class LogUploader
{
    private const string UploadUrl = "https://tiogiras.games/submitLog.php";
    public static bool hasTriedUploading;

    #region Public Methods

    /// <summary> Try to upload all logs </summary>
    /// <param name="persistentPath"> Path to the directory containing the logs </param>
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

    /// <summary> Try to upload the task log </summary>
    /// <param name="persistentPath"> Path to the directory containing the logs </param>
    /// <returns> If the upload was successful </returns>
    public static async Task <bool> TryUploadTaskLog(string persistentPath)
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

    /// <summary> Exit the application </summary>
    private static void Exit()
    {
        hasTriedUploading = true;
        EditorApplication.Exit(0);
    }

    /// <summary> Upload a log file </summary>
    /// <param name="path"> Path to the file </param>
    /// <param name="suppressDeletion"> If true the file is not deleted after upload </param>
    /// <returns> If the upload was successful </returns>
    private static async Task <bool> UploadLog(string path, bool suppressDeletion = false)
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
#endif
