using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using PPS.Data.Edmx;
using PPS.Data.RepositoryInterfaces;

namespace PPS.Data.Repositories
{
    public class SystemWarningRepository : ISystemWarningRepository
    {
        private PPSDbContext _ppsDbContext;

        public SystemWarningRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }
        public List<SystemWarningType> GetSystemWarningType()
        {
            return _ppsDbContext.SystemWarningType.Where(x => x.Active == true).ToList();
        }
        public IQueryable<SystemWarning> GetSystemWarning()
        {
            return _ppsDbContext.SystemWarning;
        }
        public SystemWarning SaveSystemWarning(SystemWarning systemWarning)
        {
            var warning = _ppsDbContext.SystemWarning.Add(systemWarning);
            _ppsDbContext.SaveChanges();
            return warning;
        }
        public SystemWarningHistory SaveSystemWarningHistory(SystemWarningHistory systemWarningHistory)
        {
            var warning = _ppsDbContext.SystemWarningHistory.Add(systemWarningHistory);
            _ppsDbContext.SaveChanges();
            return warning;
        }
        public SystemWarning UpdateSystemWarning(SystemWarning systemWarning)
        {
            _ppsDbContext.SystemWarning.Attach(systemWarning);
            _ppsDbContext.Entry(systemWarning).State = EntityState.Modified;

            _ppsDbContext.SaveChanges();
            return systemWarning;
        }

        public List<SystemWarning> UpdateBulkSystemWarning(List<SystemWarning> systemWarning)
        {
            systemWarning.ForEach(w =>
            {
                _ppsDbContext.SystemWarning.Attach(w);
                _ppsDbContext.Entry(w).State = EntityState.Modified;
            });
            _ppsDbContext.SaveChanges();
            return systemWarning;
        }
    }
}