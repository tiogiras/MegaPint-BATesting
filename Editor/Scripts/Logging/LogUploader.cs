﻿using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace MegaPint.Editor.Scripts.Logging
{

internal static class LogUploader
{
    public static bool hasTriedUploading;

    private const string UploadUrl = "https://tiogiras.games/submitLog.php";

    public static async void TryUploadAllLogs(string persistentPath)
    {
        if (!await Utility.IsValidTesterToken())
            return;
        
        var files = Directory.GetFiles(persistentPath, "*.json");

        if (files.Length == 0)
            return;

        foreach (var file in files)
        {
            await UploadLog(file);
        }
        
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
        {
            await Task.Delay(100);
        }

        if (www.result == UnityWebRequest.Result.Success)
            File.Delete(path);
    }
}

}
