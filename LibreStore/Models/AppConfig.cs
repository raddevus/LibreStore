using System.Collections;

public class AppConfig: IEnumerable{

    static public String  ConnectionDetails{get;set;}
    public AppConfig(IConfiguration config)
    {
        ConnectionDetails = config["connectionDetails"];
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}