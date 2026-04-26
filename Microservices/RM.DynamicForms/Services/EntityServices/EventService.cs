using RM.DynamicForms.Dtos.DynamicForm.TemplateDtos;
using RM.DynamicForms.UnitOfWorks;

namespace RM.DynamicForms.Services.EntityServices
{
    // add any methods specific to events entity like <<getlookupdata()>> and call it from DynamicFormService
    public static class EventService
    {

        // Example 
        public static List<FieldOptions> GetReferencesByParentId(IUnitOfWork _unitOfWork, LookupParameterModel model)
        {
            return _unitOfWork.References.GetAll().Where(c => c.ParentId == model._referenceId).Select(c => new FieldOptions
            {
                _key = c.Id,
                ValueAr = c.NameAr,
                ValueEn = c.NameEn

            }).ToList();
        }

        public static List<FieldOptions> GetEntityDataByParentId(IUnitOfWork _unitOfWork, LookupParameterModel model)
        {
            return _unitOfWork.Entity.GetAll().Where(c => c.ParentId == model._entityId).Select(c => new FieldOptions
            {
                _key = c.Id,
                ValueAr = c.NameAr,
                ValueEn = c.NameEn

            }).ToList();
        }

        public static List<FieldOptions> GetAllEntityForReportType(IUnitOfWork _unitOfWork, MokeData model)
        {

            return _unitOfWork.Entity.GetAll().Where(c => c.ParentId == model.Id).Select(c => new FieldOptions
            {
                _key = c.Id,
                ValueAr = c.NameAr,
                ValueEn = c.NameEn

            }).ToList();
        }
    }
}
