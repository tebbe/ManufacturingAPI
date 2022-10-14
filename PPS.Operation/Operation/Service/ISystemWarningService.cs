namespace PPS.Operation.Operation.Service
{
    public interface ISystemWarningService
    {
        bool CheckSystemWarning(int fiscalYear, int companyId, int userId);
    }
}
