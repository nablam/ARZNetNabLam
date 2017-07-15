// @Author Jeffrey M. Paquette ©2016

using HoloToolkit.Unity;
using UnityEngine;
using System;
using System.Collections;
using System.IO;

#if !UNITY_EDITOR
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class WaveData {

    string fileName;
    string folderName;
    string fileExtension = ".wcfg";

    public WaveData(string fileName)
    {
#if !UNITY_EDITOR
        folderName = ApplicationData.Current.RoamingFolder.Path;
#else
        folderName = Application.persistentDataPath;
#endif
        this.fileName = fileName;
        Load();
    }

    /// <summary>
    /// Opens the specified file for reading.
    /// </summary>
    /// <param name="folderName">The name of the folder containing the file.</param>
    /// <param name="fileName">The name of the file, including extension. </param>
    /// <returns>Stream used for reading the file's data.</returns>
    Stream OpenFileForRead(string folderName, string fileName)
    {
        Stream stream = null;

#if !UNITY_EDITOR
            Task<Task> task = Task<Task>.Factory.StartNew(
                            async () =>
                            {
                                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderName);
                                StorageFile file = await folder.GetFileAsync(fileName);
                                stream = await file.OpenStreamForReadAsync();
                            });
            task.Wait();
            task.Result.Wait();
#else
        stream = new FileStream(Path.Combine(folderName, fileName), FileMode.Open, FileAccess.Read);
#endif
        return stream;
    }

    /// <summary>
    /// Opens the specified file for writing.
    /// </summary>
    /// <param name="folderName">The name of the folder containing the file.</param>
    /// <param name="fileName">The name of the file, including extension.</param>
    /// <returns>Stream used for writing the file's data.</returns>
    /// <remarks>If the specified file already exists, it will be overwritten.</remarks>
    Stream OpenFileForWrite(string folderName, string fileName)
    {
        Stream stream = null;

#if !UNITY_EDITOR
            Task<Task> task = Task<Task>.Factory.StartNew(
                            async () =>
                            {
                                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderName);
                                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                                stream = await file.OpenStreamForWriteAsync();
                            });
            task.Wait();
            task.Result.Wait();
#else
        stream = new FileStream(Path.Combine(folderName, fileName), FileMode.Create, FileAccess.Write);
#endif
        return stream;
    }

    public void Save()
    {
        using (Stream stream = OpenFileForWrite(folderName, fileName + fileExtension))
        {
            // Serialize and write the meshes to the file.
            byte[] data = Serialize();
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }
    }

    public void Load()
    {
        try
        {
            using (Stream stream = OpenFileForRead(folderName, fileName + fileExtension))
            {
                // Read the file and deserialize the meshes.
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                Deserialize(data);
            }
        } catch (Exception)
        {
            Save();
        }
    }

    byte[] Serialize()
    {
        byte[] data = null;

        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                WriteData(writer);

                stream.Position = 0;
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
        }

        return data;
    }

    void Deserialize(byte[] data)
    {
        using (MemoryStream stream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                ReadData(reader);
            }
        }
    }

    public virtual void WriteData(BinaryWriter writer)
    {
    }

    public virtual void ReadData(BinaryReader reader)
    {
    }
}
