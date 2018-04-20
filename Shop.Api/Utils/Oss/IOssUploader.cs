namespace Shop.Api.Utils.Oss
{
    public interface IOssUploader
    {
        string PutObjectFromFile(string bucketName, string ossPath, string fileToUpload);
        string SyncAppendObject(string bucketName, string fileToUpload);
    }
}
