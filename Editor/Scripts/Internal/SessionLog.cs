﻿using System;
using System.Collections.Generic;
using System.Linq;
using MegaPint.Editor.Scripts.DevMode;
using MegaPint.Editor.Scripts.PackageManager.Cache;
using MegaPint.Editor.Scripts.PackageManager.Packages;

namespace MegaPint.com.tiogiras.megapint_batesting.Editor.Scripts.Internal
{

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
    
    public List <LogCategory> categories;

    public SessionLog(string sessionID)
    {
        this.sessionID = sessionID;
        sessionStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        GetUsedPackages();
    }

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

    #region Public Methods

    public void EndSession()
    {
        sessionEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void Log(string categoryName, string logText)
    {
        DevLog.Log($"Added log: {logText}\n to category: {categoryName}");

        LogCategory category = GetCategory(categoryName);
        category.logs.Add(new LogEntry {logText = logText, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")});
    }

    #endregion

    #region Private Methods

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

    #endregion
}

}