using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

public class GoogleDriveHelper
{
    private static readonly string[] Scopes = { DriveService.Scope.DriveFile }; // Adjust the scope if necessary
    private static readonly string ApplicationName = "Medical Certificate For Patient";
    private static readonly string CredentialsPath = "credentials.json";
    private static readonly string TokenPath = "token.json";

    public string UploadFileToGoogleDrive(string filePath)
    {
        UserCredential credential;
        using (var stream = new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(TokenPath, true)).Result;
        }

        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = Path.GetFileName(filePath),
            MimeType = "image/png" // Update MIME type as per your file type
        };

        FilesResource.CreateMediaUpload request;
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            request = service.Files.Create(fileMetadata, stream, fileMetadata.MimeType);
            request.Fields = "id";
            request.Upload();
        }

        var file = request.ResponseBody;

        var permission = new Permission()
        {
            Type = "anyone",
            Role = "reader"
        };
        service.Permissions.Create(permission, file.Id).Execute();

        return $"https://drive.google.com/file/d/{file.Id}/view";
    }

    public static DriveService GetDriveService()
    {
        UserCredential credential;
        using (var stream = new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(TokenPath, true)).Result;
        }

        return new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }
}
