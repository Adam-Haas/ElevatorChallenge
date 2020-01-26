using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge
{
    public class WeightOverloadException : Exception
    {
        public WeightOverloadException(string message, int overloadedAmount) :base(message)
        {
            OverloadedAmount = overloadedAmount;
        }

        public int OverloadedAmount { get; set; }
    }
}
