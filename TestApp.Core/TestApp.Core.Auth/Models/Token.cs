using System;
using System.Collections.Generic;

namespace TestApp.Core.Auth.Models
{
    public partial class Token
    {
        public Guid Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string Username { get; set; }
    }
}
