using LinqKit;
using RM.Core.Helpers;
using RM.DynamicForms.Const;
using RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos;

namespace RM.DynamicForms.Services
{
    public static class AdvancedSearchService
    {

        public static ExpressionStarter<Models.FormValueDetails> FormValueDetailsFilteration(List<AdvancedSearch> advancedSearchList, int formId, List<int> formInputsDatasourceFromAPI, bool isExport)
        {
            var filter = PredicateBuilder.New<Models.FormValueDetails>(true);
            bool resultExcuted = false;
            if (!isExport)
                filter.And(u => u.FormInput.FormId == formId && u.FormInput.IsDeleted == false);

            else
                filter.And(u => u.FormInput.FormId == formId && u.FormInput.ShowInExport == true && u.FormInput.IsDeleted == false);

            var filterationTypes = advancedSearchList.Select(c => c.ItemType);

            if (filterationTypes.Intersect(TextInputs.GetTextInputs().Select(c => c.Type)).Any())

                resultExcuted = SearchForTextValues(advancedSearchList, ref filter, resultExcuted);

            if (filterationTypes.Contains(FormInputTypes.number))

                resultExcuted = SearchForNumericValues(advancedSearchList, ref filter, resultExcuted);

            if (filterationTypes.Contains(FormInputTypes.date))

                resultExcuted = SearchForDateValues(advancedSearchList, ref filter, resultExcuted);

            if (filterationTypes.Contains(FormInputTypes.time))

                resultExcuted = SearchForTimeValues(advancedSearchList, ref filter, resultExcuted);

            if (filterationTypes.Contains(FormInputTypes.datetime))

                resultExcuted = SearchForDateTimeValues(advancedSearchList, ref filter, resultExcuted);

            if (filterationTypes.Contains(FormInputTypes.hijriDate))

                resultExcuted = SearchForHijriDateValues(advancedSearchList, ref filter, resultExcuted);

            if (filterationTypes.Any(type => type == FormInputTypes.onecheckbox || type == FormInputTypes.inputSwitch))

                resultExcuted = SearchForBooleanValues(advancedSearchList, ref filter, resultExcuted);

            if (filterationTypes.Contains(FormInputTypes.rating))

                resultExcuted = SearchForRatingValues(advancedSearchList, ref filter, resultExcuted);

            if (formInputsDatasourceFromAPI.Any())
            {
                resultExcuted = SearchForAPIDatasourceValues(advancedSearchList, formInputsDatasourceFromAPI, ref filter, resultExcuted);
            }

            return filter;
        }


