using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.API.Shared.ViewModel.File
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public Guid FileGuid { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int FileTypeId { get; set; }
        public string Description { get; set; }
    }
}