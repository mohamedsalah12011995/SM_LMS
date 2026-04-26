using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos
{
    /// <summary>
    /// <param name="ParamModel">The ParamModel It is used to send an object to the method that you want to call while drawing the control inside the form.</param>
    /// If using <<ParamModel>> you must call DeserializeInnerParamModel
    /// <<Example in Service >>
    ///  public List<FieldOptionsDto> GetEntityDataForAllReportType(LookupParameterModel model)
    /// {
    ///     MokeData paramModel = new MokeData();
    ///      paramModel = model.DeserializeInnerParamModel(paramModel);
    ///      return MaintenanceReports.GetAllEntityForReportType(_unitOfWork, paramModel);
    ///   }
    /// 
    /// </summary>
    public class LookupParameterModel
    {
        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        public int? _referenceId { get; set; }
        [JsonIgnore]
        public int? _entityId { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string ReferenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }
        public string EntityId { set { _entityId = Accessor.Set(value); } get { return Accessor.Get(_entityId); } }
        public dynamic ParamModel { get; set; }
    }

    public static class LookupParameterExtension
    {
        public static bool ArePropertiesIsNull(this LookupParameterModel obj)
        {
            return typeof(LookupParameterModel).GetProperties().All(propertyInfo => propertyInfo.GetValue(obj) == null);
        }

        public static T DeserializeInnerParamModel<T>(this LookupParameterModel obj, T modelName)
        {
            if (obj != null)
            {
                var innerModel = Convert.ToString(obj.ParamModel);

                if (innerModel != null)
                {
                    innerModel = innerModel.Replace("ValueKind = Object :", "");
                    var innerParamModel = System.Text.Json.JsonSerializer.Deserialize<T>(innerModel);
                    return innerParamModel;
                }

            }
            return (T)Convert.ChangeType(null, typeof(T)); ;


        }
    }
}
