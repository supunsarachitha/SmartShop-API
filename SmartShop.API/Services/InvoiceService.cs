using Microsoft.EntityFrameworkCore;
using SmartShop.API.Common;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly SmartShopDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public InvoiceService(SmartShopDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ApplicationResponse<Invoice>> CreateInvoiceAsync(Invoice invoice)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (invoice == null || invoice.Items == null || !invoice.Items.Any())
                {
                    return ResponseFactory.CreateErrorResponse<Invoice>(
                        "Invalid invoice data.",
                        "Invoice",
                        "Invoice or items are missing.",
                        StatusCodes.Status400BadRequest);
                }

                invoice.Id = Guid.NewGuid();
                invoice.InvoiceDate = _dateTimeProvider.UtcNow;
                invoice.InvoiceNumber = $"INV-{_dateTimeProvider.UtcNow:yyyyMMddHHmmss}-{invoice.Id.ToString().Substring(0, 8)}";

                var productIds = invoice.Items.Select(i => i.ProductId).Distinct().ToList();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

                invoice.Total = invoice.Items.Sum(i =>
                {
                    var product = products.FirstOrDefault(p => p.Id == i.ProductId);
                    return product != null ? product.Price * i.Quantity : 0;
                });

                _context.Invoices.Add(invoice);

                foreach (var item in invoice.Items)
                {
                    item.Id = Guid.NewGuid();
                    item.InvoiceId = invoice.Id;
                }
                _context.InvoiceItems.AddRange(invoice.Items);

                if (invoice.Payments != null && invoice.Payments.Count > 0)
                {
                    foreach (var payment in invoice.Payments)
                    {
                        payment.Id = Guid.NewGuid();
                        payment.InvoiceId = invoice.Id;
                        payment.Date = _dateTimeProvider.UtcNow;
                    }
                    _context.Payments.AddRange(invoice.Payments);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResponseFactory.CreateSuccessResponse(
                    invoice,
                    "Invoice created successfully.",
                    StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResponseFactory.CreateErrorResponse<Invoice>(
                    "Failed to create invoice.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Invoice>> DeleteInvoiceAsync(Guid id)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Payments)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (invoice == null)
                {
                    return ResponseFactory.CreateErrorResponse<Invoice>(
                        "Invoice not found.",
                        "Id",
                        "No invoice found with the provided id.",
                        StatusCodes.Status404NotFound);
                }

                if (invoice.Items != null)
                {
                    _context.InvoiceItems.RemoveRange(invoice.Items);
                }
                if (invoice.Payments != null)
                {
                    _context.Payments.RemoveRange(invoice.Payments);
                }
                _context.Invoices.Remove(invoice);

                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    invoice,
                    "Invoice deleted successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Invoice>(
                    "Failed to delete invoice.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<List<Invoice>>> GetAllInvoicesAsync()
        {
            try
            {
                var invoices = await _context.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Payments)
                    .ToListAsync();

                return ResponseFactory.CreateSuccessResponse(
                    invoices,
                    "Invoices retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<List<Invoice>>(
                    "Failed to retrieve invoices.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Invoice>> GetInvoiceByIdAsync(Guid id)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Payments)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (invoice == null)
                {
                    return ResponseFactory.CreateErrorResponse<Invoice>(
                        "Invoice not found.",
                        "Id",
                        "No invoice found with the provided id.",
                        StatusCodes.Status404NotFound);
                }

                return ResponseFactory.CreateSuccessResponse(
                    invoice,
                    "Invoice retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Invoice>(
                    "Failed to retrieve invoice.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Invoice>> UpdateInvoiceAsync(Guid id, Invoice invoice)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingInvoice = await _context.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Payments)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (existingInvoice == null)
                {
                    return ResponseFactory.CreateErrorResponse<Invoice>(
                        "Invoice not found.",
                        "Id",
                        "No invoice found with the provided id.",
                        StatusCodes.Status404NotFound);
                }

                existingInvoice.CustomerId = invoice.CustomerId;
                existingInvoice.Status = invoice.Status;
                existingInvoice.InvoiceDate = invoice.InvoiceDate;

                var productIds = invoice.Items?.Select(i => i.ProductId).Distinct().ToList() ?? new List<Guid>();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

                existingInvoice.Total = invoice.Items?.Sum(i =>
                {
                    var product = products.FirstOrDefault(p => p.Id == i.ProductId);
                    return product != null ? product.Price * i.Quantity : 0;
                }) ?? 0;

                // Update InvoiceItems
                if (invoice.Items != null)
                {
                    _context.InvoiceItems.RemoveRange(existingInvoice.Items);
                    foreach (var item in invoice.Items)
                    {
                        item.Id = Guid.NewGuid();
                        item.InvoiceId = existingInvoice.Id;
                    }
                    _context.InvoiceItems.AddRange(invoice.Items);
                }

                // Update Payments
                if (invoice.Payments != null)
                {
                    _context.Payments.RemoveRange(existingInvoice.Payments);
                    foreach (var payment in invoice.Payments)
                    {
                        payment.Id = Guid.NewGuid();
                        payment.InvoiceId = existingInvoice.Id;
                        payment.Date = _dateTimeProvider.UtcNow;
                    }
                    _context.Payments.AddRange(invoice.Payments);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResponseFactory.CreateSuccessResponse(
                    existingInvoice,
                    "Invoice updated successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResponseFactory.CreateErrorResponse<Invoice>(
                    "Failed to update invoice.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
