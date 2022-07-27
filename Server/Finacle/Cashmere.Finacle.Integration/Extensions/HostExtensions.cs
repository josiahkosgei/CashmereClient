using Cashmere.Finacle.Integration.CQRS.DataAccessLayer.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Cashmere.Finacle.Integration.Extensions
{

    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DepositorServerContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    var retryPolicy = Policy.Handle<SqlException>()
                            .WaitAndRetry(
                                retryCount: 5,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                                onRetry: (exception, retryCount, context) =>
                                {
                                    logger.LogError(message: $"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                                });


                    //apply to transient exceptions
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        //policy.Execute(async () => await ResetDatabaseFriendlyWay(context, logger));
                        ResetDatabaseFriendlyWay(context, logger);
                    }

                    retryPolicy.Execute(() => InvokeSeeder(seeder, context, services));

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DepositorServerContext
        {
            context.Database.Migrate();
        }
        private static void ResetDatabaseFriendlyWay<TContext>(TContext context, ILogger<TContext> logger)
            where TContext : DepositorServerContext
        {


            try
            {
                using var dbContextTransaction = context.Database.BeginTransaction();
                #region SqlScript
                // Azure friendly Sql Script
                var dropForeignKeyconstraints = @"
                                                DECLARE @name VARCHAR(128)
                                                DECLARE @constraint VARCHAR(254)
                                                DECLARE @SQL VARCHAR(254)
                                                DECLARE @SQL2 VARCHAR(254)
                                                DECLARE @SQL3 VARCHAR(254)

                                                SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)

                                                WHILE @name is not null
                                                BEGIN
                                                    SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                                                    WHILE @constraint IS NOT NULL
                                                    BEGIN
                                                        IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'History') AND @name != '__EFMigrationsHistory'
                                                            BEGIN
                                                                 SELECT @SQL = 'ALTER TABLE [history].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint) +']'
                                                                 EXEC (@SQL)
                                                            END
                                                        SELECT @SQL2 = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint) +']'
                                                        EXEC (@SQL2)
                                                        PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name
                                                        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                                                    END
                                                SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)
                                                END;

                                                    WHILE @name IS NOT NULL
                                                    BEGIN
                                                        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                                                        WHILE @constraint is not null
                                                        BEGIN
                                                            IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'History') AND @name != '__EFMigrationsHistory'
                                                                BEGIN
                                                                    SELECT @SQL = 'ALTER TABLE [history].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint)+']'
                                                                    EXEC (@SQL)
                                                                END
                                                            SELECT @SQL2 = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint) +']'
                                                            EXEC (@SQL2)
                                                            PRINT 'Dropped PK Constraint: ' + @constraint + ' on ' + @name
                                                            SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                                                        END
                                                    SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY TABLE_NAME)
                                                    END;

                                           SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 ORDER BY [name])
                                            WHILE @name IS NOT NULL
                                            BEGIN
                                                IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'History') AND @name != '__EFMigrationsHistory'
                                                    BEGIN
                                                        SELECT @SQL = ('ALTER TABLE [' + RTRIM(@name) +'] SET (SYSTEM_VERSIONING = OFF)');
                                                        EXEC (@SQL)
                                                        SELECT @SQL2 = 'DROP TABLE [history].[' + RTRIM(@name) +']'
                                                        EXEC (@SQL2)
                                                    END
                                                SELECT @SQL3 = 'DROP TABLE [dbo].[' + RTRIM(@name) +']'
                                                EXEC (@SQL3)
                                                PRINT 'Dropped Table: ' + @name
                                                SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 AND [name] > @name ORDER BY [name])
                                            END                                           

                                            IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'History')
                                            BEGIN
                                                DROP SCHEMA History
                                            END
                                                ";
                #endregion

                context.Database.ExecuteSqlRaw(dropForeignKeyconstraints);

                logger.LogInformation("Cleared database tables associated with context {DbContextName}", typeof(TContext).Name);
                context.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred while Clearing Database used on context {DbContextName}", typeof(TContext).Name);
            }

        }
    }
}
