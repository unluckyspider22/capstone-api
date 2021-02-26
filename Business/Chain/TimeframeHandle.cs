using ApplicationCore.Request;
using ApplicationCore.Utils;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Calendar.v3.Data;
using ShaNetHoliday.Engine.Standard;
using ShaNetHoliday.Models;
namespace ApplicationCore.Chain
{
    public interface ITimeframeHandle : IHandler<OrderResponseModel>
    {

    }
    public class TimeframeHandle : Handler<OrderResponseModel>, ITimeframeHandle
    {
        public override void Handle(OrderResponseModel order)
        {
            var promotions = order.Promotions;
            foreach (Promotion promotion in promotions)
            {
                //HandleExpiredDate(promotion, order);
                HandleHoliday(promotion, order);
                HandleDayOfWeek(promotion, order.OrderDetail.BookingDate.DayOfWeek);
                HandleHour(promotion, order.OrderDetail.BookingDate.Hour);
            }
            base.Handle(order);
        }
     
        public void HandleHoliday(Promotion promotion, OrderResponseModel order)
        {
            //           string[] scopes = new string[] {
            //    CalendarService.Scope.Calendar //, // Manage your calendars
            //	//CalendarService.Scope.CalendarReadonly // View your Calendars
            //};
            //           string filePath = "zinc-chiller-304407-f53ac846c4d6.json";

            //           var service = ServiceAccountExample.AuthenticateServiceAccount("nguyen-do@zinc-chiller-304407.iam.gserviceaccount.com", filePath, scopes);
            var countriesAvailable = HolidaySystem.Instance.CountriesAvailable;
            foreach (Country country in countriesAvailable)
            {
                Console.WriteLine(country.Languages);
            }
        }
        public void HandleDayOfWeek(Promotion promotion, DayOfWeek dayOfWeekStr)
        {
            int dayOfWeekNum = 0;
            switch (dayOfWeekStr.ToString())
            {
                case "Monday": dayOfWeekNum = 1; break;
                case "Tuesday": dayOfWeekNum = 2; break;
                case "Wednesday": dayOfWeekNum = 4; break;
                case "Thursday": dayOfWeekNum = 8; break;
                case "Friday": dayOfWeekNum = 16; break;
                case "Saturday": dayOfWeekNum = 32; break;
                case "Sunday": dayOfWeekNum = 64; break;
            }
            if (!Common.CompareBinaryForDay(dayOfWeekNum, promotion.DayFilter))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_DayInWeek);
            }
        }
        public void HandleHour(Promotion promotion, int hourOfDay)
        {
            if (!Common.CompareBinaryForHour(hourOfDay, promotion.HourFilter))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_HourFrame);
            }
        }
        //    public void AuthServiceAccountGoogleCalendar() {
        //        string[] scopes = new string[] { CalendarService.Scope.Calendar };
        //        GoogleCredential credential;
        //        using (var stream = new FileStream("zinc-chiller-304407-9606873199e6.p12", FileMode.Open, FileAccess.Read))
        //        {
        //            credential = GoogleCredential.FromStream(stream)
        //                             .CreateScoped(scopes);
        //        }

        //        // Create the Calendar service.
        //        var service = new CalendarService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = "Calendar Authentication Sample",
        //        });
        //    }
        //}
        public static class ServiceAccountExample
        {

            /// <summary>
            /// Authenticating to Google calender using a Service account
            /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
            /// </summary>
            /// Both param pass from webform1.aspx page on page load
            /// <param name="serviceAccountEmail">From Google Developer console https://console.developers.google.com/projectselector/iam-admin/serviceaccounts </param>
            /// <param name="serviceAccountCredentialFilePath">Location of the .p12 or Json Service account key file downloaded from Google Developer console https://console.developers.google.com/projectselector/iam-admin/serviceaccounts </param>
            /// <returns>AnalyticsService used to make requests against the Analytics API</returns>

            public static CalendarService AuthenticateServiceAccount(string serviceAccountEmail, string serviceAccountCredentialFilePath, string[] scopes)
            {

                try
                {
                    if (string.IsNullOrEmpty(serviceAccountCredentialFilePath))
                        throw new Exception("Path to the service account credentials file is required.");
                    if (!File.Exists(serviceAccountCredentialFilePath))
                        throw new Exception("The service account credentials file does not exist at: " + serviceAccountCredentialFilePath);
                    if (string.IsNullOrEmpty(serviceAccountEmail))
                        throw new Exception("ServiceAccountEmail is required.");

                    // For Json file
                    if (Path.GetExtension(serviceAccountCredentialFilePath).ToLower() == ".json")
                    {
                        GoogleCredential credential;
                        //using(FileStream stream = File.Open(serviceAccountCredentialFilePath, FileMode.Open, FileAccess.Read, FileShare.None))


                        using (var stream = new FileStream(serviceAccountCredentialFilePath, FileMode.Open, FileAccess.Read))
                        {
                            credential = GoogleCredential.FromStream(stream)
                                 .CreateScoped(scopes).CreateWithUser("xyz@gmail.com");//put a email address from which you want to send calendar its like (calendar by xyz user )
                        }

                        // Create the  Calendar service.
                        return new CalendarService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential,
                            ApplicationName = "Calendar_Appointment event Using Service Account Authentication",
                        });
                    }
                    else if (Path.GetExtension(serviceAccountCredentialFilePath).ToLower() == ".p12")
                    {   // If its a P12 file

                        var certificate = new X509Certificate2(serviceAccountCredentialFilePath, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
                        var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
                        {
                            Scopes = scopes
                        }.FromCertificate(certificate));

                        // Create the  Calendar service.
                        return new CalendarService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential,
                            ApplicationName = "Calendar_Appointment event Using Service Account Authentication",

                        });
                    }
                    else
                    {
                        throw new Exception("Something Wrong With Service accounts credentials.");
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Create_Service_Account_Calendar_Failed", ex);
                }
            }


        }
    }
}

