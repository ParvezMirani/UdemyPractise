using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Request;
using DLL.Models;
using DLL.Repositories;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();
        Task<Department> GetAAsync(string code);
        Task<Department> InsertAsync(DepartmentInsertRequestViewModel request);
        Task<Department> UpdateAsync(string code, Department department);
        Task<Department> DeleteAsync(string code);

        Task<bool> IsCodeExists(string code);
        Task<bool> IsNameExists(string name);
    }


    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Department> DeleteAsync(string code)
        {
            var department = await _departmentRepository.GetAAsync(code);

            if(department == null)
            {
                throw new ApplicationValidationException("Department not found");
            }

            if (await _departmentRepository.DeleteAsync(department))
            {
                return department;
            }

            throw new ApplicationValidationException("Problem occured while deleting Department");
        }

        public async Task<Department> GetAAsync(string code)
        {
            var department = await _departmentRepository.GetAAsync(code);

            if(department == null)
            {
                throw new ApplicationValidationException("Department not found");
            }

            return department;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department> InsertAsync(DepartmentInsertRequestViewModel request)
        {
            Department adepartment = new Department();
            adepartment.Code = request.Code;
            adepartment.Name = request.Name;
            return await _departmentRepository.InsertAsync(adepartment);
        }

        public async Task<bool> IsCodeExists(string code)
        {
            var department = await _departmentRepository.FindByCode(code);

            if (department == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsNameExists(string name)
        {
            var department = await _departmentRepository.FindByName(name);

            if (department == null)
            {
                return true;
            }

            return false;
        }

        public async Task<Department> UpdateAsync(string code, Department updateDepartment)
        {
            var department = await _departmentRepository.GetAAsync(code);

            if (department == null)
            {
                throw new ApplicationValidationException("Department not found");
            }

            if (!string.IsNullOrWhiteSpace(updateDepartment.Code))
            {
                var existAlreadyCode = await _departmentRepository.FindByCode(updateDepartment.Code);
                if(existAlreadyCode != null)
                {
                    throw new ApplicationValidationException($"Code : '{updateDepartment.Code}' already exists in our system");
                }

                department.Code = updateDepartment.Code;
            }

            if (!string.IsNullOrWhiteSpace(updateDepartment.Name))
            {
                var existAlreadyName = await _departmentRepository.FindByName(updateDepartment.Name);
                if (existAlreadyName != null)
                {
                    throw new ApplicationValidationException($"Name : '{updateDepartment.Name}' already exists in our system");
                }

                department.Name = updateDepartment.Name;
            }

            if(await _departmentRepository.UpdateAsync(department))
            {
                return department;
            }

            throw new ApplicationValidationException("Problem Occured updating department");
        }
    }
}
