namespace RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos
{
    public class TextInputs
    {
        public string Type { get; set; }

        public static List<TextInputs> GetTextInputs()
        {
            return new List<TextInputs>
            {
                 new TextInputs {Type="text"},
                 new TextInputs {Type="textarea"},
                 new TextInputs {Type="editor"},
                 new TextInputs {Type="autoCode"},
                 new TextInputs {Type="inputOtp"}
            };
        }
    }

    public class DatasourceInputs
    {
        public string Type { get; set; }

        public static List<DatasourceInputs> GetDatasourceInputs()
        {
            return new List<DatasourceInputs>
            {
                 new DatasourceInputs {Type="radio"},
                 new DatasourceInputs {Type="checkbox"},
                 new DatasourceInputs {Type="dropdownSingle"},
                 new DatasourceInputs {Type="dropdownMultiple"},
                 new DatasourceInputs {Type="plistbox"}
            };
        }
    }
}
