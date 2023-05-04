using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAppApi.Models.Enums;

namespace MusicAppApi.Models.Responses
{
    public class AzureUploadResponse
    {
        public AzureUploadStatuses UploadStatus { get; set; }
        public string BlobUrl { get; set; }
    }
}
