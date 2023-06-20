using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core.Constants
{
    public static class AppConstants
    {
        public static class AzureBlob
        {
            public const string AudioBlobContainerName = "audios";
            public const string ImageBlobContainerName = "audio-images";
        }

        public static class CacheConstants
        {
            public const string GenreCacheKey = "GenreCacheKey";
        }

        public static class GeneralConstants
        {
            public const int ItemsPerSection = 20;
        }
    }
}