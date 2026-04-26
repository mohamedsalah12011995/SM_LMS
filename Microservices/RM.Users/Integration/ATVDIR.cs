using System;
using System.DirectoryServices.AccountManagement;

namespace RM.Users.Integration
{
    public class ATVDIR
    {
        public static bool ValidateCredentials(string AdDomain,string userName, string password)
        {
            try
            {
                using (var adContext = new PrincipalContext(ContextType.Domain, AdDomain))
                {
                    return adContext.ValidateCredentials(userName, password);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
