
using RM.FileSharing.Dtos;

namespace RM.FileSharing.Services
{
    public interface IFileSharingService
    {
        OperationOutput GetGroups(Dtos.FileSharingInfo RequestedData);
        OperationOutput GetGroupMembers(Dtos.FileSharingInfo RequestedData);
        OperationOutput UserOrGroupSearch(Dtos.FileSharingInfo RequestedData);
        Task<OperationOutput> GetDirectory(Dtos.FileSharingInfo RequestedData);

        OperationOutput GetSearchResult(FileSharingInfo RequestedData);
        OperationOutput GetDirFileAccessRules(Dtos.FileSharingInfo RequestedData);
        OperationOutput ChangeDirFileAccessRules(FileSharingInfo RequestedData);

        Task<OperationOutput> UploadFileAsync(Stream fileStream, string contentType);
        OperationOutput CreateDirectory(FileSharingInfo RequestedData);
        OperationOutput DeleteFileOrDirectory(List<FileSharingInfo> RequestedData);
        OperationOutput RenameFileOrDirectory(FileSharingInfo RequestedData);

        OperationOutput CopyFileOrDirectory(FilesSharingInfo RequestedData);

        OperationOutput MoveFileOrDirectory(FilesSharingInfo RequestedData);
        OperationOutput GetProccessStatus(FileSharingInfo RequestedData);

        OperationOutput CompressFiles(FilesSharingInfo RequestedData);


    }
}