        public static ExpressionStarter<Models.FormValueDataSource> FormValueDatasourceFilteration(List<AdvancedSearch> inputs, bool isExport)
        {
            var filter = PredicateBuilder.New<Models.FormValueDataSource>(true);

            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    int? value = Accessor.Set(input.CompareValue.ToString().Replace("ValueKind = String : ", string.Empty));

                    if (value.HasValue)
                    {
                        if (!isExport)
                            filter.And(u => u.FormInput.IsDeleted == false && u.FormDataSourceId == value && u.InputKey == input._key);
                        else filter.And(u => u.FormInput.ShowInExport == true && u.FormInput.IsDeleted == false && u.FormDataSourceId == value && u.InputKey == input._key);
                    }
                }
            }
            return filter;

        }


        #region HELPER METHODS 
        static bool SearchForTextValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => TextInputs.GetTextInputs()
            .Exists(t => t.Type == c.ItemType)).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = input.CompareValue.ToString().Replace("ValueKind = String : ", string.Empty).Trim();
                    if (resultExcuted)
                        filter.Or(u => u.InputKey == input._key && u.InputValue.Contains(value));
                    else filter.And(u => u.InputKey == input._key && u.InputValue.Contains(value));
                    resultExcuted = true;
                }
            }
            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;

        }
        static bool SearchForNumericValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => c.ItemType == FormInputTypes.number).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = decimal.Parse(input.CompareValue.ToString().Replace("ValueKind = String : ", string.Empty));

                    switch (input.CompareType)
                    {
                        case "=":
                            {
                                if (resultExcuted)
                                    filter.Or(u => u.InputKey == input._key && u.NumericValue == value);
                                else filter.And(u => u.InputKey == input._key && u.NumericValue == value);
                            }
                            break;

                        case ">=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.NumericValue >= value);
                            else filter.And(u => u.InputKey == input._key && u.NumericValue >= value); break;

                        case "<=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.NumericValue <= value);
                            else filter.And(u => u.InputKey == input._key && u.NumericValue <= value); break;

                        case "!=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.NumericValue != value);
                            else filter.And(u => u.InputKey == input._key && u.NumericValue != value); break;
                    }
                    resultExcuted = true;
                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;
        }
        static bool SearchForDateValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => c.ItemType == FormInputTypes.date).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = DateTime.Parse(input.CompareValue.ToString().Replace("ValueKind = String : ", string.Empty));

                    switch (input.CompareType)
                    {
                        case "=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date == value.Date);
                            else
                                filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date == value.Date);

                            break;
                        case ">=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date >= value.Date);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date >= value.Date);

                            break;
                        case "<=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date <= value.Date);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date <= value.Date);


                            break;
                        case "!=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date != value.Date);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date != value.Date);

                            break;
                    }
                    resultExcuted = true;
                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;


        }
        static bool SearchForTimeValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => c.ItemType == FormInputTypes.time).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = DateTime.Parse(input.CompareValue.ToString().Replace("ValueKind = String : ", string.Empty));

                    if (resultExcuted)
                        filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.TimeOfDay.Hours == value.TimeOfDay.Hours && u.DateTimeValue.Value.TimeOfDay.Minutes == value.TimeOfDay.Minutes);
                    else
                        filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.TimeOfDay.Hours == value.TimeOfDay.Hours && u.DateTimeValue.Value.TimeOfDay.Minutes == value.TimeOfDay.Minutes);
                    resultExcuted = true;
                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;


        }
        static bool SearchForDateTimeValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => c.ItemType == FormInputTypes.datetime).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = DateTime.Parse(input.CompareValue.ToString().Replace("ValueKind = String : ", string.Empty)).AddSeconds(0);

                    switch (input.CompareType)
                    {
                        case "=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) == value);
                            else
                                filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) == value);

                            break;
                        case ">=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) >= value);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) >= value);

                            break;
                        case "<=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) <= value);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) <= value);


                            break;
                        case "!=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) != value);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.AddSeconds(0) != value);

                            break;
                    }
                    resultExcuted = true;
                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;


        }
        static bool SearchForHijriDateValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => c.ItemType == FormInputTypes.hijriDate).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = Dates.ConvertFromHijriToGerogian(Convert.ToString(input.CompareValue).Replace("ValueKind = String :", ""));

                    switch (input.CompareType)
                    {
                        case "=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date == value.Date);
                            else
                                filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date == value.Date);

                            break;
                        case ">=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date >= value.Date);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date >= value.Date);

                            break;
                        case "<=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date <= value.Date);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date <= value.Date);


                            break;
                        case "!=":
                            if (resultExcuted)
                                filter.Or(u => u.InputKey == input._key && u.DateTimeValue.Value.Date != value.Date);
                            else filter.And(u => u.InputKey == input._key && u.DateTimeValue.Value.Date != value.Date);

                            break;
                    }
                    resultExcuted = true;

                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;


        }
        static bool SearchForBooleanValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => c.ItemType == FormInputTypes.onecheckbox
                                            || c.ItemType == FormInputTypes.inputSwitch).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = bool.Parse(input.CompareValue.ToString().Replace("ValueKind = String :", ""));

                    if (resultExcuted)
                        filter.Or(u => u.InputKey == input._key && u.BooleanValue == value);

                    else filter.And(u => u.InputKey == input._key && u.BooleanValue == value);
                    resultExcuted = true;
                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;
        }
        static bool SearchForRatingValues(List<AdvancedSearch> advancedSearchList, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => c.ItemType == FormInputTypes.rating).ToList();
            if (inputs.Any())
            {
                foreach (var input in inputs)
                {

                    if (resultExcuted)
                        filter.Or(u => u.InputKey == input._key && u.InputValue.ToString() == input.CompareValue.ToString().Replace("ValueKind = String :", ""));

                    else filter.And(u => u.InputKey == input._key && u.InputValue.ToString() == input.CompareValue.ToString().Replace("ValueKind = String :", ""));

                    resultExcuted = true;
                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;
        }

        static bool SearchForAPIDatasourceValues(List<AdvancedSearch> advancedSearchList, List<int> formInputsDatasourceFromAPI, ref ExpressionStarter<Models.FormValueDetails> filter, bool resultExcuted)
        {
            var inputs = advancedSearchList.Where(c => DatasourceInputs.GetDatasourceInputs()
                              .Exists(t => t.Type == c.ItemType) && formInputsDatasourceFromAPI.Contains(c._key.Value)).ToList();

            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    var value = input.CompareValue.ToString().Replace("ValueKind = String :", "");

                    if (resultExcuted)
                        filter.Or(u => u.InputKey == input._key && u.InputValue.ToString().Contains(value.Trim()));

                    else filter.And(u => u.InputKey == input._key && u.InputValue.ToString().Contains(value.Trim()));

                    resultExcuted = true;

                }
            }

            return !resultExcuted ? inputs.Count() > 0 : resultExcuted;
        }

        #endregion

    }
}
