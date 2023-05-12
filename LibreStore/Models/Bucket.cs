namespace LibreStore.Models;

public class Bucket{
    public Int64 Id{get;set;}
    
    // We don't want this value exposed to end user
    [System.Text.Json.Serialization.JsonIgnore]
    public Int64 MainTokenId{get;set;}
    // Intent is the purpose of the data, or can be used as an app name or app id, etc.
    public String? Intent{get;set;}
    public String Data{get;set;}
    public String Hmac{get;set;}
    public String Iv {get;set;}
    public DateTime? Created {get;set;}
    public DateTime? Updated{get;set;}
    public bool Active{get;set;}

    public Bucket(Int64 id, Int64 mainTokenId){
        Id = id;
        MainTokenId = mainTokenId;
        Data = String.Empty;
        Hmac = String.Empty;
        Iv = String.Empty;
        Intent = null;
    }

    public Bucket(Int64 mainTokenId, String? intent, String data, String hmac, String iv)
    {
        MainTokenId = mainTokenId;
        Data = data;
        Hmac = hmac;
        Iv = iv;
        if (intent != null){
            // insuring intent is not empty and is 20 chars or less
            // User will never know because it truncates anything over 20
            if (intent.Length > 20){
                this.Intent = intent.Substring(0,20);
            }
            else{
                this.Intent = intent;
            }
        }
    }

    public Bucket(Int64 id, Int64 mainTokenId, String? intent,
        String data, String hmac, String iv,
        String created, string updated, bool active) : this(mainTokenId,intent,data,hmac,iv)
    {
        Id = id;
        Created = created != String.Empty ? DateTime.Parse(created) : null;
        Updated = updated != String.Empty ? DateTime.Parse(updated) : null;
        Active = active;
        Intent = null;
    }
    
}