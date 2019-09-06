using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class ServiceLabelMasterResponseViewModel
    {
        public ServiceLabelMasterResponseViewModel()
        {
            ServiceMaster = new List<ServiceMasterResponseViewModel>();
        }
        public int Id { get; set; }
        public string LabelName { get; set; }      
        public List<ServiceMasterResponseViewModel> ServiceMaster { get; set; }

    }
}
