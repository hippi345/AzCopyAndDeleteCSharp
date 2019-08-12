using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.DataMovement;

namespace AzCopyAndDelete
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the simple data backup app w/ option to delete");
            Console.WriteLine("What is your storage account name?");
            string SAName = Console.ReadLine();
            Console.WriteLine("Cool deal and what is your storage account key?");
            string SAKey = Console.ReadLine();
            Console.WriteLine("Coolio and what is the container/path you are wanting to target?");
            string container = Console.ReadLine();
            Console.WriteLine("Oh and one last thing. Where is the file or folder you want to upload to Storage?");
            string sourceFile = Console.ReadLine();

            Console.WriteLine("Cool let's get this party started");

            string connectionString = $"DefaultEndpointsProtocol=https;AccountName={SAName};AccountKey={SAKey};EndpointSuffix=core.windows.net";

            CloudStorageAccount storageAccount; 

            if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
                Console.WriteLine("Uploading to Blob storage as blob '{0}'", sourceFile);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(sourceFile);
                await cloudBlockBlob.UploadFromFileAsync(sourceFile);

                Console.WriteLine("Right on bro if you made it this far. You want to list the blobs just to make sure you got it in storage safely? y/n");
                String answer = Console.ReadLine();
                if (answer.Equals("y") || answer.Equals("yes"))
                {
                    await listTheBlobs(cloudBlobContainer);
                }
                else if (answer.Equals("n") || answer.Equals("no"))
                {
                    Console.WriteLine("I feel that yo. We can move on to whether you want to delete the files so they don't take up disk space");
                }
                else
                {
                    Console.WriteLine("I did not quite catch that but we will move on to whether you want to delete files");
                }

                Console.WriteLine("Hey so since you got your files in storage, you may not want em on your local machine anymore. Want to delete them? y/n");

                String deleteReply = Console.ReadLine();

                if (deleteReply.Equals("y") || deleteReply.Equals("yes"))
                {
                    await deleteTheFile(sourceFile);
                }
                else if (deleteReply.Equals("n") || deleteReply.Equals("no"))
                {
                    Console.WriteLine("I feel that yo. We can move on to whether you want to delete the files so they don't take up disk space");
                }
                else
                {
                    Console.WriteLine("I did not quite catch that but we will move on to whether you want to delete files");
                }

                Console.WriteLine("well looks like we reached the end good sir or madam. Have a good one");
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Big bummer bro. That connection string did not come through for us.");
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
        }
        static async Task listTheBlobs(CloudBlobContainer cloudBlobContainer)
        {
            // List the blobs in the container.
            Console.WriteLine("List blobs in container.");
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                BlobResultSegment results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    Console.WriteLine(item.Uri);
                }
            } while (blobContinuationToken != null); // Loop while the continuation token is not null.
        }
        static async Task deleteTheFile(string source)
        {
                if (File.Exists(source))
                {
                    File.Delete(source);
                }
        }
    }
}
