using SmartShop.API.Models;
using SmartShop.API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartShop.API.Interfaces
{
    public interface IInvoiceService
    {
        Task<ApplicationResponse<List<Invoice>>> GetAllInvoicesAsync();
        Task<ApplicationResponse<Invoice>> GetInvoiceByIdAsync(Guid id);
        Task<ApplicationResponse<Invoice>> UpdateInvoiceAsync(Guid id, Invoice invoice);
        Task<ApplicationResponse<Invoice>> CreateInvoiceAsync(Invoice invoice);
        Task<ApplicationResponse<Invoice>> DeleteInvoiceAsync(Guid id);
    }
}

