namespace eShop.Core.Entities
{
    public class Image : BaseEntity
    {
        public string imgPath { get; set; }
        public Guid TargetId { get; set; }
    }
}
