using AutoMapper;
using Pupil.DataLayer;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Services
{
    public class FeeStructureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeeStructureService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleResponse<string>> CreateAsync(FeeStructureRequestDc requestObj)
        {
            var response = new SingleResponse<string>();
            try
            {
                await _unitOfWork.Run(async () =>
                {
                    #region FeeStructure
                    FeeStructure tblFeeStructure = new FeeStructure();
                    tblFeeStructure.Sname = requestObj.FeeStructure.Sname;
                    tblFeeStructure.ReceiptPrefix = requestObj.FeeStructure.ReceiptPrefix;
                    tblFeeStructure.ReceiptStartingNumber = requestObj.FeeStructure.ReceiptStartingNumber;
                    tblFeeStructure.TotalAmount = requestObj.FeeStructure.TotalAmount;
                    tblFeeStructure.TotalAmountWithTax = requestObj.FeeStructure.TotalAmountWithTax;
                    tblFeeStructure.Status = true;
                    _unitOfWork.FeeStructureRepository.Insert(tblFeeStructure);
                    await _unitOfWork.SaveChangesAsync();
                    #endregion
                    if (tblFeeStructure.FeeStructureId > 0)
                    {
                        #region Fee Structure Class
                        List<FeeStructureClass> feeStructureClasses = new List<FeeStructureClass>();
                        foreach (var selectedClass in requestObj.SelectedClass)
                        {
                            feeStructureClasses.Add(new FeeStructureClass()
                            {
                                FeeStructureClassId = Guid.NewGuid(),
                                FeeStructureId = tblFeeStructure.FeeStructureId,
                                ClassId = selectedClass,
                            });
                        }
                        _unitOfWork.FeeStructureClassRepository.InsertRange(feeStructureClasses);
                        await _unitOfWork.SaveChangesAsync();
                        #endregion

                        #region Fee Structure Installments
                        List<FeeStructureInstallment>  feeStructureInstallments = new List<FeeStructureInstallment>();
                        foreach (var installment in requestObj.SelectedIntallments)
                        {
                            feeStructureInstallments.Add(new FeeStructureInstallment()
                            {
                                InstallmentId = installment.InstallmentId,
                                FeeStructureId = tblFeeStructure.FeeStructureId,
                                FeeTypeId = installment.FeeTypeId,
                                Tax = installment.Tax,
                                Amount = installment.Amount,
                                DateOfInstallment = installment.DateOfInstallment,
                            });
                        }
                        _unitOfWork.FeeStructureInstallmentsRepository.InsertRange(feeStructureInstallments);
                        await _unitOfWork.SaveChangesAsync();
                        #endregion
                    }

                });


                response.Message = "Saved Successfully!";
                response.Status = StatusCode.Ok;
                response.Data = "Success";

            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = "Error";
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }
        public async Task<SingleResponse<string>> UpdateAsync(FeeStructureRequestDc requestObj)
        {
            var response = new SingleResponse<string>();
            try
            {
                await _unitOfWork.Run(async () =>
                {
                    #region FeeStructure
                    FeeStructure tblFeeStructure = await _unitOfWork.FeeStructureRepository.GetByIdAsync(requestObj.FeeStructure.FeeStructureId);

                    tblFeeStructure.Sname = requestObj.FeeStructure.Sname;
                    tblFeeStructure.ReceiptPrefix = requestObj.FeeStructure.ReceiptPrefix;
                    tblFeeStructure.ReceiptStartingNumber = requestObj.FeeStructure.ReceiptStartingNumber;
                    tblFeeStructure.TotalAmount = requestObj.FeeStructure.TotalAmount;
                    tblFeeStructure.TotalAmountWithTax = requestObj.FeeStructure.TotalAmountWithTax;
                    tblFeeStructure.Status = true;
                    _unitOfWork.FeeStructureRepository.Update(tblFeeStructure);
                    await _unitOfWork.SaveChangesAsync();
                    #endregion
                    #region Fee Structure Class
                    List<FeeStructureClass> feeStructureClasses = (await _unitOfWork.FeeStructureClassRepository.GetSearchAsync(x => x.FeeStructureId == requestObj.FeeStructure.FeeStructureId)).ToList();
                    _unitOfWork.FeeStructureClassRepository.DeleteRange(feeStructureClasses);
                    await _unitOfWork.SaveChangesAsync();
                    feeStructureClasses = new List<FeeStructureClass>();
                    foreach (var selectedClass in requestObj.SelectedClass)
                    {
                        feeStructureClasses.Add(new FeeStructureClass()
                        {
                            FeeStructureClassId = Guid.NewGuid(),
                            FeeStructureId = tblFeeStructure.FeeStructureId,
                            ClassId = selectedClass,
                        });
                    }
                    _unitOfWork.FeeStructureClassRepository.InsertRange(feeStructureClasses);
                    await _unitOfWork.SaveChangesAsync();
                    #endregion

                    #region Fee Structure Installments
                    List<FeeStructureInstallment> feeStructureInstallments = (await _unitOfWork.FeeStructureInstallmentsRepository.GetSearchAsync(x => x.FeeStructureId == requestObj.FeeStructure.FeeStructureId)).ToList();
                    _unitOfWork.FeeStructureInstallmentsRepository.DeleteRange(feeStructureInstallments);
                    await _unitOfWork.SaveChangesAsync();
                    feeStructureInstallments = new List<FeeStructureInstallment>();
                    foreach (var installment in requestObj.SelectedIntallments)
                    {
                        feeStructureInstallments.Add(new FeeStructureInstallment()
                        {
                            InstallmentId = installment.InstallmentId,
                            FeeStructureId = tblFeeStructure.FeeStructureId,
                            FeeTypeId = installment.FeeTypeId,
                            Tax = installment.Tax,
                            Amount = installment.Amount,
                            DateOfInstallment = installment.DateOfInstallment,
                        });
                    }
                    _unitOfWork.FeeStructureInstallmentsRepository.InsertRange(feeStructureInstallments);
                    await _unitOfWork.SaveChangesAsync();
                    #endregion
                });
                response.Message = "Saved Successfully!";
                response.Status = StatusCode.Ok;
                response.Data = "Success";

            }
            catch (Exception ex)
            {
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = "Error";
                response.Message = AppConstants.ExceptionMessage;
            }
            return response;
        }
        public async Task<ListResponse<FeeStructureDc>> GetAllAsync()
        {
            var response = new ListResponse<FeeStructureDc>();
            try
            {

                response.Data = await _unitOfWork.FeeStructureRepository.GetWithProjectionAsync(x => new FeeStructureDc
                {
                    FeeStructureId = x.FeeStructureId,
                    StructureName = x.Sname,
                    ReceiptPrefix = x.ReceiptPrefix,
                    ReceiptStartingNumber = x.ReceiptStartingNumber
                }, x => x.Status == true);
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new List<FeeStructureDc>();
            }

            return response;
        }

        public async Task<SingleResponse<FeeStructureRequestDc>> GetById(int id)
        {
            var response = new SingleResponse<FeeStructureRequestDc>();
            try
            {
                FeeStructureRequestDc feeStructureRequestDc = new FeeStructureRequestDc();
                FeeStructurePropsDc feeStructurePropsDc = (await _unitOfWork.FeeStructureRepository.GetWithProjectionAsync(x => new FeeStructurePropsDc
                {
                    FeeStructureId = x.FeeStructureId,
                    Sname = x.Sname,
                    ReceiptPrefix = x.ReceiptPrefix,
                    ReceiptStartingNumber = x.ReceiptStartingNumber,
                    TotalAmount = x.TotalAmount,
                    TotalAmountWithTax = x.TotalAmountWithTax,
                }, x => x.FeeStructureId == id)).First();

                List<int> selectedClasses = (await _unitOfWork.FeeStructureClassRepository.GetSearchAsync(x => x.FeeStructureId == id)).Select(x => x.ClassId).ToList();

                //List<SelectedIntallmentPropsDc> selectedIntallmentPropsDcs = (await _unitOfWork.FeeStructureInstallmentsRepository.GetWithProjectionAsync(x => new SelectedIntallmentPropsDc
                //{
                //    InstallmentId = x.InstallmentId,
                //    FeeTypeId = x.FeeTypeId,
                //    Tax=x.Tax,
                //    Amount = x.Amount,
                //    DateOfInstallment=x.DateOfInstallment
                //}, x => x.FeeStructureId == id)).ToList();

                List<SelectedIntallmentPropsDc> selectedIntallmentPropsDcs = (from lm in await _unitOfWork.FeeStructureInstallmentsRepository.GetSearchAsync(x => x.FeeStructureId == id)
                                                                              join ft in await _unitOfWork.FeeTypeRepository.GetAllAsync() on lm.FeeTypeId equals ft.Id
                                                                              select new SelectedIntallmentPropsDc
                                                                              {
                                                                                  InstallmentId = lm.InstallmentId,
                                                                                  FeeTypeName = ft.FeeType,
                                                                                  FeeTypeId = lm.FeeTypeId,
                                                                                  Tax = lm.Tax,
                                                                                  Amount = lm.Amount,
                                                                                  DateOfInstallment = lm.DateOfInstallment
                                                                              }).ToList();
                List<SelectedFeeTypePropsDc> selectedFeeTypePropsDcs = selectedIntallmentPropsDcs.Select(x => new SelectedFeeTypePropsDc()
                {
                    FeeTypeId = x.FeeTypeId,
                    FeeTypeName = x.FeeTypeName,
                    Tax = x.Tax
                }).DistinctBy(x => x.FeeTypeId).ToList();

                //List<SelectedFeeTypePropsDc> selectedFeeTypePropsDcs = (from lm in await _unitOfWork.FeeStructureInstallmentsRepository.GetSearchAsync(x=>x.FeeStructureId==id)
                //                                                        join ft in await _unitOfWork.FeeTypeRepository.GetAllAsync() on lm.FeeTypeId equals ft.Id
                //                                                        select new SelectedFeeTypePropsDc { FeeTypeId=lm.FeeTypeId,FeeTypeName=ft.FeeType,Tax=lm.Tax}).DistinctBy(x=>x.FeeTypeId).ToList();

                feeStructureRequestDc.FeeStructure = feeStructurePropsDc;
                feeStructureRequestDc.SelectedFeeTypes = selectedFeeTypePropsDcs;
                feeStructureRequestDc.SelectedClass = selectedClasses;
                feeStructureRequestDc.SelectedIntallments = selectedIntallmentPropsDcs;
                response.Data = feeStructureRequestDc;
                response.Status = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                response.Message = AppConstants.ExceptionMessage;
                response.Status = StatusCode.SystemException;
                response.Error = ex.Message;
                response.Data = new FeeStructureRequestDc();
            }

            return response;
        }
    }
}
