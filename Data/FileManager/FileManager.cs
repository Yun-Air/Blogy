﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PhotoSauce.MagicScaler;

namespace Blogy.Data.FileManager //: IFileManager
{
  //  private string _imagePath;
    public class FileManager : IFileManager
    {
        private string _imagePath;

        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Images"];
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
            //throw new NotImplementedException();
        }

        public bool RemoveImage(string image)
        {
            try
            {

                var file = Path.Combine(_imagePath, image);
                if (File.Exists(file))
                    File.Delete(file);
                    return true;
                //throw new NotImplementedException();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
            var save_path = Path.Combine(_imagePath);
            if(!Directory.Exists(save_path))
            {
                Directory.CreateDirectory(save_path);
            }

            //var filename = image.FileName //Internet Explorer Error
            //throw new NotImplementedException();
            var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";

            using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
            {
                //await image.CopyToAsync(fileStream);
                    MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, ImageOptions());
            }

          
            return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }
        private ProcessImageSettings ImageOptions() => new ProcessImageSettings
        {
            Width = 800,
            Height = 500,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 100,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420,

        };
    }
}
