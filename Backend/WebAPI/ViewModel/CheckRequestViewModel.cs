using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebAPI.ViewModel {
    public class CheckRequest{
        public IFormFile File{get;set;}
        public bool PeerCheck{get;set;}
        public bool WebCheck{get;set;}
    }

    public class UploadMultiRequest
    {
        public List<IFormFile> Files { get; set; }
    }
}
