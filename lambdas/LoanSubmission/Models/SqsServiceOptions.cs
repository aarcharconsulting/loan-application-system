using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanSubmission.Models
{
    public class SqsServiceOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string RequestQueueUrl { get; set; }
        public string ResponseQueueUrl { get; set; }
    }

}
