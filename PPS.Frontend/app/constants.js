angular.module('AtlasPPS').constant("PpsConstant", {
    "DefaultError": "Your request has failed.",
    "IncompleteModelInput": "Required value(s) is/are missing.",
    "PasswordMismatch": "New password and confirm password should match.",
    "ShortPassword": "Password should be at least 8 characters long.",
    "DefaultSuccess": "Your request has been completed successfully.",
    "DuplicateAccountHead": "You have already been added this account head.",
    "DuplicateProductName": "You have already been added this Product.",
    "DuplicateRawMaterialTypeName": "You have already been added this raw material type.",
    DifferentProductTypeErrorMessage: "You can't add dirrent type of product",
    TransactionType: {
        Payment: 1,
        Received: 2,
        Journal: 3,
        Contra: 4
    },
    TranAmountType: {
        Debit: 1,
        Credit: 2
    },
    TranMode: {
        AddNew: 1,
        Update: 2,
        Delete: 3
    },
    DOStatus: {
        Initiated: 1,
        Submitted: 2,
        Verified: 3,
        Approved: 4,
        Delivered: 5
    },
    DOPaymentStatus: {
        NotApproved: 0,
        Paid: 1,
        Regular: 2,
        Warning: 3,
        PayDay: 4,
        Danger: 5
    },
    // TODO: Need to sync totalBackAllowsDays
    MinDate: new Date(
        new Date().getFullYear() - 1,
        //new Date().getFullYear()
        new Date().getMonth(),
        //new Date().getDate() - 2
        new Date().getDate()
    ),
    MaxDate: new Date(
        new Date().getFullYear(),
        new Date().getMonth(),
        //new Date().getMonth() + 1,
        new Date().getDate()
    ),
    DemandOrderDiscountSetting: {
        RegularDiscount: true,
        SpecialDiscount: true,
        AdditionalDiscount: false,
        ExtraDiscount: false,
        CashBack: true,
        DiscountAfterDiscount: true
    },
    ReportHeaderSetting: {
        CompanyLogoIncludeInReport: false
    }
});


angular.module('AtlasPPS').constant('reportSettings', {
    reportBaseUri: '/index.html#/'
});