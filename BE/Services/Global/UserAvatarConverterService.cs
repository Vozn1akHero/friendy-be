using System.IO;
using BE.Interfaces;

namespace BE.Services
{
    public class UserAvatarConverterService : IUserAvatarConverterService
    {
        public byte[] ConvertToByte(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = File.ReadAllBytes(path);
                fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();

                return bytes;
            }
        }
    }
}