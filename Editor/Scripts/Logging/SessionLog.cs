using System;

namespace MegaPint.Editor.Scripts.Logging
{

[Serializable]
internal class SessionLog
{
    public string sessionID;
    public string sessionStartTime;
    public string sessionEndTime;

    public SessionLog(string sessionID)
    {
        this.sessionID = sessionID;
        sessionStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void EndSession()
    {
        sessionEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

}
