using PPS.API.Shared.ViewModel.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Service.ServiceInterfaces
{
    public interface IFileService
    {
        FileViewModel SaveFile(FileViewModel fileVm);
    }
}