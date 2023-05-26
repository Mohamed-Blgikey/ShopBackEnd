using Core.DTOS.Auth;
using Core.Interfaces;
using Infrastructure.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AddressRep : IAddressRep
    {
        private readonly StoreContext context;

        public AddressRep(StoreContext context)
        {
            this.context = context;
        }

        public async Task<AddressDTo> CreateAsync(AddressDTo address, string userID)
        {
            var x = await context.Addresses.FirstOrDefaultAsync(a => a.AppUserId == userID);
            if (x == null)
            {
                x = new Address
                {
                    AppUserId = userID,
                    City = address.City,
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    State = address.State,
                    Street = address.Street,
                    ZipCode = address.ZipCode,
                };
                await context.Addresses.AddAsync(x);
            }
            else
            {
                x.City = address.City;
                x.FirstName = address.FirstName;
                x.LastName = address.LastName;
                x.State = address.State;
                x.Street = address.Street;
                x.ZipCode = address.ZipCode;
                x.AppUserId = userID;
            }
            await context.SaveChangesAsync();
            return address;
        }

        public async Task<AddressDTo> GetAddress(string UserID)
        {
            var x = await context.Addresses.FirstOrDefaultAsync(a => a.AppUserId == UserID);
            var z = new AddressDTo
            {
                City = x.City,
                FirstName = x.FirstName,
                LastName = x.LastName,
                State = x.State,
                Street = x.Street,
                ZipCode = x.ZipCode,
            };
            return z;
        }
    }
}
