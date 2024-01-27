using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace SignalRYoutube.Models
{
    public class TblUser
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Dept { get; set; } = string.Empty;
    }
}
