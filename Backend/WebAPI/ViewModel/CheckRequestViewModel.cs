using Microsoft.AspNetCore.Http;

namespace WebAPI.ViewModel {
    public class CheckRequest{
        public IFormFile File{get;set;}
        public bool PeerCheck{get;set;}
        public bool WebCheck{get;set;}
    }
}
