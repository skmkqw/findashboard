# Findashboard Backend

## Overview
Findashboard is a financial dashboard designed for teams, focusing on activities in the Web3 world. The backend handles user authentication, team and project management, activity tracking, and financial data management, including wallet balances.

## Tech Stack
- **Backend Framework**: .NET (ASP.NET Core)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core (EF Core)
- **Authentication**: JWT-based authentication

## Features
- User authentication (sign-up, login, JWT-based authentication)
- Team and project management (create, join, and manage teams)
- Activity tracking (log work done, assign to projects)
- Wallet and profile management
- Balance tracking

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/skmkqw/findashboard.git
   cd findashboard-backend
   ```
2. Install dependencies:
   ```sh
   dotnet restore
   ```
3. Set up environment variables in an `appsettings.json` file:
   ```json
   {
     "JwtSettings": {
       "Secret": "your_secret_key",
       "ExpiryDays": 1,
       "Issuer": "your_issuer",
       "Audience": "your_audince"
     }
   }
   ```
4. Change connection string in `DependencyInjection.cs` in Ifrastructure project
   ```csharp
       private static IServiceCollection AddPersistence(this IServiceCollection services)
       {
          services.AddDbContext<ZBankDbContext>(options => options.UseNpgsql(connectionString: "your_connection_string"));
          //....
       }
   ```
4. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
5. Start the server:
   ```sh
   dotnet run
   ```

## API HTTP Endpoints
### Important note: endoints listed below are already implemented. In future this list will be extended
### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Authenticate and retrieve a JWT token
- `POST /api/auth/logout` - Log out

### Teams & Personal Spaces
- `POST /api/teams` - Create a new team
- `POST /api/teams/invites/send` - Send join team request
- `POST /api/teams/invites/{inviteId}/accept` - Accept join team request
- `POST /api/teams/invites/{inviteId}/decline` - Decline join team request
- `POST /api/personalSpaces` - Create pesonal space
- `GET /api/personalSpaces` - Get personal space details

### Users & Notifications
- `GET /api/users/noitfications` - Get your notifications
- `PUT /api/notifications/{notificationId}/markAsRead` - Mark notification as read
- `DELETE /api/notification/{notificationId}` - Delete notification

### Profiles
- `POST /api/profiles` - Add a profile to a user

### Wallets & Balances
- `POST /api/wallets` - Add a wallet to a user’s profile
- `POST /api/balances` - Add a balance to a user's wallet

## API WSS Endpoints
### Important note: endoints listed below are already implemented. In future this list will be extended
- `wss://localhost:xxxx/notification-hub` - Get notifications in real-time
- `wss://localhost:xxxx/currency-hub` - Get currency price & price change updates in real-time

## Contributing
1. Fork the repository
2. Create a feature branch (`git checkout -b feature-name`)
3. Commit changes (`git commit -m "Add new feature"`)
4. Push to the branch (`git push origin feature-name`)
5. Open a pull request

## License
MIT License

## Links
[Frontend repository](https://github.com/skmkqw/zbank-frontend)
