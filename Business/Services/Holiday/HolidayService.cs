
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Holiday;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class HolidayService : BaseService<Holiday, HolidayDto>, IHolidayService
    {
        private IConfiguration _config;
        public HolidayService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            _config = configuration;
        }

        protected override IGenericRepository<Holiday> _repository => _unitOfWork.HolidayRepository;

        public async Task<List<Holiday>> GetHolidays()
        {
            var result = await _repository.Get();
            return result.ToList();
        }

        public async Task<string> SyncHolidays()
        {
            string apiKey = _config.GetValue<string>("AppSettings:GoogleApiKey");
            string API = _config.GetValue<string>("AppSettings:HolidayAPI");

            GoogleHolidayResponse holidayResponse = null;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string url = string.Format("{0}?key={1}", API, apiKey);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                response.Content.Headers.ContentType.CharSet = @"utf-8";
                var responseBody = await response.Content.ReadAsStringAsync();
                holidayResponse = JsonConvert.DeserializeObject<GoogleHolidayResponse>(responseBody);
            }
            if (holidayResponse != null)
            {
                var items = holidayResponse.Items;
                var now = Common.GetCurrentDatetime();
                foreach (var item in items)
                {
                    var entity = new Holiday
                    {
                        Date = item.Start.Date,
                        HolidayName = item.Summary,
                        InsDate = now,
                        UpdDate = now
                    };
                    _repository.Add(entity);
                }
                await _unitOfWork.SaveAsync();
            }
            return apiKey;
        }
    }
}
