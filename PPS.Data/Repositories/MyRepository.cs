using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPS.Data.Repositories
{
    public class MyRepository : IMyRepository
    {
        private PPSDbContext _ppsDbContext;
        public MyRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }


    }
}