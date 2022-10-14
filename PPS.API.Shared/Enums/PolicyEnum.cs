namespace PPS.API.Shared.Enums
{
    public enum PolicyEnum
    {
        AccessLogin = 1,
        AccessBasicProfile = 10,
        AccessBasicProfileUpdate = 11,
        AccessBasicExtendedProfile = 12,
        AccessBasicExtendedProfileUpdate = 13,


        AdminUserViewList = 100,
        AdminUserView = 101,
        AdminUserAdd = 102,
        AdminUserUpdate = 103,
        AdminUserUpdateStatusActive = 104,
        AdminUserUpdateStatusDeactivate = 105,
        AdminUserUpdateLock = 106,
        AdminUserUpdateUnLock=107,
        AdminUserPasswordReset = 108,

        AdminRoleViewList = 110,
        AdminRoleView = 111,
        AdminRoleAdd = 112,
        AdminRoleUpdate = 113
    }
}