
using System.Collections.Generic;
using System.Linq;

namespace RM.OpenData.Integrations
{
    public class OpenData
    {

        private bool IsTransaction = false;
        OpenDataSvc.OpendataServicesClient opendataServicesClient = new ();
        public OpenData()
        {

        }

        public bool InsertRequest(Dtos.OpenDataRequest openDataRequest)
        {
            try
            {

            return opendataServicesClient.InsertRequest(openDataRequest.Name, openDataRequest.Email, openDataRequest.Mobile, openDataRequest.Address, openDataRequest.Details);
            }
            catch (System.Exception)
            {
                return false;
            }


        }
        public List<Dtos.OpenDataStats> GetOpenDataStats()
        {
            List<Dtos.OpenDataStats> OpenDataStats=new ();
            try
            {

                OpenDataStats= opendataServicesClient.GetOpenDataStats().Select(x=>new Dtos.OpenDataStats { 
                                  Name=x.Name,
                                  Number=x.Number
                                  }).ToList();
            }
            catch (System.Exception ex)
            {
                return OpenDataStats;
            }
            return OpenDataStats;

        }


    }
}
