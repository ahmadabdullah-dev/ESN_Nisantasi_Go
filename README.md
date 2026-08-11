# Social Plan Platform

## How to Run

Set the configurations in `appsettings.json`.

```json
"ConnectionStrings": {
  "DefaultConnection": ""
},
"EmailConfiguration": {
  "From": "",
  "smtpServer": "",
  "Port": "",
  "UserName": "",
  "Password": "",
  "EnableSsl": ""
},
```

## Database Migrations

Run these commands from the **solution root**.

**Add a migration:**

```powershell
dotnet ef migrations add Mig_1 `
  --project .\DataAccess\DataAccess.csproj `
  --startup-project .\API\API.csproj
```

**Apply migrations:**

```powershell
 dotnet ef database update `
  --project .\DataAccess\DataAccess.csproj `
  --startup-project .\API\API.csproj
