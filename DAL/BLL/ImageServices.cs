/**
 * Hanlding store image to traget Directory
 * Author Calvin Yuslianto
 * 2020 Calvin Yuslianto
 * 
 * lol tentu saja file ini tidak ada error handlingnya 
 * handling sendiri ya pek
 * bo sempat co a tan wkkwkwkw
 */
using System;
using System.IO;

namespace DAL.BLL
{
    public class ImageServices
    {
        private string imageUrl = null;
        private string targetDirectory = @"\resources\images\";

        public string createdFileName { get; set;}

        public ImageServices(string imageUrl)
        {
            this.imageUrl = imageUrl;
        }

        public void storeImage()
        {
            if (!isFileExist()) throw new FileNotFoundException("Image not found");

            saveToTargetDirectory();
        }

        private void saveToTargetDirectory()
        {
            createDirectoryIfNotExists();

            string fileNameToBeSaved = getHashFileName();

            File.Copy(imageUrl, targetDirectory + fileNameToBeSaved);

            createdFileName = fileNameToBeSaved;
        }

        private string getHashFileName()
        {
            return  hashFileName() + getFileExtension();
        }

        private string hashFileName()
        {
            return Helper.SHA256ComputeHash(getFileNameWithoutExtension() + DateTimeOffset.Now.ToUnixTimeSeconds());
        }

        private string getFileExtension()
        {
            return Path.GetExtension(imageUrl);
        }


        private string getFileNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(imageUrl);
        }

        private void createDirectoryIfNotExists()
        {
            if (Directory.Exists(targetDirectory) == false)
            {
                Directory.CreateDirectory(targetDirectory);
            }
        }

        private bool isFileExist()
        {
            return File.Exists(imageUrl);
        }
    }
}
