using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.Sales;

namespace PPS.Data.Dtos.Sales
{
    public class DemandOrderDto
    {
        public DemandOrderDto()
        {
            DemandOrderDetail = new List<DemandOrderDetailDto>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DemandOrderNo { get; set; }
        public int DemandOrderTypeId { get; set; }
        public int DiscountTypeId { get; set; }
        public DateTime DODate { get; set; }
        public int? VarifiedBy { get; set; }
        public DateTime VarifiedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string ReferenceDONo { get; set; }
        public int? SubmittedBy { get; set; }
        public DateTime SubmittedOn { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public int? ReturnedBy { get; set; }
        public DateTime ReturnedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int IsCurrentRecord { get; set; }
        public int PreviousId { get; set; }
        public bool Locked { get; set; }
        public int RejectedReasonTypeId { get; set; }
        public int SaleTypeId { get; set; }

        public ICollection<DemandOrderDetailDto> DemandOrderDetail { get; set; }

        public DemandOrderModel ToDemandOrderModel()
        {
            var vm = new DemandOrderModel
            {
                Id = this.Id,
                CustomerId = this.CustomerId,
                DemandOrderNo = this.DemandOrderNo,
                DemandOrderTypeId = this.DemandOrderTypeId,
                DiscountTypeId = this.DiscountTypeId,
                DODate = this.DODate,
                VarifiedBy = this.VarifiedBy,
                VarifiedOn = this.VarifiedOn,
                ApprovedBy = this.ApprovedBy,
                ApprovedOn = this.ApprovedOn,
                ReferenceDONo = this.ReferenceDONo,
                SubmittedBy = this.SubmittedBy,
                SubmittedOn = this.SubmittedOn,
                RejectedBy = this.RejectedBy,
                RejectedOn = this.RejectedOn,
                ReturnedBy = this.ReturnedBy,
                ReturnedOn = this.ReturnedOn,
                CreatedBy = this.CreatedBy,
                CreatedOn = this.CreatedOn,
                IsCurrentRecord = this.IsCurrentRecord,
                PreviousId = this.PreviousId,
                Locked = this.Locked,
                RejectedReasonTypeId = this.RejectedReasonTypeId,
                SaleTypeId = this.SaleTypeId
            };

            vm.DemandOrderDetail = new List<DemandOrderDetailVm>();

            foreach(var DO in DemandOrderDetail)
            {
                var vmDoDetail = new DemandOrderDetailVm
                {
                    //Id = DO.Id,
                    //DemandOrderId = DO.DemandOrderId,
                    //ProductId = DO.ProductTypeId,
                    //Quantity = DO.Quantity,
                    //Discount = DO.Discount
                };
                vm.DemandOrderDetail.Add(vmDoDetail);
            }

            return vm;
        }
    }
}