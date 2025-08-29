
# 📡 SmartShop API — Full Request Guide

This document provides comprehensive examples for all SmartShop API endpoints. All requests use JSON and require a Bearer token for authentication.

---

## 🔐 Authentication

Include this header in every request:

```http
Authorization: Bearer <your_token>
Content-Type: application/json
```

If the token is missing or invalid, the API will return:

```json
{
  "status": "error",
  "message": "Unauthorized"
}
```

---

## 🛍️ Products

### ➕ Create Product

**POST** `/products`

```json
{
  "name": "Wireless Mouse",
  "price": 19.99,
  "stock": 50
}
```

**Response**
```json
{
  "id": 101,
  "name": "Wireless Mouse",
  "price": 19.99,
  "stock": 50
}
```

---

### 📋 List Products

**GET** `/products`

**Response**
```json
[
  {
    "id": 101,
    "name": "Wireless Mouse",
    "price": 19.99,
    "stock": 50
  }
]
```

---

### 📝 Update Product

**PUT** `/products/{id}`

```json
{
  "name": "Wireless Mouse Pro",
  "price": 24.99,
  "stock": 40
}
```

**Response**
```json
{
  "id": 101,
  "name": "Wireless Mouse Pro",
  "price": 24.99,
  "stock": 40
}
```

---

### ❌ Delete Product

**DELETE** `/products/{id}`

**Response**
```json
{
  "status": "success",
  "message": "Product deleted"
}
```

---

## 👥 Customers

### ➕ Add Customer

**POST** `/customers`

```json
{
  "name": "Jane Doe",
  "email": "jane@example.com",
  "phone": "123-456-7890"
}
```

**Response**
```json
{
  "id": 201,
  "name": "Jane Doe",
  "email": "jane@example.com",
  "phone": "123-456-7890"
}
```

---

### 📋 List Customers

**GET** `/customers`

**Response**
```json
[
  {
    "id": 201,
    "name": "Jane Doe",
    "email": "jane@example.com",
    "phone": "123-456-7890"
  }
]
```

---

### 📝 Update Customer

**PUT** `/customers/{id}`

```json
{
  "name": "Jane D.",
  "email": "jane.d@example.com",
  "phone": "987-654-3210"
}
```

**Response**
```json
{
  "id": 201,
  "name": "Jane D.",
  "email": "jane.d@example.com",
  "phone": "987-654-3210"
}
```

---

### ❌ Delete Customer

**DELETE** `/customers/{id}`

**Response**
```json
{
  "status": "success",
  "message": "Customer deleted"
}
```

---

## 🧾 Invoices

### ➕ Create Invoice

**POST** `/invoices`

```json
{
  "customer_id": 201,
  "items": [
    { "product_id": 101, "quantity": 2 },
    { "product_id": 102, "quantity": 1 }
  ]
}
```

**Response**
```json
{
  "invoice_id": 301,
  "total": 69.97,
  "status": "pending"
}
```

---

### 📋 List Invoices

**GET** `/invoices`

**Response**
```json
[
  {
    "invoice_id": 301,
    "customer_id": 201,
    "total": 69.97,
    "status": "pending"
  }
]
```

---

### 📝 Update Invoice

**PUT** `/invoices/{id}`

```json
{
  "status": "paid"
}
```

**Response**
```json
{
  "invoice_id": 301,
  "status": "paid"
}
```

---

### ❌ Delete Invoice

**DELETE** `/invoices/{id}`

**Response**
```json
{
  "status": "success",
  "message": "Invoice deleted"
}
```

---

## 💳 Payments

### 💰 Record Payment

**POST** `/payments`

```json
{
  "invoice_id": 301,
  "amount": 69.97,
  "method": "credit_card"
}
```

**Response**
```json
{
  "payment_id": 401,
  "status": "completed",
  "date": "2025-08-29T12:00:00Z"
}
```

---

### 📋 List Payments

**GET** `/payments`

**Response**
```json
[
  {
    "payment_id": 401,
    "invoice_id": 301,
    "amount": 69.97,
    "method": "credit_card",
    "status": "completed",
    "date": "2025-08-29T12:00:00Z"
  }
]
```

---

## ⚠️ Error Responses

| Code | Message                  | Description                      |
|------|--------------------------|----------------------------------|
| 400  | Invalid input data       | Missing or malformed fields      |
| 401  | Unauthorized             | Missing or invalid token         |
| 404  | Resource not found       | ID does not exist                |
| 500  | Internal server error    | Unexpected failure               |

---

## 🧪 Sample cURL Request

```bash
curl -X POST https://api.smartshop.com/v1/products \
  -H "Authorization: Bearer <your_token>" \
  -H "Content-Type: application/json" \
  -d '{"name":"Wireless Mouse","price":19.99,"stock":50}'
```

---

## 📌 Notes

- All timestamps use ISO 8601 format.
- Prices are in USD.
- Pagination and filtering will be added in future versions.
