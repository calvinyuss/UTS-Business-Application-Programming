using System;
using System.Text;
using System.Security.Cryptography;

namespace DAL.BLL
{
    static class Helper
    {
        internal static string SHA256ComputeHash(string rawData)
        {
            string result = "";
            try
            {
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in bytes)
                    {
                        sb.Append(item.ToString("X2"));
                    }
                    result = sb.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
