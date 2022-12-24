namespace FolderOperations;

internal static class MoveToFolderOneLevelUp
{
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
                if (string.IsNullOrWhiteSpace(targetFile.DirectoryName))
                {
                    throw new NullReferenceException();
                }

                if (targetFile.Directory?.Parent == null)
                {
                    throw new NullReferenceException();
                }

                var a = targetFile.Directory.Name;

                // 空になるディレクトリパスを保存
                deleteDirectoryHashSet.Add(targetFile.DirectoryName);

                // CDのアートファイルのため削除
                if (targetFile.Extension == ".jpg")
                {
                    targetFile.Delete();
                    continue;
                }

                var oldFileFullname = targetFile.FullName;
                var newFileFullname = $"{targetFile.DirectoryName}_{targetFile.Name.Replace(".mp4", ".m4a")}";

                // 親ディレクトリ名は20文字にする
                if (targetFile.Directory.Name.Length > 20)
                {
                    newFileFullname = $"{Path.Combine(targetFile.Directory.Parent.FullName,targetFile.Directory.Name[..20])}…_{targetFile.Name.Replace(".mp4", ".m4a")}";

                    // 親ディレクトリ名を短くしたので同名のファイルが作成されていたら連番を付けて同名ファイルが作成されるのを回避
                    for (int i = 1; File.Exists(newFileFullname); i++)
                    {
                        newFileFullname = $"{Path.Combine(targetFile.Directory.Parent.FullName, targetFile.Directory.Name[..20])}…{i}_{targetFile.Name.Replace(".mp4", ".m4a")}";
                    }
                }

                targetFile.MoveTo(newFileFullname);
                Console.WriteLine($"{oldFileFullname} → {targetFile.FullName}(ファイル移動)");
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