using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Transaction
{
    class Program
    {
        static void Main(string[] args)
        {
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.Serializable;
            var ggg = "ggg";
            try
            {
                using (TransactionScope scopeSync = new TransactionScope(TransactionScopeOption.Required))
                {
                    ggg = "hghg";
                    string rr = null;
                    rr=rr.ToString();
                }
            }
            catch (Exception)
            {

            }

            
        }
    }
}
