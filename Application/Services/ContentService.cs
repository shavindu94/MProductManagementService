using Application.DtoObjects;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Common.AWS;

namespace Application.Services
{
    public class ContentService : IContentService
    {
        private static string accessKey = "";
        private static string accessSecret = "";
        private static string bucket = "";
        private static string awsBucketUrl = "";

        public async Task<string> SaveContent(FileModel fileModel)
        {
            if (fileModel.FormFile != null && fileModel.FormFile.Length > 0)
            {
                var response = await UploadObject(fileModel.FormFile);

                if (response.Success)
                {
                    string filePath = awsBucketUrl + response.FileName;
                    return filePath;
                }
                else
                {
                    throw new Exception("Image Upload failed");
                }                
            }
            else
            {
                throw new Exception("No file selected");

            }

        }

        public async Task<string> RemoveContent(string fileUrl)
        {
            if (!string.IsNullOrWhiteSpace(fileUrl))
            {
                string fileName = fileUrl.Replace(awsBucketUrl, "");

                var response = await RemoveObject(fileName);

                if (!response.Success) ////// need to change this
                {
                    return "Successfully Removed";
                }
                else
                {
                   throw new Exception("Image delete failed");
                }
            }
            else
            {
                throw new Exception("No file to delete");

            }

        }


        public static async Task<GetObjectModel> GetObject(string name)
        {
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.EUCentral1);

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = name
            };
            var responseObject = await client.GetObjectAsync(request);
            return new GetObjectModel
            {

                ContentType = responseObject.Headers.ContentType,
                Content = responseObject.ResponseStream
            };
        }

        public static async Task<UploadObjectModel> UploadObject(IFormFile file)
        {
            // connecting to the client
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast2);

            byte[] fileBytes = new Byte[file.Length];
            file.OpenReadStream().Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));

            var fileName = Guid.NewGuid() + file.FileName;

            PutObjectResponse response = null;

            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                response = await client.PutObjectAsync(request);
            };

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                // this model is up to you, in my case I have to use it following;
                return new UploadObjectModel
                {
                    Success = true,
                    FileName = fileName
                };
            }
            else
            {
                return new UploadObjectModel
                {
                    Success = false,
                    FileName = fileName
                };
            }
        }

        public static async Task<UploadObjectModel> RemoveObject(String fileName)
        {
            var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.USEast2);

            var request = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = fileName
            };

            var response = await client.DeleteObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return new UploadObjectModel
                {
                    Success = true,
                    FileName = fileName
                };
            }
            else
            {
                return new UploadObjectModel
                {
                    Success = false,
                    FileName = fileName
                };
            }
        }


    }
}
