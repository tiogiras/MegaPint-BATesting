#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using MegaPint.Editor.Scripts.DevMode;
using MegaPint.Editor.Scripts.PackageManager.Cache;
using MegaPint.Editor.Scripts.PackageManager.Packages;

namespace MegaPint.Editor.Scripts.Internal
{

/// <summary> Contains log data </summary>
[Serializable]
internal class SessionLog
{
    [Serializable]
    public class LogCategory
    {
        public string categoryName;
        public List <LogEntry> logs;
    }

    [Serializable]
    public struct LogEntry
    {
        public string timestamp;
        public string logText;
    }

    public string sessionID;
    public string sessionStartTime;
    public string sessionEndTime;

    public bool usingPlayModeStartScene;
    public bool usingAutoSave;
    public bool usingScreenshot;
    public bool usingValidators;

    public float[] neededTimes;
    public List <LogCategory> categories;

    public SessionLog(string sessionID)
    {
        this.sessionID = sessionID;
        sessionStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        GetUsedPackages();
    }

    #region Public Methods

    /// <summary> End the session for this log file </summary>
    public void EndSession()
    {
        sessionEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary> Log a message to this file </summary>
    /// <param name="categoryName"> Category of the message </param>
    /// <param name="logText"> Log message </param>
    public void Log(string categoryName, string logText)
    {
        DevLog.Log($"Added log: {logText}\n to category: {categoryName}");

        LogCategory category = GetCategory(categoryName);
        category.logs.Add(new LogEntry {logText = logText, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")});
    }

    #endregion

    #region Private Methods

    /// <summary> Get the category or create a new one </summary>
    /// <param name="categoryName"> CategoryName </param>
    /// <returns> Found category </returns>
    private LogCategory GetCategory(string categoryName)
    {
        LogCategory category = null;

        categories ??= new List <LogCategory>();

        if (categories.Count > 0)
            category = categories.FirstOrDefault(c => c.categoryName.Equals(categoryName));

        if (category != null)
            return category;

        category = new LogCategory {categoryName = categoryName, logs = new List <LogEntry>()};
        categories.Add(category);

        return category;
    }

    /// <summary> Get all used packages </summary>
    private void GetUsedPackages()
    {
        if (!PackageCache.WasInitialized)
        {
            PackageCache.onCacheRefreshed += GetUsedPackages;
            PackageCache.Refresh();

            return;
        }

        PackageCache.onCacheRefreshed -= GetUsedPackages;

        usingPlayModeStartScene = PackageCache.IsInstalled(PackageKey.PlayModeStartScene);
        usingAutoSave = PackageCache.IsInstalled(PackageKey.AutoSave);
        usingScreenshot = PackageCache.IsInstalled(PackageKey.Screenshot);
        usingValidators = PackageCache.IsInstalled(PackageKey.Validators);
    }

    #endregion
}

}
#endif
