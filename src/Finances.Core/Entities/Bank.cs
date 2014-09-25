namespace Finances.Core.Entities
{
    public class Bank
    {
        public virtual int BankId { get; set; }
        public virtual string Name { get; set; }
        public virtual byte[] Logo { get; set; }
    }
}
