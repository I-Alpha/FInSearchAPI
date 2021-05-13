namespace FInSearchAPI.Interfaces
{
    public interface IRequest
    {
         int Id { get; set; }    
         string Type { get; }
         string BaseUrl { get; set; }   
         string Parameters { get; set; }
        
    }
}