﻿using System;
using System.Net;
using System.IO;

namespace ConsoleFtpClient.Core
{
    public class FtpClient
    {
        //поля
        //поле для хранения имени фтп-сервера
        private string _Host;
 
        //поле для хранения логина
        private string _UserName;
 
        //поле для хранения пароля
        private string _Password;
 
        //объект для запроса данных
        FtpWebRequest ftpRequest;
 
        //объект для получения данных
        FtpWebResponse ftpResponse;
 
        //флаг использования SSL
        private bool _UseSSL = false;
 
        //фтп-сервер
        public string Host
        {
            get
            {
                return _Host;
            }
            set
            {
                _Host = value;
            }
        }
       //логин
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
        //пароль
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        //Для установки SSL-чтобы данные нельзя было перехватить
        public bool UseSSL
        {
            get
            {
                return _UseSSL;
            }
            set
            {
                _UseSSL = value;
            }
        }
        //Реализеум команду LIST для получения подробного списока файлов на FTP-сервере
        public FileStruct[] ListDirectory(string path)
        {
            if (path == null || path == "")
            {
                path = "/";
            }
            //Создаем объект запроса
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + _Host + path);
            //логин и пароль
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            //команда фтп LIST
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
 
            ftpRequest.EnableSsl = _UseSSL;
            //Получаем входящий поток
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
 
            //переменная для хранения всей полученной информации
            string content = "" ;
            
                StreamReader sr = new StreamReader(ftpResponse.GetResponseStream(), System.Text.Encoding.ASCII);
                content = sr.ReadToEnd();
                sr.Close();
            ftpResponse.Close();
 
            DirectoryListParser parser = new DirectoryListParser(content);
            return parser.FullListing;
        }
 
        // TODO Переписать для скачавания в нудный адрес
        //метод протокола FTP RETR для загрузки файла с FTP-сервера
        public void DownloadFile(string path, string fileName)
        {
 
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + _Host + path + "/"  + fileName);
 
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            //команда фтп RETR
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
 
            ftpRequest.EnableSsl = _UseSSL;
            //Файлы будут копироваться в кталог программы
            FileStream downloadedFile = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
 
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            //Получаем входящий поток
            Stream responseStream = ftpResponse.GetResponseStream();
 
            //Буфер для считываемых данных
            byte[] buffer = new byte[1024];
            int size=0;
 
            while ((size=responseStream.Read(buffer, 0, 1024))>0)
            {
                downloadedFile.Write(buffer, 0, size);
                 
            }
            ftpResponse.Close();
            downloadedFile.Close();
            responseStream.Close();
        }
        //метод протокола FTP STOR для загрузки файла на FTP-сервер
        public void UploadFile(string path, string fileName)
        {
            //для имени файла
            string shortName = fileName.Remove(0, fileName.LastIndexOf("\\" ) + 1);
 
            FileStream uploadedFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
             
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + _Host + path + shortName);
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            ftpRequest.EnableSsl = _UseSSL;
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
 
            //Буфер для загружаемых данных
            byte[] file_to_bytes = new byte[uploadedFile.Length];
            //Считываем данные в буфер
            uploadedFile.Read(file_to_bytes, 0, file_to_bytes.Length);
 
            uploadedFile.Close();
 
            //Поток для загрузки файла 
            Stream writer = ftpRequest.GetRequestStream();
 
            writer.Write(file_to_bytes, 0, file_to_bytes.Length);
            writer.Close();
        }
        //метод протокола FTP DELE для удаления файла с FTP-сервера 
        public void DeleteFile(string path)
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + _Host + path);
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            ftpRequest.EnableSsl = _UseSSL;
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
 
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
 
        //метод протокола FTP MKD для создания каталога на FTP-сервере 
        public void CreateDirectory(string path, string folderName)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + _Host + path + folderName);
             
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            ftpRequest.EnableSsl = _UseSSL;
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
 
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
        //метод протокола FTP RMD для удаления каталога с FTP-сервера 
        public void RemoveDirectory(string path)
        {
            string filename = path;
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + _Host + path);
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            ftpRequest.EnableSsl = _UseSSL;
            ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
 
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
    }
    //Для парсинга полученного детального списка каталогов фтп-сервера
    //Структура для хранения детальной информации о файде или каталоге

    //Класс для парсинга
}
