# Warehouse Management API

ASP.NET Core Web API developed for **Session 02 – Building REST APIs & API Documentation**.

The project simulates warehouse inventory operations using in-memory collections.

## Implemented Homework Tasks

### Task 1 — Supplier Module

The following Supplier features were implemented:

- Supplier model
- Supplier DTOs
- Supplier service
- Supplier controller
- Get all suppliers
- Get supplier by ID
- Create supplier
- Deactivate supplier

### Task 2 — Product-Supplier Link

The product-supplier assignment feature was implemented.

The API validates that:

- The product exists
- The supplier exists
- Archived products cannot be assigned to a supplier

Endpoint:

POST /api/products/{id}/assign-supplier/{supplierId}

## Product Endpoints:

| Method | Endpoint                                          | Description                 |
| ------ | ------------------------------------------------- | --------------------------- |
| GET    | `/api/products`                                   | Get all products            |
| GET    | `/api/products?onlyAvailable=true`                | Get available products only |
| GET    | `/api/products/{id}`                              | Get product by ID           |
| GET    | `/api/products/search?name=...&supplier=...`      | Search products             |
| POST   | `/api/products`                                   | Create product              |
| POST   | `/api/products/{id}/quantity`                     | Update product quantity     |
| POST   | `/api/products/{id}/price`                        | Update product price        |
| POST   | `/api/products/{id}/image`                        | Upload product image        |
| DELETE | `/api/products/{id}`                              | Archive product             |
| GET    | `/api/products/server-time`                       | Get formatted server time   |
| POST   | `/api/products/{id}/assign-supplier/{supplierId}` | Assign supplier to product  |

## Supplier Endpoints:

| Method | Endpoint              | Description         |
| ------ | --------------------- | ------------------- |
| GET    | `/api/suppliers`      | Get all suppliers   |
| GET    | `/api/suppliers/{id}` | Get supplier by ID  |
| POST   | `/api/suppliers`      | Create supplier     |
| DELETE | `/api/suppliers/{id}` | Deactivate supplier |

## Running the Project:

dotnet restore
dotnet build
dotnet run --project WarehouseManagement.Api

## Swagger Screenshots

### Product Endpoints Overview

![Product Endpoints Overview 1](docs/images/swagger/swagger-products-endpoints-overview-01.png)

![Product Endpoints Overview 2](docs/images/swagger/swagger-products-endpoints-overview-02.png)

---

### Get All Products

#### Request

![Get All Products Request](docs/images/swagger/swagger-get-all-products-request.png)

#### Response

![Get All Products Response](docs/images/swagger/swagger-get-all-products-response.png)

---

### Get Product by ID

#### Request

![Get Product by ID Request](docs/images/swagger/swagger-get-product-by-id-request.png)

#### Successful Response

![Get Product by ID Success](docs/images/swagger/swagger-get-product-by-id-success.png)

#### Product Not Found

![Get Product by ID Not Found](docs/images/swagger/swagger-get-product-by-id-not-found.png)

---

### Search Products by Name

#### Request

![Search Products by Name Request](docs/images/swagger/swagger-search-products-by-name-request.png)

#### Response

![Search Products by Name Response](docs/images/swagger/swagger-search-products-by-name-response.png)

---

### Search Products with Multiple Filters

#### Request

![Search Products Multiple Filters Request](docs/images/swagger/swagger-search-products-multiple-filters-request.png)

#### Response

![Search Products Multiple Filters Response](docs/images/swagger/swagger-search-products-multiple-filters-response.png)

---

### Search Products — Empty Parameters

#### Request

![Search Products Empty Request](docs/images/swagger/swagger-search-products-empty-request.png)

#### Bad Request Response

![Search Products Bad Request](docs/images/swagger/swagger-search-products-bad-request.png)

---

### Create Product

#### Request

![Create Product Request](docs/images/swagger/swagger-create-product-request.png)

#### Successful Response

![Create Product Success](docs/images/swagger/swagger-create-product-success.png)

---

### Update Product Quantity

#### Request

![Update Product Quantity Request](docs/images/swagger/swagger-update-product-quantity-request.png)

#### Successful Response

![Update Product Quantity Success](docs/images/swagger/swagger-update-product-quantity-success.png)

---

### Negative Product Quantity Validation

#### Request

![Negative Product Quantity Request](docs/images/swagger/swagger-update-product-negative-quantity-request.png)

#### Validation Error

![Negative Product Quantity Error](docs/images/swagger/swagger-update-product-negative-quantity-error.png)

---

### Update Product Price

#### Request

![Update Product Price Request](docs/images/swagger/swagger-update-product-price-request.png)

#### Successful Response

![Update Product Price Success](docs/images/swagger/swagger-update-product-price-success.png)

#### Price Update Log

![Product Price Update Log](docs/images/swagger/product-price-update-log.png)

---

### Invalid Product Price Validation

#### Request

![Invalid Product Price Request](docs/images/swagger/swagger-invalid-product-price-request.png)

#### Validation Error

![Invalid Product Price Error](docs/images/swagger/swagger-invalid-product-price-error.png)

---

### Product Image Upload

#### Upload Form

![Product Image Upload Form](docs/images/swagger/swagger-product-image-upload-form.png)

#### Successful Upload

![Product Image Upload Success](docs/images/swagger/swagger-product-image-upload-success.png)

#### Image Saved in Uploads Directory

![Product Image Saved](docs/images/swagger/product-image-saved-in-uploads.png)

#### Invalid File Validation

![Product Image Invalid File Error](docs/images/swagger/swagger-product-image-invalid-file-error.png)

---

### Archive Product

#### Successful Archive

![Archive Product Success](docs/images/swagger/swagger-archive-product-success.png)

#### Get Archived Product Request

![Get Archived Product Request](docs/images/swagger/swagger-get-archived-product-request.png)

#### Archived Product Response

![Get Archived Product Response](docs/images/swagger/swagger-get-archived-product-response.png)

---

### Server Time with Accept-Language Header

#### Request

![Server Time Language Header Request](docs/images/swagger/swagger-server-time-language-header-request.png)

#### Response

![Server Time Language Header Response](docs/images/swagger/swagger-server-time-language-header-response.png)

---

## Supplier Swagger Screenshots

### Get All Suppliers

#### Request

![Get All Suppliers Request](docs/images/swagger/swagger-get-all-suppliers-request.png)

#### Response

![Get All Suppliers Response](docs/images/swagger/swagger-get-all-suppliers-response.png)

---

### Get Supplier by ID

![Get Supplier by ID Success](docs/images/swagger/swagger-get-supplier-by-id-success.png)

---

### Create Supplier

#### Request

![Create Supplier Request](docs/images/swagger/swagger-create-supplier-request.png)

#### Successful Response

![Create Supplier Success](docs/images/swagger/swagger-create-supplier-success.png)

---

### Verify Supplier Creation

![Get Suppliers After Create](docs/images/swagger/swagger-get-suppliers-after-create.png)

---

### Deactivate Supplier

![Deactivate Supplier Success](docs/images/swagger/swagger-deactivate-supplier-success.png)

---

## Product-Supplier Assignment

### Assign Supplier to Product

#### Request

![Assign Supplier to Product Request](docs/images/swagger/swagger-assign-supplier-to-product-request.png)

#### Successful Response

![Assign Supplier to Product Success](docs/images/swagger/swagger-assign-supplier-to-product-success.png)
