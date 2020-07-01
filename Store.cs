using System;

public class Store{

    public Int32 ID {get;set;}
    public Int32 MainTokenId {get;set;}
    // MainTokenKey is just for the DTO (data transfer object)
    // It is not stored in the DB in the Store table
    public String MainTokenKey{get;set;}
    public String Data {get;set;}
    public DateTime Created {get;set;}
    public DateTime Updated {get;set;}
    public bool Active{get;set;}
    
    public Store()
    {
        
    }
    public Store(Int32 id, String mainTokenKey)
    {
        this.ID = id;
        this.MainTokenKey = mainTokenKey;
        this.Created = new DateTime();
        this.Active = true;
    }
}