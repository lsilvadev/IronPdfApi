using System.Collections.Generic;

namespace IronApi.Models
{
    public class ApiStamp
    {
        public string FileBase64 { get; set; }
        public bool ReverseY { get; set; }
        public List<ApiStampField> Fields { get; set; }
    }
}
