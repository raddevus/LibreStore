namespace LibreStore.Models;

public class Cya{
    public Int64 Id{get;set;}
    
    // We don't want this value exposed to end user
    [System.Text.Json.Serialization.JsonIgnore]
    public Int64 MainTokenId{get;set;}
    public String Data{get;set;}
    public String Hmac{get;set;}
    public String Iv{get;set;}
    public DateTime? Created {get;set;}
    public DateTime? Updated{get;set;}
    public bool Active{get;set;}

    public Cya(Int64 mainTokenId){
        MainTokenId = mainTokenId;
    }
    public Cya(Int64 id, Int64 mainTokenId){
        Id = id;
        MainTokenId = mainTokenId;
    }

    public Cya(Int64 mainTokenId, String data, String hmac, String iv)
    {
        MainTokenId = mainTokenId;
        Data = data;
        Hmac = hmac;
        Iv = iv;
    }

    public Cya(Int64 id, Int64 mainTokenId, 
        String data, String hmac, String iv,
        String created, string updated, bool active)
    {
        Id = id;
        MainTokenId = mainTokenId;
        Data = data;
        Hmac = hmac;
        Iv = iv;
        Created = created != String.Empty ? DateTime.Parse(created) : null;
        Updated = updated != String.Empty ? DateTime.Parse(updated) : null;
        Active = active;
    }
    
}