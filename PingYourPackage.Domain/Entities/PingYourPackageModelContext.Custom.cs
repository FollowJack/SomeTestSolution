using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PingYourPackage.Domain.Entities
{
    public partial class Entities
    {

        #region Request-wide Singleton

        private const string CONTEXTKEY_DATACONTEXT = "{CD01E2F2-3893-4580-8D19-9C948B6F33BD}";

        public static Entities Current
        {
            get
            {
                Entities result =
                    HttpContext.Current.Items[CONTEXTKEY_DATACONTEXT] as Entities;
                if (result == null)
                {
                    result = new Entities();
                    HttpContext.Current.Items.Add(CONTEXTKEY_DATACONTEXT, result);
                }
                return result;
            }
        }

        #endregion
    }
}