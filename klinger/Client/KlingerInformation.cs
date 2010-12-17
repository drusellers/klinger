namespace klinger.Client
{
    using System.Diagnostics;

    public class KlingerInformation
    {
            public string Version
            {
                get { return FileVersionInfo.GetVersionInfo(GetType().Assembly.Location).FileVersion; }
            }
    }
}