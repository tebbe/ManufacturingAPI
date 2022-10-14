using PPS.Shared.Service.Vm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PPS.Shared.Service.ServiceInterfaces
{
    public interface IEmailService
    {
        Task<bool> SendRegisterEmail(EmailVm vm);
    }
}