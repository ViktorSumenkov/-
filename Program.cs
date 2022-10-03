using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.IO.Compression;

namespace Lab_1_Lat
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Company { get; set; }
    }
    
    class Program
    {

        public static void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("Восстановлен файл: {0}", targetFile);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drivers)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }

            string path = @"D:\VS\MyTest.txt";
            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
                    fs.Write(info, 0, info.Length);
                }
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    string textFromFile = System.Text.Encoding.Default.GetString(array);
                    Console.WriteLine($"Текст из файла: {textFromFile}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Удалить файл? \nНажмите 1 чтобы удалить.");
            string num = Console.ReadLine();
            switch (num)
            {
                case "1":
                    Console.WriteLine("Файл удален");
                    File.Delete(path);
                    break;
                default:
                    Console.WriteLine("Файл сохранен");
                    break;
            }
            Person tom = new Person { Name = "Tom", Age = 35 };
            string json = JsonSerializer.Serialize<Person>(tom);
            using (FileStream fstream = new FileStream($"test.json", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }

            // чтение из файла
            using (FileStream fstream = File.OpenRead($"test.json"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");

                Person restoredPerson = JsonSerializer.Deserialize<Person>(textFromFile);
                Console.WriteLine(restoredPerson.Name);
            }
            Compress("compressme.xml", "zipfile.zip");

            // Создать архив в формате zip
            // Добавить файл, выбранный пользователем, в архив
            Console.WriteLine("Имя файла для сжатия:");
            var f_name = Console.ReadLine();
            if (f_name == null)
                return;

            Compress(f_name, "zipfile2.zip");

            Console.WriteLine("Разархивирование zipfile2.zip и вывод данных о нем.");
            Decompress("zipfile2.zip", "new_" + f_name);

            // Удалить файл и архив
            File.Delete("out-zip.zip");
            File.Delete("uncomp_" + f_name);
        }
    }
}

            