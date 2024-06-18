namespace c8_FileSystem
{
    internal class Program
    {
        //Напишите консольную утилиту для копирования файлов
        //Пример запуска: utility.exe file1.txt file2.txt
        static void CopyFile(string f1, string f2)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var file1 = Path.Combine(currentDirectory, f1);
            var file2 = Path.Combine(currentDirectory, f2);
            if (Path.Exists(file1))
            {
                if (!Path.Exists(file2))
                {
                    File.Copy(file1, file2);
                    Console.WriteLine("Файл был успешно копирован.");
                }
                else
                {
                    Console.WriteLine("Файл уже существует.");
                }

            }
            else
            {
                Console.WriteLine("Файл не существует.");
            }
        }
        //Напишите утилиту рекурсивного поиска файлов в заданном каталоге и подкаталогах
        static List<FileInfo> SearchFiles(string directory, string target)
        {
            List<FileInfo>files =new();
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            if(!directoryInfo.Exists)
            {
                Console.WriteLine("Заданный путь не существует.");
                return files;
            }
            try
            {
                files.AddRange(directoryInfo.GetFiles(target));
                foreach(var dir in directoryInfo.GetDirectories())
                {
                    files.AddRange(SearchFiles(dir.ToString(), target));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return files;
        }
        //Напишите утилиту читающую тестовый файл и выводящую на экран строки содержащие искомое слово.
        static List<string>? SearchStrings(string path, string target)
        {
            List<string> strings = new();
            if(!Path.Exists(path))
            {
                Console.WriteLine("Файл не найден.");
                return null;
            }
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string? line;
                    while((line = sr.ReadLine()) != null)
                    {
                        if(line.Contains(target, StringComparison.CurrentCultureIgnoreCase))
                            strings.Add(line);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return strings;
        }
        //Объедините две предыдущих работы(практические работы 2 и 3): поиск файла и поиск текста в файле написав утилиту которая ищет файлы определенного расширения с указанным текстом.Рекурсивно.Пример вызова утилиты: utility.exe txt текст.
        static IEnumerable<FileInfo> SearchFilesByExtensionAndText(string path, string extension, string text)
        {
            var  files = SearchFiles(path, extension).Where(x => ContainsTextInFile(x, text));
            return files;
        }
        static bool ContainsTextInFile(FileInfo fileInfo, string target) 
        {
            if (fileInfo == null || target == null)
                return false;
            try
            { 
                string text = File.ReadAllText(fileInfo.FullName);
                if (text.Contains(target, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        static void Main(string[] args)
        {
            //args = new string[] { "cs", "Передано неверное количество аргументов" };
            if (args.Length != 2) 
            {
                Console.WriteLine("Передано неверное количество аргументов.");
                return;
            }
            var directoryPath = Directory.GetParent(Directory.GetCurrentDirectory())?.ToString();
            if (directoryPath != null)
            {
                var result = SearchFilesByExtensionAndText(directoryPath, $"*.{args[0]}", args[1]);
                foreach (var file in result)
                    Console.WriteLine(file);
            }        
        }
    }
}