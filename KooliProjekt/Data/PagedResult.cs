namespace KooliProjekt.Data
{
    public class PagedResult<T>:PagedResultBase
    {
        public List<T> Result { get; set; }
        public PagedResult()
        {
            Result = new List<T>();
        }
    }
}
