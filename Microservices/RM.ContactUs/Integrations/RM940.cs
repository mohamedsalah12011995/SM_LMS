
using System.Collections.Generic;
using System.Linq;

namespace RM.ContactUs.Integrations
{
    public class RM940
    {

        AmanaIntegration.AmanaReceiverMobileSoapClient amanaReceiver = new AmanaIntegration.AmanaReceiverMobileSoapClient(AmanaIntegration.AmanaReceiverMobileSoapClient.EndpointConfiguration.AmanaReceiverMobileSoap);
        string _userName;
        string _password;
        List<Dtos.Amana940Category> categories = null;


        public RM940(string UserName,string Password)
        {
            _userName = UserName;
            _password= Password;
        }

        public async Task<List<Dtos.Amana940Category>> GetCategoriesListAsync()
        {
            try
            {
                var catgAsync = await amanaReceiver.CategoryListAsync(_userName, _password);
                categories = catgAsync.Select(a => new Dtos.Amana940Category
                {
                    DescAr = a.CategoryDesc,
                    DescEn = a.CategoryDescEn,
                    ID = a.CategoryID
                }).ToList();
            }
            catch (System.Exception)
            {

                return null;
            }
            return categories;
        }
        public async Task<List<Dtos.Amana940Category>> GetCategoriesListAsync(string CategoryId)
        {
            try
            {
                var subCatgAsync = await amanaReceiver.SubCategoryListAsync(CategoryId, _userName, _password);

                categories = subCatgAsync.Select(a => new Dtos.Amana940Category
                {
                    DescAr = a.SubCategoryDesc,
                    DescEn = a.SubCategoryDescEn,
                    ID = a.SubCategoryID
                }).ToList();
            }
            catch (System.Exception)
            {

                return null;
            }
            return categories;
        }
    }
}
