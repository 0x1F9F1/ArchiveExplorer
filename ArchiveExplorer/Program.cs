using System;
using Archive.Local;
using Archive.Root;
using Archive.Web;
using Archive.Common;

namespace Archive
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new RootDevice();

            root.AddDevice(new LocalDevice());
            root.AddDevice(new HttpDevice());

            var archives = new ArchiveManager();

            archives.AddType(new ZipArchiveType());

            // var file = root.GetFile(@"W:\Notes\VFS.txt");
            // var file = root.GetFile(@"https://www.sample-videos.com/text/Sample-text-file-10kb.txt");
            // var file = root.GetFile(@"awdads");
            var file = root.GetFile(@"https://www.sample-videos.com/zip/10mb.zip");

            var archive = (BasicArchive) archives.Open(file);

            foreach (var name in archive.FileNames)
            {
                Console.WriteLine("File: {0}", name);
            }

            var inputFile = archive.GetFile("big_buck_bunny_240p_10mb.mp4");
            var outputFile = root.GetFile(@"W:\test.mp4");

            inputFile.CopyTo(outputFile);
        }
    }
}
