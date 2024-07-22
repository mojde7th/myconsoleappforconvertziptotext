using System;
using System.IO;
using System.IO.Compression;

namespace ZipFileReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the ZIP file
            string zipFilePath = @"D:\ABP Proj\ABP Cloned\BookStoreABP\For Converting to text\src20.zip";

            // Determine the output file path based on the ZIP file name
            string outputDirectory = Path.GetDirectoryName(zipFilePath);
            string zipFileNameWithoutExtension = Path.GetFileNameWithoutExtension(zipFilePath);
            string outputFilePath = Path.Combine(outputDirectory, $"{zipFileNameWithoutExtension}5.txt");

            // Extract and combine files
            ExtractAndCombineFiles(zipFilePath, outputFilePath);
        }

        static void ExtractAndCombineFiles(string zipFilePath, string outputFilePath)
        {
            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    using (StreamWriter writer = new StreamWriter(outputFilePath))
                    {
                        int fileNumber = 1;

                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.FullName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) ||
                                entry.FullName.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
                                ||
                                entry.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
                                ||
                                entry.FullName.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                                )
                            {
                                writer.WriteLine($"{fileNumber} - File: {entry.FullName}");
                                writer.WriteLine(new string('-', 80));

                                using (StreamReader reader = new StreamReader(entry.Open()))
                                {
                                    writer.Write(reader.ReadToEnd());
                                }

                                writer.WriteLine();
                                writer.WriteLine(new string('=', 80));
                                writer.WriteLine();

                                fileNumber++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Files have been extracted and combined into {outputFilePath}");
        }
    }
}
