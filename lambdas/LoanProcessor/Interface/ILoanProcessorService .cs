using Amazon.Lambda.SQSEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanProcessor.Interface
{
    public interface ILoanProcessorService
    {
        Task ProcessLoanRequestAsync(SQSEvent sqsEvent, Amazon.Lambda.Core.ILambdaContext context);
    }
}
