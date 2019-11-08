using System;

namespace sendITAPI.Models
{
    public class Parcel
    {
        public long Id { get; set; }
        public string ParcelDestination { get; set; }
        public string PickUpAddress { get; set; }
        public enum Status
        {
            Pending,
            Cancelled,
            Delivered
        }
        
        public long UserId { get; set; }
        public User Owner { get; set; }
    }
}