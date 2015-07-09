namespace Finances.Core.Entities
{
    public class TransferCategory
    {
        public int TransferCategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
