using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanSubmission.Models
{
    public class ProviderResponse
    {

        public LoanDetails LoanDetails { get; set; }
        public int ProviderId { get; set; }
        public string RequestId { get; set; }
        public string ProviderName { get; set; }

    }
}
