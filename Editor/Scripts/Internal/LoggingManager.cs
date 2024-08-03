#if UNITY_EDITOR
using System;
using System.IO;
using MegaPint.Editor.Scripts.DevMode;
using MegaPint.Editor.Scripts.Windows;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace MegaPint.Editor.Scripts.Internal
{

/// <summary> Handles the logging </summary>
[InitializeOnLoad]
internal static class LoggingManager
{
    private static string s_persistentDataPath;
    private static string s_managementFilePath;

    private static string s_currentLogFileName;
    private static string s_currentLogFilePath;

    private static string s_taskLogFilePath;

    private static SessionLog s_taskLog;
    private static SessionLog s_currentLog;

    private static int s_currentLogSaveInterval;

    private static bool s_taskLogging;
    private static string s_currentTask;

    private static string s_loggingFolderPath;

    private static SessionLog _CurrentLog => s_taskLogging ? s_taskLog : s_currentLog;

    static LoggingManager()
    {
        if (!SaveValues.BaTesting.AgreedToTerms)
            ContextMenu.BATesting.OpenTermsAgreement();

        GetSessionLogFile();
        GetSessionLog();

        GetTaskLogFile();
        GetTaskLog();

        AssemblyReloadEvents.beforeAssemblyReload += () =>
        {
            LogToCurrentSession("General / DomainReload", "Domain reload detected");

            SaveCurrentLog();
        };

        EditorApplication.wantsToQuit += OnWantsToQuit;

        TaskManager.onStartTaskLogging += ActivateTaskLogging;
        TaskManager.onStopTaskLogging += DeActivateTaskLogging;

        Overview.onSend += UploadTaskLog;
    }

    #region Public Methods

    /// <summary> Log a message to the current active session </summary>
    /// <param name="categoryName"> Category of the message </param>
    /// <param name="logText"> Log Message </param>
    public static void LogToCurrentSession(string categoryName, string logText)
    {
        if (s_currentLogSaveInterval >= SaveValues.BaTesting.LogSaveInterval)
        {
            s_currentLogSaveInterval = 0;

            SaveCurrentLog();
        }
        else
            s_currentLogSaveInterval++;

        var task = string.IsNullOrEmpty(s_currentTask) ? "" : $"Task_{s_currentTask} / ";

        _CurrentLog.Log($"{task}{categoryName}", logText);
    }

    #endregion

    #region Private Methods

    /// <summary> Activate logging to the task log </summary>
    /// <param name="currentTask"> Current active task </param>
    private static void ActivateTaskLogging(string currentTask)
    {
        DevLog.Log($"Activated Task Logging for task: {currentTask}");

        AnyKeyDetector.Enable();

        s_currentTask = currentTask;
        s_taskLogging = true;
    }

    /// <summary> Create a new session log file </summary>
    private static void CreateNewSession()
    {
        var newFileName = Guid.NewGuid().ToString();

        File.WriteAllText(GetManagementFilePath(), newFileName);
        s_currentLogFileName = default;

        File.Create(GetCurrentLogFilePath()).Close();

        s_currentLog = new SessionLog(s_currentLogFileName);
        SaveCurrentLog();
    }

    /// <summary> Deactivate logging to the task log </summary>
    private static void DeActivateTaskLogging()
    {
        DevLog.Log("Deactivated Task Logging");

        AnyKeyDetector.Disable();

        s_currentTask = "";
        s_taskLogging = false;
    }

    /// <summary> Get the name of the current log file </summary>
    /// <returns> Name of the current log file </returns>
    private static string GetCurrentLogFileName()
    {
        return s_currentLogFileName ??= File.ReadAllText(GetManagementFilePath());
    }

    /// <summary> Get the path of the current log file </summary>
    /// <returns> Path of the current log file </returns>
    private static string GetCurrentLogFilePath()
    {
        return s_currentLogFilePath ??= Path.Join(GetPersistentDataPath(), $"{GetCurrentLogFileName()}.json");
    }

    /// <summary> Get the path to the Management.txt </summary>
    /// <returns> Path to the Management.txt </returns>
    private static string GetManagementFilePath()
    {
        s_managementFilePath ??= Path.Join(GetPersistentDataPath(), "Management.txt");

        if (!File.Exists(s_managementFilePath))
            File.Create(s_managementFilePath).Close();

        return s_managementFilePath;
    }

    /// <summary> Get path to the directory of the log files </summary>
    /// <returns> Path to the persistent data </returns>
    private static string GetPersistentDataPath()
    {
        s_loggingFolderPath ??= Path.Join(
            Path.Combine(Application.persistentDataPath.Split(Path.AltDirectorySeparatorChar)[..^2]),
            "MegaPintLogging");

        if (!Directory.Exists(s_loggingFolderPath))
            Directory.CreateDirectory(s_loggingFolderPath);

        s_persistentDataPath ??= Path.Combine(s_loggingFolderPath, Application.productName);

        if (!Directory.Exists(s_persistentDataPath))
            Directory.CreateDirectory(s_persistentDataPath);

        return s_persistentDataPath;
    }

    /// <summary> Deserialize the current active session log </summary>
    private static void GetSessionLog()
    {
        var json = File.ReadAllText(GetCurrentLogFilePath());
        s_currentLog = JsonUtility.FromJson <SessionLog>(json);
    }

    /// <summary> Check that the session log file for the current log exists </summary>
    private static void GetSessionLogFile()
    {
        if (!string.IsNullOrEmpty(GetCurrentLogFileName()))
            return;

        CreateNewSession();
    }

    /// <summary> Deserialize the task log </summary>
    private static void GetTaskLog()
    {
        var json = File.ReadAllText(GetTaskLogFilePath());
        s_taskLog = JsonUtility.FromJson <SessionLog>(json);
    }

    /// <summary> Check that the task log file exists </summary>
    private static void GetTaskLogFile()
    {
        if (File.Exists(GetTaskLogFilePath()))
            return;

        File.Create(GetTaskLogFilePath()).Close();

        s_taskLog = new SessionLog($"TaskLog {Guid.NewGuid()}");

        var json = JsonUtility.ToJson(s_taskLog, true);
        File.WriteAllText(GetTaskLogFilePath(), json);
    }

    /// <summary> Get the path of the task log file </summary>
    /// <returns> Path to the task log file </returns>
    private static string GetTaskLogFilePath()
    {
        return s_taskLogFilePath ??= Path.Join(GetPersistentDataPath(), "Task.json");
    }

    /// <summary> Called when the application wants to quit </summary>
    /// <returns> If the application is allowed to quit </returns>
    private static bool OnWantsToQuit()
    {
        if (LogUploader.hasTriedUploading)
            return LogUploader.hasTriedUploading;

        s_taskLogging = true;
        SaveCurrentLog();
        s_taskLogging = false;

        s_currentLog.EndSession();
        SaveCurrentLog();

        File.WriteAllText(GetManagementFilePath(), "");

        LogUploader.TryUploadAllLogs(GetPersistentDataPath());

        return LogUploader.hasTriedUploading;
    }

    /// <summary> Save the current log </summary>
    private static void SaveCurrentLog()
    {
        if (_CurrentLog == null)
            return;

        DevLog.Log("Saving current log");

        var json = JsonUtility.ToJson(_CurrentLog, true);
        File.WriteAllText(s_taskLogging ? GetTaskLogFilePath() : GetCurrentLogFilePath(), json);
    }

    /// <summary> Upload the task log file </summary>
    /// <param name="neededTimes"> All needed values of all tasks </param>
    private static async void UploadTaskLog(float[] neededTimes)
    {
        s_taskLog.neededTimes = neededTimes;

        var json = JsonUtility.ToJson(s_taskLog, true);
        await File.WriteAllTextAsync(GetTaskLogFilePath(), json);

        if (await LogUploader.TryUploadTaskLog(GetPersistentDataPath()))
        {
            EditorUtility.DisplayDialog(
                "Upload Results.",
                "The results have been successfully uploaded. Please do not delete the project until the results have been reviewed.",
                "Ok");

            Overview.onSendComplete?.Invoke();

            return;
        }

        EditorUtility.DisplayDialog(
            "Upload Results.",
            "The upload of the results has failed! Confirm that you have an established internet connection and try again. If the problem persists, please contact me.",
            "Ok");

        Overview.onSendComplete?.Invoke();
    }

    #endregion
}

}
#endif
