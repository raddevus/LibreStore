namespace LibreStore.Models;

public class Usage{
    public Int64 Id{get;set;}
    
    public Int64 MainTokenId{get;set;}
    public String IpAddress{get;set;}
    
    public String Action{get;set;}
    public DateTime Created {get;set;}
    public bool Active{get;set;}

    public Usage(Int64 mainTokenId, String ipAddress, String action="")
    {
        MainTokenId = mainTokenId;
        IpAddress = ipAddress;
        Action = action;
        Created = DateTime.Now;
        Active = true;
    }
}