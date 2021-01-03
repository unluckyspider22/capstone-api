using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ChannelDto : BaseDto
    {
        public Guid ChannelId { get; set; }
        public Guid? BrandId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelCode { get; set; }
    }
}
