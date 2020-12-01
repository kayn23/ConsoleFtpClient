using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using ConsoleFtpClient.Controllers;

namespace ConsoleFtpClient.Core
{
    public class FtpClient: IClient
    {
        FtpWebRequest ftpRequest;
        FtpWebResponse ftpResponse;

        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; } = false;

        //Реализеум команду LIST для получения подробного списока файлов на FTP-сервере
        public List<Item> ListDirectory(string path)
        {
            if (path == null || path == "")
            {
                path = "/";
            }
            //Создаем объект запроса
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + Host + path);
            //логин и пароль
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            //команда фтп LIST
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
 
            ftpRequest.EnableSsl = UseSsl;
            //Получаем входящий поток
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
 
            //переменная для хранения всей полученной информации
            string content = "" ;
            
                StreamReader sr = new StreamReader(ftpResponse.GetResponseStream(), System.Text.Encoding.ASCII);
                content = sr.ReadToEnd();
                sr.Close();
            ftpResponse.Close();
 
            return FileParser.Parse(content);
        }
        
        //метод протокола FTP STOR для загрузки с FTP-сервер
        public void UploadFile(string pathTo, string pathFrom)
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + Host + pathFrom);
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpRequest.EnableSsl = UseSsl;
            string shortName = pathFrom.Remove(0, pathFrom.LastIndexOf("/") + 1);
            FileStream downloadedFile = new FileStream((pathTo + "\\" + shortName).Replace('/', '\\'),
                FileMode.Create, FileAccess.ReadWrite);
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            //Получаем входящий поток
            Stream responseStream = ftpResponse.GetResponseStream();
 
            //Буфер для считываемых данных
            byte[] buffer = new byte[1024];
            int size=0;
 
            while ((size = responseStream.Read(buffer, 0, 1024))>0)
            {
                downloadedFile.Write(buffer, 0, size);
                 
            }
            ftpResponse.Close();
            downloadedFile.Close();
            responseStream.Close();
        }
        //метод протокола FTP DELE для удаления файла с FTP-сервера 
        public void DeleteFile(string path)
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + Host + path);
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.EnableSsl = UseSsl;
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
 
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
 
        //метод протокола FTP MKD для создания каталога на FTP-сервере 
        public void CreateDirectory(string path, string folderName)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + Host + path + folderName);
             
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.EnableSsl = UseSsl;
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
 
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
        //метод протокола FTP RMD для удаления каталога с FTP-сервера 
        public void RemoveDirectory(string path)
        {
            string filename = path;
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + Host + path);
            ftpRequest.Credentials = new NetworkCredential(UserName, Password);
            ftpRequest.EnableSsl = UseSsl;
            ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
 
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
    }
}
