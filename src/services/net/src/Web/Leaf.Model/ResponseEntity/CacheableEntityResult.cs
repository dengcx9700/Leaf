namespace Leaf.Model.ResponseEntity
{
    public class CacheableEntityResult<T> : EntityResult<T>
    {
        public long CacheTime { get; set; }
    }
    public class PageableCacherEntityResult<T>:CacheableEntityResult<T>
    {
        public int Skip { get; set; }

        public int Take { get; set; }

        public long Total { get; set; }
    }
}
