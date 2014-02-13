using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Enroll.Managers;
using Telerik.Web.Zip;

public class ZipLibrary
{
    public List<ZipFile> Files;
    public string ZipFileName;

    public ZipLibrary(string zipFileName)
    {
        ZipFileName = zipFileName;
    }

    /// <summary>
    /// ZipFile yapsındaki Files nesnesi dışarıdan yollanan klasör yolu ile doldurulur ve zip olarak indirilir.
    /// </summary>
    public void ZipFolder(string directoryPath, string folderMap)
    {
        Files = new List<ZipFile>();
        var memStream = new MemoryStream();
        var package = ZipPackage.Create(memStream);
        foreach (var file in GetDirectories(directoryPath, folderMap))
        {
            Stream stream = new MemoryStream(File.ReadAllBytes(Path.GetFullPath(file.Path)));
            package.AddStream(stream, file.Name);
            stream.Close();
        }

        SendZipToClient(memStream);

        memStream.Close();
        package.Close(true);
    }


    /// <summary>
    /// ZipFile yapsındaki Files nesnesi dışarıdan "path&name" alanları doldurulark gönderilirse zip indirilir.
    /// </summary>
    public void ZipFile()
    { 
        var memStream = new MemoryStream();
        var package = ZipPackage.Create(memStream);
        foreach (var file in Files)
        {
            Stream stream = new MemoryStream(File.ReadAllBytes(file.Path));
            package.AddStream(stream, file.Name);
            stream.Close();
        }

        SendZipToClient(memStream);

        memStream.Close();
        package.Close(true);
    }

    public List<ZipFile> GetDirectories(string directoryName, string folderMap)
    {
        foreach (var file in Directory.GetFiles(directoryName))
        {
            var fileName = (folderMap + "\\" + Path.GetFileName(file)).TrimStart('\\').TrimEnd('\\');
            var f = new ZipFile
                        {
                            Name = fileName,
                            Path = Path.GetFullPath(file)
                        };

            try
            {
                var fileStream = File.OpenRead(Path.GetFullPath(file));
                if (fileStream.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        ms.SetLength(fileStream.Length);
                        ms.Write(File.ReadAllBytes(Path.GetFullPath(file)), 0, (int) fileStream.Length);
                        fileStream.Close();
                        f.Data = ms.ToArray();
                        ms.Close();
                    }
                    Files.Add(f);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        var directories = Directory.GetDirectories(directoryName);
        if (directories.Count() > 0)
        {
            var newFolderMap = folderMap;
            foreach (var directory in directories)
            {
                var s = directory.Split('\\');
                var newDirectoryName = s[s.Length - 1];
                newFolderMap = (folderMap + "\\" + newDirectoryName).TrimStart('\\').TrimEnd('\\');
                Files = GetDirectories(directory + "\\", newFolderMap);
            }
        }
        return Files;
    }

    public void SendZipToClient(MemoryStream memStream)
    {
        memStream.Position = 0;

        if (memStream.Length > 0)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + ZipFileName + ".zip");
            HttpContext.Current.Response.ContentType = "application/zip";
            HttpContext.Current.Response.BinaryWrite(memStream.ToArray());
            HttpContext.Current.Response.End();
        }
        memStream.Close();
    }
}

public class ZipFile
{
    private static int _counter;
    private readonly object Key = new object();

    public ZipFile()
    {
        Id = GetId();
    }

    public string Path { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }
    public int Id { get; private set; }

    protected int GetId()
    {
        lock (Key)
        {
            _counter++;
        }
        return _counter;
    }
}