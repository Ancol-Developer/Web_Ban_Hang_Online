using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace BanHangOnline.Common;

public class Common<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public Common(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static Common<T> CreateAsync(List<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new Common<T>(items, count, pageIndex, pageSize);
    }
}

public class Common
{
    private static string password = "xxxxxxx";
    private static string Email = "xxxxxx.com";

    public static bool SendMail(string name, string subject, string content,
        string toMail)
    {
        bool rs = false;
        try
        {
            MailMessage message = new MailMessage();
            var smtp = new SmtpClient();
            {
                smtp.Host = "smtp.gmail.com"; //host name
                smtp.Port = 587; //port number
                smtp.EnableSsl = true; //whether your smtp server requires SSL
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential()
                {
                    UserName = Email,
                    Password = password
                };
            }
            MailAddress fromAddress = new MailAddress(Email, name);
            message.From = fromAddress;
            message.To.Add(toMail);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = content;
            smtp.Send(message);
            rs = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            rs = false;
        }
        return rs;
    }
    public static string FormatNumber(object value, int SoSauDauPhay = 2)
    {
        bool isNumber = IsNumeric(value);
        decimal GT = 0;
        if (isNumber)
        {
            GT = Convert.ToDecimal(value);
        }
        string str = "";
        string thapPhan = "";
        for (int i = 0; i < SoSauDauPhay; i++)
        {
            thapPhan += "#";
        }
        if (thapPhan.Length > 0) thapPhan = "." + thapPhan;
        string snumformat = string.Format("0:#,##0{0}", thapPhan);
        str = String.Format("{" + snumformat + "}", GT);

        return str;
    }
    private static bool IsNumeric(object value)
    {
        return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
    }
}

