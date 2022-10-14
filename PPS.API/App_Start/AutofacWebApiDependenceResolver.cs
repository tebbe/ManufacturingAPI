﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPS.API.App_Start
{
    public class AutofacWebApiDependenceResolver : IDependencyResolver
    {
        private readonly IComponentContext container;
        public AutofacWebApiDependenceResolver(IContainer container)
        {

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            var ret = this.container.ResolveOptional(serviceType);
            return ret;
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            Type enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var ret = (IEnumerable<object>)this.container.ResolveOptional(enumerableType);
            return ret;
        }
    }
}