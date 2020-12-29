using ApplicationCore.Models;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IMembershipService
    {
        public List<Membership> FindMembership();


        public Membership FindMembership(Guid id);

        public int UpdateMembership(Guid id, MembershipParam param);

        public int AddMembership(Membership membership);

        public int DeleteMembership(Guid id);
    }
}
