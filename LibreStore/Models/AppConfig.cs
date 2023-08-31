using System.Collections;

public class AppConfig: IEnumerable{

    static public String  ConnectionDetails{get;set;}
    /// <summary>
    /// User can now add a DB password at command line, which
    /// will replace the <PwdPlaceholder> in the appsettings.json
    /// When starting app from command line, user can opt to add
    /// DB password like $ dotnet run <YourPassword> -- no quotes or angle brackets needed
    /// However, user can still provide password in appsettings.json connectionDetails if desired
    /// </summary>
    /// <param name="config"></param>
    /// <param name="dbPassword"></param>
    public AppConfig(IConfiguration config, String dbPassword ="")
    {
        ConnectionDetails = config["connectionDetails"];
        if (!String.IsNullOrEmpty(dbPassword)){
            ConnectionDetails = ConnectionDetails.Replace("<PwdPlaceholder>", dbPassword,StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}