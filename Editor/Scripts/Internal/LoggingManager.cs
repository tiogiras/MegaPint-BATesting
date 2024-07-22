using System;
using System.IO;
using MegaPint.Editor.Scripts;
using MegaPint.Editor.Scripts.DevMode;
using MegaPint.Editor.Scripts.Windows;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;
using ContextMenu = MegaPint.Editor.Scripts.ContextMenu;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Internal
{

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

    private static SessionLog _CurrentLog => s_taskLogging ? s_taskLog : s_currentLog;
    
    private static void ActivateTaskLogging(string currentTask)
    {
        DevLog.Log($"Activated Task Logging for task: {currentTask}");
        
        AnyKeyDetector.Enable();

        s_currentTask = currentTask;
        s_taskLogging = true;
    }

    private static void DeActivateTaskLogging()
    {
        DevLog.Log("Deactivated Task Logging");
        
        AnyKeyDetector.Disable();

        s_currentTask = "";
        s_taskLogging = false;
    }

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
    }

    private static void GetTaskLogFile()
    {
        if (File.Exists(GetTaskLogFilePath()))
            return;

        File.Create(GetTaskLogFilePath()).Close();

        s_taskLog = new SessionLog($"TaskLog {Guid.NewGuid()}");
        
        var json = JsonUtility.ToJson(s_taskLog, true);
        File.WriteAllText(GetTaskLogFilePath(), json);
    }

    private static void GetTaskLog()
    {
        var json = File.ReadAllText(GetTaskLogFilePath());
        s_taskLog = JsonUtility.FromJson <SessionLog>(json);
    }

    private static string GetTaskLogFilePath()
    {
        return s_taskLogFilePath ??= Path.Join(GetPersistentDataPath(), "Task.json");
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

        var task = string.IsNullOrEmpty(s_currentTask) ? "" : $"Task_{s_currentTask} / ";
        
        _CurrentLog.Log($"{task}{categoryName}", logText);
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

    private static string s_loggingFolderPath;
    
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

        s_taskLogging = true;
        SaveCurrentLog();
        s_taskLogging = false;
        
        s_currentLog.EndSession();
        SaveCurrentLog();

        File.WriteAllText(GetManagementFilePath(), "");

        LogUploader.TryUploadAllLogs(GetPersistentDataPath());

        return LogUploader.hasTriedUploading;
    }

    private static void SaveCurrentLog()
    {
        if (_CurrentLog == null)
            return;
        
        DevLog.Log("Saving current log");
        
        var json = JsonUtility.ToJson(_CurrentLog, true);
        File.WriteAllText(s_taskLogging ? GetTaskLogFilePath() : GetCurrentLogFilePath(), json);
    }

    #endregion
}

}
