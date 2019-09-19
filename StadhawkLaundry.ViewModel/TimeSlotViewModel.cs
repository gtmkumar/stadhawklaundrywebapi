using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StadhawkLaundry.ViewModel
{
    public class TimeSlotViewModel
    {
        public int Key { get; set; }
        public string FullDate { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string ShortMonth { get; set; }
        public List<TimeSlots> timeSlots { get; set; }
    }

    public class TimeSlots
    {
        public int SlotId { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string SlotRange { get; set; }
        public bool IsSlotAvailable { get; set; }
    }
}
