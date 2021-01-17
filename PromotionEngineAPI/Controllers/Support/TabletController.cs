using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TabletController : ControllerBase
    {
        private const string MOMO_SECRET_KEY = "tFuUcIVdHBxAPuawYOH5ORzCJX7wJ9Ol";

        [HttpPost]
        [Route("confirm")]
        public QRNotifyResponse qrConfirmPayment([FromBody] QRNotifyRequest qrNotifyRequest)
        {
            QRNotifyResponse result = new QRNotifyResponse
            {
                status = 0,
                amount = qrNotifyRequest.amount,
                message = "Thành công",
                partnerRefId = qrNotifyRequest.partnerRefId,
                momoTransId = qrNotifyRequest.momoTransId
            };
            result.signature = createSignature(result);
            Console.WriteLine(result.signature);
            return result;
        }

        private string createSignature(QRNotifyResponse result)
        {
            string mess = "";
            mess += "amount=" + result.amount;
            mess += "&message=" + result.message;
            mess += "&momoTransId=" + result.momoTransId;
            mess += "&partnerRefId=" + result.partnerRefId;
            mess += "&status=" + result.status;
            return hmacSha256Digest(mess, MOMO_SECRET_KEY);
        }
        private string hmacSha256Digest(string message, string secret)
        {
            UTF32Encoding encoding = new UTF32Encoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA256 cryptographer = new HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }


    }
}