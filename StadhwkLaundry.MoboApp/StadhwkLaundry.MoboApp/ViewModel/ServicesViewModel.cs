using System;
using System.Collections.Generic;
using System.Text;

namespace StadhwkLaundry.MoboApp.ViewModel
{
    public class ServicesViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public object CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public object ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public List<SubcategoryViewModel> Subcategories { get; set; }
    }
}
