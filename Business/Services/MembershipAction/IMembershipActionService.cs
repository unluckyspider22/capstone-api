using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IMembershipActionService
    {
        public List<MembershipAction> GetMembershipActions();

        public MembershipAction FindMembershipAction(Guid id);

        public int AddMembershipAction(MembershipAction param);
        public int UpdateMembershipAction(Guid id, MembershipActionParam param);

        public int DeleteMembershipAction(Guid id);
    }
}
