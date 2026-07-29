using Inventory.API.DTOs.Supplier;
using Inventory.API.Entities;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;

namespace Inventory.API.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync(QueryParameters query)
        {
            var suppliers = await _repository.GetAllAsync(query);

            return suppliers.Select(s => new SupplierDto
            {
                Id = s.Id,
                SupplierCode = s.SupplierCode,
                CompanyName = s.CompanyName,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                City = s.City,
                State = s.State,
                Country = s.Country,
                PostalCode = s.PostalCode,
                IsActive = s.IsActive
            });
        }

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);

            if (supplier == null)
                return null;

            return new SupplierDto
            {
                Id = supplier.Id,
                SupplierCode = supplier.SupplierCode,
                CompanyName = supplier.CompanyName,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                City = supplier.City,
                State = supplier.State,
                Country = supplier.Country,
                PostalCode = supplier.PostalCode,
                IsActive = supplier.IsActive
            };
        }

        public async Task<bool> CreateAsync(CreateSupplierDto dto)
        {
            // Check duplicate Supplier Code
            var existingSupplier = await _repository.GetBySupplierCodeAsync(dto.SupplierCode);

            if (existingSupplier != null)
                return false;

            var supplier = new Supplier
            {
                SupplierCode = dto.SupplierCode,
                CompanyName = dto.CompanyName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(supplier);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto)
        {
            var supplier = await _repository.GetByIdAsync(id);

            if (supplier == null)
                return false;

            supplier.CompanyName = dto.CompanyName;
            supplier.ContactPerson = dto.ContactPerson;
            supplier.Email = dto.Email;
            supplier.Phone = dto.Phone;
            supplier.Address = dto.Address;
            supplier.City = dto.City;
            supplier.State = dto.State;
            supplier.Country = dto.Country;
            supplier.PostalCode = dto.PostalCode;
            supplier.IsActive = dto.IsActive;

            _repository.Update(supplier);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);

            if (supplier == null)
                return false;

            _repository.Delete(supplier);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}