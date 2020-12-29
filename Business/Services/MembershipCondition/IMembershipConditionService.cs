using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IMembershipConditionService
    {
        public List<MembershipCondition> GetMembershipConditions();

        public MembershipCondition FindMembershipCondition(Guid id);

        public int AddMembershipCondition(MembershipCondition param);
        public int UpdateMembershipCondition(Guid id, MembershipConditionParam param);

        public int DeleteMembershipCondition(Guid id);

        
    }
}
