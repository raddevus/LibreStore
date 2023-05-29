namespace LibreStore.Models;

public class Owner{
public Int64 Id{get;set;}
    
    public Int64 ID{get;set;}
    public String? Email{get;set;}
    public DateTime? ExpireDate {get;set;}
    public bool Subscribed{get;set;}
    public DateTime? Created {get;set;}
    public DateTime? Updated{get;set;}
    public bool Active{get;set;}
}