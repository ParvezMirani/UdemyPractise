using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IDepartmentService
    {
        IQueryable<Department> GetAllAsync();
        Task<Department> GetAAsync(string code);
        Task<Department> InsertAsync(DepartmentInsertRequestViewModel request);
        Task<Department> UpdateAsync(string code, Department department);
        Task<Department> DeleteAsync(string code);

        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
        Task<bool> IsIdExists(int id);
    }


    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IQueryable<Department> GetAllAsync()
        {
            return _unitOfWork.DepartmentRepository.QueryAll();
        }


        public async Task<Department> GetAAsync(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Code == code);

            if (department == null)
            {
                throw new ApplicationValidationException("Department not found");
            }

            return department;
        }


        public async Task<Department> InsertAsync(DepartmentInsertRequestViewModel request)
        {
            Department adepartment = new Department();
            adepartment.Code = request.Code;
            adepartment.Name = request.Name;
            await _unitOfWork.DepartmentRepository.CreateAsync(adepartment);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return adepartment;
            }

            throw new ApplicationValidationException("Adding new department failed");
        }


        public async Task<Department> UpdateAsync(string code, Department updateDepartment)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Code == code);

            if (department == null)
            {
                throw new ApplicationValidationException("Department not found");
            }

            if (!string.IsNullOrWhiteSpace(updateDepartment.Code))
            {
                var existAlreadyCode = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Code == updateDepartment.Code);
                if (existAlreadyCode != null)
                {
                    throw new ApplicationValidationException($"Code : '{updateDepartment.Code}' already exists in our system");
                }

                department.Code = updateDepartment.Code;
            }

            if (!string.IsNullOrWhiteSpace(updateDepartment.Name))
            {
                var existAlreadyName = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Name == updateDepartment.Name);
                if (existAlreadyName != null)
                {
                    throw new ApplicationValidationException($"Name : '{updateDepartment.Name}' already exists in our system");
                }

                department.Name = updateDepartment.Name;
            }

            _unitOfWork.DepartmentRepository.Update(department);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return department;
            }

            throw new ApplicationValidationException("Problem Occured updating department");
        }


        public async Task<Department> DeleteAsync(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Code == code);

            if (department == null)
            {
                throw new ApplicationValidationException("Department not found");
            }

            _unitOfWork.DepartmentRepository.Delete(department);

            if (await _unitOfWork.SaveCompletedAsync())
            {
                return department;
            }

            throw new ApplicationValidationException("Problem occured while deleting Department");
        }



        public async Task<bool> IsCodeExists(string code)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Code == code);

            if (department == null)
            {
                return true;
            }

            return false;
        }


        public async Task<bool> IsNameExists(string name)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.Name == name);

            if (department == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsIdExists(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.FindSingleAsync(x => x.DepartmentId == id);

            if (department == null)
            {
                return true;
            }

            return false;
        }
    }
}
