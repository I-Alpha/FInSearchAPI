namespace FInSearchAPI.Models.Responses.Result
{
    public interface IResponseResult<T>
    {
        T GetData => (T)this;
    }
}