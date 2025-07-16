
# 🛍️ eShop - ASP.NET Core MVC Project

A simple and modular eCommerce web application built using **ASP.NET Core MVC**. This project demonstrates key concepts like layered architecture, Entity Framework Core, Identity, and Razor views, Desigen patterns (Repostry pattern,Unit of work, strategy pattern), tailored for learning and real-world application.

----------

## 🚀 Features

-   🛒 Product browsing & details
    
-   🧾 Shopping cart
    
-   👤 User registration & login & Reset password & Forget password & Confirm Email (ASP.NET Core Identity)
    
-   📦 Admin panel for managing products and categories
    
-   🔍 Product search and filtering
    


----------

## 🧱 Tech Stack

-   ASP.NET Core MVC (.NET 8)

-   C# (LINQ, Dependency Injection)

-   Dapper for reading data (Micro ORM)
    
-   SQL Server with Entity Framework Core (Code-First)
    
-   ASP.NET Core Identity
    
-   Bootstrap 5
    
-   Razor Views

-   Design Patterns (MVC + Repository Pattern + Unit of work + Strategy Pattern)

-   N-tiers architecture 
    
-   In memory Caching

----------

## 📁 Project Structure
```
eShop/
├── src/
├  ├── eShop.MVC         => UI & Controller Layer (ASP.NET Core MVC) 
├  ├── eShop.Core        => Entities, Utilities 
├  ├── eShop.DAL         => Unit of Work & Repositories & Data Context & Migrations & Disk i/o 
├  ├── eShop.Business    => Services & ViewModel & Attributes & Exceptions & Mapping
├── test/
├   ├── unit/
├       ├── eShop.BLL.Tests       => Unit Tests 
```
----------

## 🛠️ Setup & Run

### Prerequisites

-   [.NET SDK 8](https://dotnet.microsoft.com/download)
    
-   SQL Server (LocalDB or any other)
    
-   Visual Studio / VS Code
    

### Steps

bash
```
# 1. Clone the repository git clone https://github.com/AbdullahHanfi/eShop.git 
# 2. Navigate to the Web project  cd eShop/eShop.MVC 
# 3. Apply migrations and seed data dotnet ef database update 
# 4. Run the application dotnet run
```

Then go to `https://localhost:7064` in your browser.

----------

## 🔐 Login Details

### 👑 Super Admin

-   **Email:** `superadmin@gmail.com`
    
-   **Password:** `superadmin@gmail.com`
    

### 👤 Normal User

-   **Email:** `user@gmail.com`
    
-   **Password:** `user@gmail.com`
    

> ⚠️ _These credentials are for testing or demo purposes only. Make sure to update them before deploying to production._

----------

## 📸 Screenshots

![Login page](https://github.com/AbdullahHanfi/eShop/blob/master/images/1%20(1).png)

![Admin Category Management](https://github.com/AbdullahHanfi/eShop/blob/master/images/1%20(2).png)

![Profile](https://github.com/AbdullahHanfi/eShop/blob/master/images/1%20(3).png)

![Admin product view](https://github.com/AbdullahHanfi/eShop/blob/master/images/1%20(4).png)

![Product details](https://github.com/AbdullahHanfi/eShop/blob/master/images/1%20(5).png)

![Admin product edit](https://github.com/AbdullahHanfi/eShop/blob/master/images/1%20(6).png)

![Admin create product](https://github.com/AbdullahHanfi/eShop/blob/master/images/1%20(7).png)

----------

## 🧩 Sample Code

Here’s a simple example for InMemory Cache:

```csharp
[HttpGet("categories")]
public async Task<IActionResult> GetCategories()
{
    if (_cache.TryGetValue(CacheKeys.ProductsKey, out List<CategoryViewModel> cachedCategories))
    {
        return Ok(cachedCategories);
    }

    var model = await _unitOfWork.Categories.FindAllAsync(e => e.MainLayout);

    if (model is null)
        return Ok(new List<CategoryViewModel>());

    var result = new List<CategoryViewModel>();
    result = _mapper.Map<List<CategoryViewModel>>(model.ToList());

    _cache.Set(CacheKeys.CategoriesKey, result, TimeSpan.FromMinutes(5));

    return Ok(result);
}
```
Here’s a simple example for Dapper:

```csharp
public TEntity GetById(Guid Id)
{
    try
    {
        using (var _connection = new SqlConnection(_CN))
        {
            string tableName = GetTableName(typeof(TEntity));
            string idName = GetKeyColumnName(typeof(TEntity));
            string query = $"SELECT * FROM {tableName} where {idName} = @Id AND IsDeleted = 'false'";
            return _connection.QueryFirstOrDefault<TEntity>(query, new { Id = Id });
        }
    }
    catch (Exception ex) { }
    return null;
}
```
Here’s a simple example for reading from EF metaData:

```csharp
protected string GetKeyColumnName(Type entityType)
{
    var entity = _context.Model.FindEntityType(entityType);
    if (entity == null)
        return string.Empty;

    var key = entity.FindPrimaryKey();
    if (key == null || key.Properties.Count == 0)
        return string.Empty;

    var keyProperty = key.Properties[0];

    var tableName = entity.GetTableName();
    var schema = entity.GetSchema();

    return keyProperty.GetColumnName(StoreObjectIdentifier.Table(tableName, schema));
}
```
Here’s a simple example of modular service registration keeps the `Program.cs` clean and your layers loosely coupled :

```csharp
public static class ServiceRegisterationBLL
{
    public static void Add(this IServiceCollection services)
    {
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IAccountServies, AccountServies>();
        services.AddScoped<IImageServices, ImageServices>();
        services.AddScoped<IMailTransport, MailTransport>();
        services.AddScoped<IGuidProvider, GuidProvider>();
        services.AddAutoMapper(typeof(AccountProfile));
        services.AddAutoMapper(typeof(ProductProfile));

    }
}
public class Program
{
    public static void Main(string[] args)
    {
        ServiceRegisterationBLL.Add(builder.Services);
    }
}
```

----------

## 📌 TODO / Roadmap

-   Payment integration
    
-   Order history and checkout process
    
-   Email notifications for new products

-   Ability for super admin to manage admins 

-   Unit testing for other component 

-   Adding docker image in Docker Hub 

-   Add Logging

----------

## 🤝 Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you’d like to change.

