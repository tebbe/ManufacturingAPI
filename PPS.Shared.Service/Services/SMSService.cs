using PPS.Shared.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using PPS.Shared.Service.Vm;
using PPS.API.Shared.Utility;

namespace PPS.Shared.Service.Services
{
    public class SMSService : ISMSService
    {
        public async Task<bool> SendSMS(SMSVm vm)
        {
            try
            {
                var sendSms = ConfigUtils.GetSafeAppSettingValue("SendSMS");
                var smsEnabled = false;
                bool.TryParse(sendSms, out smsEnabled);
                if (smsEnabled)
                {
                    var user = ConfigUtils.GetSafeAppSettingValue("SmsPortalUser");
                    var password = ConfigUtils.GetSafeAppSettingValue("SmsPortalPassword");
                    var brand = ConfigUtils.GetSafeAppSettingValue("SmsPortalBrand");
                    var client = new WebClient();
                    var numbers = string.Join(",", vm.Numbers.ToArray());
                    var url = $"http://portal.itsolutionbd.net/api/localbulk.php?user={user}&pass={password}&to={numbers}&sender={brand}&message={vm.SmsText}&unicode=0/1";
                    var stream = client.OpenRead(url);
                    var reader = new StreamReader(stream);
                    var result = await reader.ReadToEndAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendDelarReceiptSMS(SMSVm vm)
        {
            try
            {
                var sendSms = ConfigUtils.GetSafeAppSettingValue("SendDelarReceiptSMS");
                var smsEnabled = false;
                bool.TryParse(sendSms, out smsEnabled);
                if (smsEnabled)
                {
                    var user = ConfigUtils.GetSafeAppSettingValue("SmsPortalUser");
                    var password = ConfigUtils.GetSafeAppSettingValue("SmsPortalPassword");
                    var brand = ConfigUtils.GetSafeAppSettingValue("SmsPortalBrand");
                    var msg = ConfigUtils.GetSafeAppSettingValue("SmsReceiptConfirmation");
                    //var msg = "Dear Customer/Dealer\nAssalamualaikum,\n\nYou have deposited Tk {0} to PPS through {1} on {2}.\nPlease confirm/check.\n\nIf you have any query, please let us know.\n\nThanks,\nPPS Plastic Pipe Ind.Ltd.\nCell: 01700703321-22, 25\nHotline: 01700703333";
                    var sms = string.Format(msg, string.Format("{0:n}", vm.Amount), vm.BankThrough, vm.DateOn.ToString("dd/MM/yyyy"));
                    var client = new WebClient();
                    var numbers = string.Join(",", vm.Numbers.ToArray());
                    var url = $"http://portal.itsolutionbd.net/api/localbulk.php?user={user}&pass={password}&to={numbers}&sender={brand}&message={sms}&unicode=0/1";
                    var stream = client.OpenRead(url);
                    var reader = new StreamReader(stream);
                    var result = reader.ReadToEndAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}