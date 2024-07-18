using System;
using System.IO;
using MegaPint.Editor.Scripts.DevMode;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace MegaPint.Editor.Scripts.Logging
{

[InitializeOnLoad]
internal static class LoggingManager
{
    private static string s_persistentDataPath;
    private static string s_managementFilePath;

    private static string s_currentLogFileName;
    private static string s_currentLogFilePath;

    private static SessionLog s_currentLog;

    private static int s_currentLogSaveInterval;

    static LoggingManager()
    {
        if (!SaveValues.BaTesting.AgreedToTerms)
            ContextMenu.BATesting.OpenTermsAgreement();

        GetSessionLogFile();
        GetSessionLog();

        EditorApplication.wantsToQuit += OnWantsToQuit;
    }

    #region Public Methods

    public static void LogToCurrentSession(string categoryName, string logText)
    {
        if (s_currentLogSaveInterval >= SaveValues.BaTesting.LogSaveInterval)
        {
            s_currentLogSaveInterval = 0;

            SaveCurrentLog();
        }
        else
            s_currentLogSaveInterval++;

        s_currentLog.Log(categoryName, logText);
    }

    #endregion

    #region Private Methods

    private static void CreateNewSession()
    {
        var newFileName = Guid.NewGuid().ToString();

        File.WriteAllText(GetManagementFilePath(), newFileName);
        s_currentLogFileName = default;

        File.Create(GetCurrentLogFilePath()).Close();

        s_currentLog = new SessionLog(s_currentLogFileName);
        SaveCurrentLog();
    }

    private static string GetCurrentLogFileName()
    {
        return s_currentLogFileName ??= File.ReadAllText(GetManagementFilePath());
    }

    private static string GetCurrentLogFilePath()
    {
        return s_currentLogFilePath ??= Path.Join(GetPersistentDataPath(), $"{GetCurrentLogFileName()}.json");
    }

    private static string GetManagementFilePath()
    {
        s_managementFilePath ??= Path.Join(GetPersistentDataPath(), "Management.txt");

        if (!File.Exists(s_managementFilePath))
            File.Create(s_managementFilePath).Close();

        return s_managementFilePath;
    }

    private static string GetPersistentDataPath()
    {
        s_persistentDataPath ??= Path.Join(
            Path.Combine(Application.persistentDataPath.Split(Path.AltDirectorySeparatorChar)[..^2]),
            "MegaPintLogging");

        if (!Directory.Exists(s_persistentDataPath))
            Directory.CreateDirectory(s_persistentDataPath);

        return s_persistentDataPath;
    }

    private static void GetSessionLog()
    {
        var json = File.ReadAllText(GetCurrentLogFilePath());
        s_currentLog = JsonUtility.FromJson <SessionLog>(json);
    }

    private static void GetSessionLogFile()
    {
        if (!string.IsNullOrEmpty(GetCurrentLogFileName()))
            return;

        CreateNewSession();
    }

    private static bool OnWantsToQuit()
    {
        if (LogUploader.hasTriedUploading)
            return LogUploader.hasTriedUploading;

        s_currentLog.EndSession();
        SaveCurrentLog();

        File.WriteAllText(GetManagementFilePath(), "");

        LogUploader.TryUploadAllLogs(GetPersistentDataPath());

        return LogUploader.hasTriedUploading;
    }

    private static void SaveCurrentLog()
    {
        DevLog.Log("Saving current log");

        var json = JsonUtility.ToJson(s_currentLog, true);
        File.WriteAllText(GetCurrentLogFilePath(), json);
    }

    #endregion
}

}
