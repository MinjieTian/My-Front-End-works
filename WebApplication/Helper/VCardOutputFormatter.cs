using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using WebApplication.Dto;

namespace WebApplication.Helper
{
    public class VCardOutputFormatter : TextOutputFormatter
    {
        public VCardOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public async override Task<Task> WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            CardOut card = (CardOut)context.Object;
            string uid = card.UID + "";
            if (card.UID == 0)
            {
                uid = "";
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("BEGIN:VCARD");
            builder.AppendLine("VERSION:4.0");
            builder.Append("N:").AppendLine(card.N);
            builder.Append("FN:").AppendLine(card.FN);
            builder.Append("UID:").AppendLine(uid);
            builder.Append("ORG:").AppendLine(card.ORG);
            builder.Append("EMAIL;TYPE=work:").AppendLine(card.Email);
            builder.Append("TEL:").AppendLine(card.TEL);
            builder.Append("URL:").AppendLine(card.URL);
            builder.Append("CATEGORIES:").Append(card.Categories.Trim('\"')).AppendLine("");
            builder.Append("PHOTO;ENCODING=BASE64;TYPE=").Append(card.PhotoType).Append(":").AppendLine(card.Photo);
            builder.Append("LOGO;ENCODING=BASE64;TYPE=:").Append(card.PhotoType2).Append(":").AppendLine(card.Photo2);
            builder.AppendLine("END:VCARD");
            string outString = builder.ToString();
            byte[] outBytes = selectedEncoding.GetBytes(outString);
            var response = context.HttpContext.Response.Body;
            return response.WriteAsync(outBytes, 0, outBytes.Length);
        }
    }
}
