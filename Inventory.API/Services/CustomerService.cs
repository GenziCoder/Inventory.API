using Inventory.API.Common;
using Inventory.API.DTOs.Customer;
using Inventory.API.Entities;
using Inventory.API.Exceptions;
using Inventory.API.Interfaces;

namespace Inventory.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResponse<CustomerDto>> GetAllAsync(
            string? search,
            int pageNumber,
            int pageSize)
        {
            var customers = await _repository.GetAllAsync(search, pageNumber, pageSize);
            var totalCount = await _repository.GetTotalCountAsync(search);

            var data = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                CustomerCode = c.CustomerCode,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                City = c.City,
                State = c.State,
                Country = c.Country,
                PostalCode = c.PostalCode,
                IsActive = c.IsActive
            }).ToList();

            return new PagedResponse<CustomerDto>(
                data,
                totalCount,
                pageNumber,
                pageSize);
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                throw new NotFoundException("Customer not found.");

            return new CustomerDto
            {
                Id = customer.Id,
                CustomerCode = customer.CustomerCode,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                Country = customer.Country,
                PostalCode = customer.PostalCode,
                IsActive = customer.IsActive
            };
        }

        public async Task CreateAsync(CreateCustomerDto dto)
        {
            var exists = await _repository.GetByCodeAsync(dto.CustomerCode);

            if (exists != null)
                throw new BusinessException("Customer code already exists.");

            var customer = new Customer
            {
                CustomerCode = dto.CustomerCode,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                IsActive = true
            };

            await _repository.AddAsync(customer);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                throw new NotFoundException("Customer not found.");

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.Address = dto.Address;
            customer.City = dto.City;
            customer.State = dto.State;
            customer.Country = dto.Country;
            customer.PostalCode = dto.PostalCode;
            customer.IsActive = dto.IsActive;

            _repository.Update(customer);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);

            if (customer == null)
                throw new NotFoundException("Customer not found.");

            customer.IsActive = false;

            _repository.Update(customer);
            await _repository.SaveChangesAsync();
        }
    }
}