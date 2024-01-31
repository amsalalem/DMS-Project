using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.Data
{
    public class EmailSMTPSetting : BaseEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Host { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsEnableSSL { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public bool IsDefault { get; set; }
     
    }
}
