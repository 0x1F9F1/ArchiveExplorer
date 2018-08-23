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

#if true
            {
                var remoteFile = root.GetFile(@"https://www.sample-videos.com/zip/10mb.zip");

                var archive = (BasicArchive) archives.Open(remoteFile);

                foreach (var archiveFile in archive.Files)
                {
                    Console.WriteLine("File: {0}", archiveFile.Name);
                }

                var inputFile = archive.GetFile("big_buck_bunny_240p_10mb.mp4");
                var outputFile = root.GetFile(@"W:\test.mp4");

                inputFile.CopyTo(outputFile);
            }
#endif

#if false
            {
                var inputFile = root.GetFile(@"https://ci.appveyor.com/api/buildjobs/bcm5wji44wsg2eye/artifacts/Release-0.12.0.40.zip");
                var outputFile = root.GetFile(@"W:\test.zip");

                inputFile.CopyTo(outputFile);

                Console.WriteLine("Length: {0}", outputFile.Length);
            }
#endif
        }
    }
}
