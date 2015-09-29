using System;
using Finances.Core.Entities;

namespace Finances.Core.Factories
{
    public interface ITransferFactory
    {
        Transfer Create();
        void Release(Transfer s);
    }

}
