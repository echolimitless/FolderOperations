namespace FolderOperations;

internal static class MoveToFolderOneLevelUp
{
    private static readonly char separator = Path.DirectorySeparatorChar;
    static void Main(string[] args)
    {
        Console.WriteLine("最下層のフォルダにあるファイルを一つ上の階層に移動します。");
        Console.WriteLine("対象フォルダを入力してください。");
        var path = Console.ReadLine();

        if (Directory.Exists(path))
        {
            var targetDirectory = new DirectoryInfo(path);

            var targetFileList = targetDirectory.EnumerateFiles("*", SearchOption.AllDirectories);
            var deleteDirectoryHashSet = new HashSet<string>();

            Console.WriteLine("ファイルの移動を行います。");
            foreach (var targetFile in targetFileList)
            {
                deleteDirectoryHashSet.Add(targetFile.DirectoryName!);
                targetFile.MoveTo(targetFile.DirectoryName + "_" + targetFile.Name);
                Console.WriteLine($"{targetFile.FullName} → {targetFile.DirectoryName + "_" + targetFile.Name}(ファイル移動)");

            }

            Console.WriteLine("フォルダの削除を行います。");
            foreach (var deleteDirectory in deleteDirectoryHashSet)
            {
                if (!Directory.EnumerateFileSystemEntries(deleteDirectory).Any())
                {
                    Directory.Delete(deleteDirectory);
                    Console.WriteLine($"{deleteDirectory}(フォルダ削除)");
                }
            }
        }
        else
        {
            Console.WriteLine("入力した文字列はフォルダのパスではないためプログラムを終了します。");
        }

        Console.WriteLine("何かキーを押すと終了します。");
        Console.ReadKey();
    }
}