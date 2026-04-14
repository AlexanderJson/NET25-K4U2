// To run this code you need to install the following dependencies:
// dotnet add package Google.GenAI

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.GenAI;
using Google.GenAI.Types;

public class 
{
    static void SaveBinaryFile(string fileName, byte[] data)
    {
        File.WriteAllBytes(fileName, data);
        Console.WriteLine($"File saved to: {fileName}");
    }

    public static async Task Main(string[] args)
    {

    }

    static string GetFileExtension(string mimeType)
    {
        return mimeType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "audio/wav" => ".wav",
            "audio/mpeg" => ".mp3",
            _ => ".bin"
        };
    }
}


