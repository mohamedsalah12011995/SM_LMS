using RM.Core.Services;
using RM.ScientificLetters.Dtos;

namespace RM.ScientificLetters.Services
{
    public interface IScientificLettersService:IBaseService
    {
        Task<OperationOutput> GetScientificLettersLookups();
        Task<OperationOutput> GetScientificLettersList(Dtos.ScientificLetters RequestedData);
        Task<OperationOutput> GetScientificLettersDetails(Dtos.ScientificLetters RequestedData);
        Task<OperationOutput> SaveScientificLetters(Dtos.ScientificLetters RequestedData);
        Task<OperationOutput> ModelActions(Dtos.ScientificLetters RequestedData);
    }
}
