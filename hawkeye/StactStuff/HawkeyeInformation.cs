namespace hawkeye.StactStuff
{
    using System.Diagnostics;

    public class HawkeyeInformation
    {
        
            public string Version
            {
                get { return FileVersionInfo.GetVersionInfo(GetType().Assembly.Location).FileVersion; }
            }
        
    }
}