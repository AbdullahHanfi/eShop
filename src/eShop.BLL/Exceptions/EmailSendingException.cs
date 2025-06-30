using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.BLL.Exceptions
{
    public class EmailSendingException : Exception
    {
        public EmailSendingException(string ErrorMassage) : base(ErrorMassage) { }
        public EmailSendingException(string ErrorMassage, Exception innerException) : base(ErrorMassage, innerException) { }
    }
}
