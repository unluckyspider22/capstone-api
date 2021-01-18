
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class ChannelService : BaseService<Channel, ChannelDto>, IChannelService
    {
        public ChannelService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Channel> _repository => _unitOfWork.ChannelRepository;
    }
}
