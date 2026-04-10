# Patungan

## Database Migrations

This project uses Entity Framework Core for database management. Below are the common commands for working with migrations.

### Prerequisites

Make sure you have the EF Core tools installed:

```bash
dotnet tool install --global dotnet-ef
```

Or update to the latest version:

```bash
dotnet tool update --global dotnet-ef
```

### Adding a New Migration

To create a new migration after making changes to your models:

```bash
dotnet ef migrations add <MigrationName> --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

Example:
```bash
dotnet ef migrations add AddUserTable --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

### Updating the Database

To apply pending migrations to the database:

```bash
dotnet ef database update --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

To update to a specific migration:

```bash
dotnet ef database update <MigrationName> --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

To rollback to a previous migration:

```bash
dotnet ef database update <PreviousMigrationName> --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

### Other Useful Commands

List all migrations:
```bash
dotnet ef migrations list --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

Remove the last migration (only if not applied to database):
```bash
dotnet ef migrations remove --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

Generate SQL script for migrations:
```bash
dotnet ef migrations script --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

Drop the database:
```bash
dotnet ef database drop --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```

Get information about the DbContext:
```bash
dotnet ef dbcontext info --startup-project ../Patungan.API --project ../Patungan.DataAccess --context PatunganDbContext
```