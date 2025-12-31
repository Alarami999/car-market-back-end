Car Marketplace API - Backend

Overview:
A comprehensive API for a car selling platform, built using a 3-Tier Architecture with ASP.NET Core and modern technologies for user management, shopping cart, payment processing, and authentication.

Architecture:
Presentation Layer (Controllers)
       ↓
 Business Layer (Services)
       ↓
 Data Access Layer (Repositories)
       ↓
 Database Layer (Entity Framework Core)
 
Prerequisites:
.NET 8.0 SDK or newer

SQL Server (LocalDB, Express, or Full)

Stripe Account (for payments)

Twilio Account (for SMS)

IDE: Visual Studio 2022+ or VS Code with C# extension

Project Structure:
car-marketplace-api-3tier/

├── Models/           # Data Entities (DTOs & Entities) 

├── Data/            # Database Context (DbContext)

├── Repositories/    # Data Access Layer

├── Services/        # Business Logic Layer

├── Controllers/     # API Endpoints

├── Settings/        # Configuration Files

├── DTOs/           # Data Transfer Objects

├── appsettings.json # Main Configuration File

└── Program.cs       # Entry Point & Application Initialization


Key Features:
    Authentication & Authorization:
     JWT Authentication with Refresh Tokens
     OTP Verification via SMS using Twilio
     User Roles: Owner and Customer
     Role-based access control

   Car Management:
    Full CRUD operations for cars
    Car details: make, model, year, price, location, image, description
    Seed data included in the database

   Shopping Cart:
    Add/remove cars from cart
    Quantity management
    Convert cart to order

  Order & Payment System:
   Create orders from cart
   Stripe Integration for secure payments
   Webhook processing for payment confirmation
   Order statuses: Pending, Paid, Shipped, Delivered, Canceled

User Management:
 Login using phone number and password
 New account registration with OTP verification
 Session renewal (Refresh Tokens)

Setup & Configuration:


Database Initialization:
  # Update database using EF Core Migrations
   dotnet ef database update

Configure appsettings.json:
Add your keys in the following sections:
json
{
  "Jwt": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "Key": "your-secret-key-min-32-chars"
  },
  "PaymentSettings": {
    "StripeSecretKey": "sk_test_your_key",
    "StripePublishableKey": "pk_test_your_key",
    "StripeWebhookSecret": "whsec_your_secret",
    "Currency": "SAR"
  },
  "TwilioSettings": {
    "AccountSid": "your_account_sid",
    "AuthToken": "your_auth_token",
    "FromPhoneNumber": "+1234567890"
  }
}


Configure Database Connection:

Update the connection string in Program.cs:
options.UseSqlServer("Server=YOUR_SERVER;Database=CarsDB;Trusted_Connection=True;TrustServerCertificate=True;");

Running the Application:
 Using CLI
 # Restore packages
 dotnet restore

 # Build project
 dotnet build

 # Run application
  dotnet run

 # Or run in development mode
  dotnet watch run

  
Using Visual Studio:
  Open car-marketplace-api-3tier.sln file
  Press F5 to run with debugging
  Or Ctrl + F5 to run without debugging

API Endpoints:
Authentication:
Method	Path	Description
POST	/Auth/login	User login
POST	/Auth/request-otp	Request OTP code
POST	/Auth/verify-otp-and-register	Verify OTP and register
POST	/Auth/refresh	Refresh session tokens

Cars:
Method	Path	Description	Permissions
GET	/Cars	Get all cars	Public
GET	/Cars/{id}	Get specific car	Public
POST	/Cars	Add new car	Owner
PUT	/Cars/{id}	Update car	Owner
DELETE	/Cars/{id}	Delete car	Owner

Shopping Cart:
Method	Path	Description
GET	/Cart	View cart contents
POST	/Cart/add/{carId}	Add car to cart
DELETE	/Cart/remove/{carId}	Remove car from cart
DELETE	/Cart/clear	Clear cart

Payment:
Method	Path	Description
POST	/Payment/checkout	Create order and initiate payment
GET	/Config/stripe-key	Get Stripe public key
POST	/stripe-webhook	Process payment notifications (Stripe Webhook)


Permissions & Roles:
Customer:
Browse cars
Manage shopping cart
Create orders
Complete payments

Owner:
All customer permissions
Add/edit/delete cars
Inventory management

Authentication Tokens:
JWT Token Flow:
Login → Receive Access Token (15 min) + Refresh Token (7 days)
When Access Token expires → Use Refresh Token to get new tokens
Refresh Token can be manually revoked when needed

OTP Verification Flow:
Enter phone number → Send OTP via SMS
Enter code with password → Create new account
Delete code after use (5-minute validity)

Stripe Integration:

Payment Process:
Add cars to cart
Convert cart to Order
Create Stripe Checkout Session
Redirect user to Stripe payment page
After successful payment → Receive Webhook
Update order status to "Paid"

Stripe Settings:
Default currency: SAR (Saudi Riyal)
Payment methods: Card only
Webhook URL: https://your-domain.com/stripe-webhook

Twilio SMS Integration:

SMS Features:
Send OTP codes for registration
Automated message formatting
Error handling and retry logic

Sample Data:

Default Users
[
  { "PhoneNumber": "newowner", "Password": "456", "Role": "Owner" },
  { "PhoneNumber": "newcustomer", "Password": "789", "Role": "Customer" }
]

Default Cars
[
  { "Make": "Toyota", "Model": "Corolla", "Year": 2020, "Price": 15000 },
  { "Make": "Honda", "Model": "Civic", "Year": 2019, "Price": 14000 },
  { "Make": "Ford", "Model": "Mustang", "Year": 2021, "Price": 30000 }
]

Testing & Development:
Using Swagger UI:
 After running the application, navigate to:
  https://localhost:{port}/swagger

Testing Endpoints
# Test getting cars
curl -X GET https://localhost:7219/Cars

# Test login
curl -X POST https://localhost:7219/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"phoneNumber":"newcustomer","password":"789"}'
  
Security Considerations:

Best Practices Implemented:
Password encryption using PasswordHasher<User>
JWT Tokens with limited validity
HTTPS enforced for all communications
Endpoint-level permission checking
Centralized error handling

Settings to Update Before Production:
Change JWT secret keys
Update Stripe and Twilio settings with production keys
Configure CORS for allowed domains only
Set up production database

Dependencies:

Main NuGet Packages:
Microsoft.AspNetCore.Authentication.JwtBearer - JWT authentication
Microsoft.EntityFrameworkCore.SqlServer - Database ORM
Stripe.net - Payment integration
Twilio - SMS sending
Swashbuckle.AspNetCore - API documentation and testing

Troubleshooting:

Common Issues:
Database connection error: Verify SQL Server is running and connection string is correct
JWT Token issue: Check Issuer, Audience, and Key settings
SMS sending failure: Verify Twilio keys and account balance
Stripe problem: Check keys and Webhook Secret

Application Logs:
Enable Logging in appsettings.json for more details
Monitor Console Output during development

License:
This project is for educational and development use. Please review third-party library terms of use.

Contributing:
Fork the project
Create feature branch (git checkout -b feature/AmazingFeature)
Commit changes (git commit -m 'Add some AmazingFeature')
Push to branch (git push origin feature/AmazingFeature)
Open Pull Request
