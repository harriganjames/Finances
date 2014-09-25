using Finances.Core.Entities;
using Finances.WinClient.ViewModels;

namespace Finances.WinClient.Mappers
{
    //Add Editor Bank
    //Upd Editor Bank
    //Del Item Bank
    //Read Bank Editor
    //Read Bank Item
    //List Bank Item



    public interface IBankMapper
    {
        Bank Map(IBankEditorViewModel from, Bank to);
        IBankItemViewModel Map(Bank from, IBankItemViewModel to);
        IBankEditorViewModel Map(Bank from, IBankEditorViewModel to);
    }

    public class BankMapper : IBankMapper
    {
        public Bank Map(IBankEditorViewModel from, Bank to)
        {
            to.BankId = from.BankId;
            to.Name = from.Name;
            to.Logo = from.LogoRaw;
            return to;
        }

        public IBankEditorViewModel Map(Bank from, IBankEditorViewModel to)
        {
            to.BankId = from.BankId;
            to.Name = from.Name;
            to.LogoRaw = from.Logo;
            return to;
        }

        public IBankItemViewModel Map(Bank from, IBankItemViewModel to)
        {
            to.BankId = from.BankId;
            to.Name = from.Name;
            to.LogoRaw = from.Logo;
            return to;
        }

    }
}
