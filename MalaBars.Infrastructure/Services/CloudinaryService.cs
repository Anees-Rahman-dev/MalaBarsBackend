using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MalaBars.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Text;

namespace MalaBars.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> settings)
        {
            var account = new Account(
                settings.Value.CloudName,
                settings.Value.ApiKey,
                settings.Value.ApiSecret
             );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Image is required");

            await using var stream = image.OpenReadStream();

            var uploadParams = new ImageUploadParams
                {
                  File = new FileDescription(image.FileName, stream),
                  Folder = "malabars/products"
                };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception(result.Error.Message);

            return result.SecureUrl.ToString();
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return false;

            var deleteParams = new DeletionParams(publicId);

            var res = await _cloudinary.DestroyAsync(deleteParams);

            return res.Result == "ok";
        }
    }
}
