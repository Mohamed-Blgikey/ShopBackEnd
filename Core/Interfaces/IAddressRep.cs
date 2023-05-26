using Core.DTOS.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAddressRep
    {
        Task<AddressDTo> CreateAsync(AddressDTo address, string userID);
        Task<AddressDTo> GetAddress(string UserID);
    }
}
