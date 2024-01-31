using Microsoft.Extensions.Configuration;
using System;

namespace DocumentManagement.Helper
{
    public class PathHelper
    {
        public IConfiguration _configuration;

        public PathHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string DocumentPath
        {
            get
            {
                return _configuration["DocumentPath"];
            }
        }
        public string AesEncryptionKey
        {
            get
            {
                return _configuration["AesEncryptionKey"];
            }
        }
        public bool AllowEncryption
        {
            get
            {
                return Convert.ToBoolean(_configuration["AllowEncryption"]);
            }
        }
        public string[] CorsUrls
        {
            get
            {
                return string.IsNullOrEmpty(_configuration["CorsUrls"]) ? new string[] { } : _configuration["CorsUrls"].Split(",");
            }
        }
    }
}
