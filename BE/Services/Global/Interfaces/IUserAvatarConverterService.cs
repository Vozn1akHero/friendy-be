namespace BE.Interfaces
{
    public interface IUserAvatarConverterService
    {
        byte[] ConvertToByte(string path);
    }
}