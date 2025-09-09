using Microsoft.EntityFrameworkCore;
using SmartShop.API.Common;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PaymentMethodsService : IPaymentMethods
{
    private readonly SmartShopDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PaymentMethodsService(SmartShopDbContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ApplicationResponse<List<PaymentMethod>>> GetAllPaymentMethodsAsync()
    {
        try
        {
            var methods = await _context.PaymentMethods.ToListAsync();

            return ResponseFactory.CreateSuccessResponse(
                methods,
                "Payment methods retrieved successfully.",
                StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseFactory.CreateErrorResponse<List<PaymentMethod>>(
                "Failed to retrieve payment methods.",
                "Exception",
                ex.Message,
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApplicationResponse<PaymentMethod>> GetPaymentMethodByIdAsync(Guid id)
    {
        try
        {
            var method = await _context.PaymentMethods.FindAsync(id);

            if (method == null)
            {
                return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                    "Payment method not found.",
                    "Id",
                    "No payment method found with the specified ID.",
                    StatusCodes.Status404NotFound);
            }

            return ResponseFactory.CreateSuccessResponse(
                method,
                "Payment method retrieved successfully.",
                StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                "Failed to retrieve payment method.",
                "Exception",
                ex.Message,
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApplicationResponse<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethod)
    {
        try
        {
            if (paymentMethod.Id == Guid.Empty)
            {
                paymentMethod.Id = Guid.NewGuid();
            }
            paymentMethod.CreatedDate = _dateTimeProvider.UtcNow;
            paymentMethod.UpdatedDate = _dateTimeProvider.UtcNow;

            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return ResponseFactory.CreateSuccessResponse(
                paymentMethod,
                "Payment method created successfully.",
                StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                "Payment method creation failed.",
                "Exception",
                ex.Message,
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApplicationResponse<PaymentMethod>> UpdatePaymentMethodAsync(Guid id, PaymentMethod paymentMethod)
    {
        if (id != paymentMethod.Id)
        {
            return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                "Payment method ID mismatch.",
                "Id",
                "The ID in the URL does not match the ID in the payload.",
                StatusCodes.Status400BadRequest);
        }

        var existingMethod = await _context.PaymentMethods.FindAsync(id);
        if (existingMethod == null)
        {
            return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                "Payment method not found.",
                "PaymentMethod",
                "No payment method found with the specified ID.",
                StatusCodes.Status404NotFound);
        }

        existingMethod.Name = paymentMethod.Name;
        existingMethod.Description = paymentMethod.Description;
        existingMethod.Type = paymentMethod.Type;
        existingMethod.UpdatedDate = _dateTimeProvider.UtcNow;

        try
        {
            await _context.SaveChangesAsync();

            return ResponseFactory.CreateSuccessResponse(
                existingMethod,
                "Payment method updated successfully.",
                StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                "Payment method update failed.",
                "Exception",
                ex.Message,
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApplicationResponse<PaymentMethod>> DeletePaymentMethodAsync(Guid id)
    {
        try
        {
            var method = await _context.PaymentMethods.FindAsync(id);

            if (method == null)
            {
                return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                    "Payment method not found.",
                    "Id",
                    "No payment method found with the specified ID.",
                    StatusCodes.Status404NotFound);
            }

            _context.PaymentMethods.Remove(method);
            await _context.SaveChangesAsync();

            return ResponseFactory.CreateSuccessResponse(
                method,
                "Payment method deleted successfully.",
                StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return ResponseFactory.CreateErrorResponse<PaymentMethod>(
                "Payment method deletion failed.",
                "Exception",
                ex.Message,
                StatusCodes.Status500InternalServerError);
        }
    }
}