using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.Azure.WebJobs;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ImageFunctionApp
{
    public static class ImageStoreFunction
    {
        [FunctionName("ImageStoreFunction")]
        public static void Run(
            [BlobTrigger("images/{name}", Connection = "AzureWebJobsStorage")] Stream inputBlob,
            [Blob("imagesthumbnails/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputBlob,
            string name,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            using (Image<Rgba32> image = Image.Load<Rgba32>(inputBlob))
            {
                image.Mutate(i =>
                    i.Resize(new ResizeOptions { Size = new Size(250, 250), Mode = ResizeMode.Max }).Grayscale()
                );
                image.Save(outputBlob, new JpegEncoder());
            }
        }

    }
}
