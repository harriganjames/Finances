namespace Finances.Core.Entities
{
    public class Bank
    {
        public int BankId { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }

        public bool HasLogo
        {
            get
            {
                return !(Logo == null);
            }
        }

    }
}
