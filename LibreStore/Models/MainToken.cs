namespace LibreStore.Models;

public class MainToken{
    public Int64 Id{get;set;}
    public Int64 OwnerId{get;set;}
    public String Key{get; set;}
    public DateTime Created {get;set;}
    public bool Active{get;set;}

    public MainToken(String key, Int64 ownerId=0)
    {
        OwnerId = ownerId;
        Key = key;
        Created = DateTime.Now;
        Active = true;
    }

    public MainToken(Int64 id, String key,  DateTime created, Int64 ownerId, bool active)
    {
        Id = id;
        OwnerId = ownerId;
        Key = key;
        Created = created;
        Active = true;
    }
}