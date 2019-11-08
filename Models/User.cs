using System;
using System.Collections.Generic;

namespace sendITAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public List<Parcel> Parcels { get; set; }
        
    }
}
