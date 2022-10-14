using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//NoLog attribute will skip the logging 
//Example of using NoLog
//public class ExampleController : ApiController
//{
//    // GET api/example
//    [NoLog]
//    public Example Get()
//    {
//        //
//    }
//}
namespace PPS.API.HelperClasses
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class NoLogAttribute : Attribute
    {

    }
}