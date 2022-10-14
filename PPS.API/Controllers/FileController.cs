using PPS.API.HelperClasses;
using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel.File;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/File")]
    public class FileController : BaseApiController
    {
        ILogger _logger;
        private readonly string workingFolder = HttpRuntime.AppDomainAppPath + @"\Uploads";

        public FileController()
        {
            _logger = new Logger();
        }

        [Route("UploadFile")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            try
            {
                var provider = new MultipartFormDataStreamProvider(workingFolder);
                //await Request.Content.ReadAsMultipartAsync(provider);
                await Task.Run(async () => await Request.Content.ReadAsMultipartAsync(provider));

                var fileVm = new FileViewModel();
                if(provider.FileData.Count > 0)
                {
                    var data = provider.FileData.FirstOrDefault();
                    var fileInfo = new FileInfo(data.LocalFileName);
                    fileVm = new FileViewModel
                    {
                        FileGuid = Guid.NewGuid(),
                        FileName = fileInfo.Name,
                        FileTypeId = (int)FileTypeEnum.Pdf,
                        FileSize = fileInfo.Length / 1024
                    };
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, fileVm);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
