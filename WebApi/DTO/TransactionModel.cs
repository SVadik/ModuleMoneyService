using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTO
{
    public class TransactionModel
    {
        public long FromNumber { get; set; }
        public long ToNumber { get; set; }
        public int UserId { get; set; }
        public decimal Value { get; set; }
        public char Sign { get; set; }
    }
}
