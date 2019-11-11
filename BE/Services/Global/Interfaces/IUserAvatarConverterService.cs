namespace BE.Interfaces
{
    public interface IAvatarConverterService
    {
        byte[] ConvertToByte(string path);
    }
}