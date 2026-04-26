using RM.WorkFlow.Dtos;
using RM.WorkFlow.UnitOfWorks;

namespace RM.WorkFlow.Services.EntityService
{
    public class MaintenanceReports
    {

        public static List<FieldOptionsDto> GetReferencesByParentId(IUnitOfWork _unitOfWork, LookupParameterModel model)
        {
            return _unitOfWork.References.GetAll().Where(c => c.ParentId == model._referenceId).Select(c => new FieldOptionsDto
            {
                _key = c.Id,
                ValueAr = c.NameAr,
                ValueEn = c.NameEn

            }).ToList();
        }



        public static List<FieldOptionsDto> GetEntityDataByParentId(IUnitOfWork _unitOfWork, LookupParameterModel model)
        {
            return _unitOfWork.Entity.GetAll().Where(c => c.ParentId == model._entityId).Select(c => new FieldOptionsDto
            {
                _key = c.Id,
                ValueAr = c.NameAr,
                ValueEn = c.NameEn

            }).ToList();
        }



    }
}
