using EquipmentWatcher.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EquipmentWatcher
{
    public class DbApp : DbContext //IdentityDbContext<Account>
    {
        public DbApp(DbContextOptions<DbApp> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<AccessDevice> AccessDevices { get; set; }
        public DbSet<Interaction> Interactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Access>()
                .HasOne(a => a.ReceiverAccount)
                .WithMany()
                .HasForeignKey(a => a.ReceiverAccountID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Access>()
                .HasOne(a => a.ProviderAccount)
                .WithMany()
                .HasForeignKey(a => a.ProviderAccountID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccessToken>()
                .HasOne(a => a.Account)
                .WithOne()
                .HasForeignKey<AccessToken>(a => a.AccountID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    PersonID = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    MiddleName = "Smith",
                    Email = "josh_ds@mail.test"
                },
                new Person
                {
                    PersonID = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    MiddleName = "Smith",
                    Email = "jane_ds@mail.test"
                },
                new Person
                {
                    PersonID = 3,
                    FirstName = "Mykola",
                    LastName = "Gorbenko",
                    MiddleName = "Petrovych",
                    Email = "mykola_gp@mail.test"
                },
                new Person
                {
                    PersonID = 4,
                    FirstName = "Andriy",
                    LastName = "Sternenko",
                    MiddleName = "Serhiyovych",
                    Email = "andriy_ss@mail.test"
                }
                );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountID = 1,
                    PersonID = 1,
                    Login = "josh_ds",
                    Password = "11..Aa",
                    LastSession = DateTime.UtcNow
                },
                new Account
                {
                    AccountID = 2,
                    PersonID = 2,
                    Login = "jane_ds",
                    Password = "22..Bb",
                    LastSession = DateTime.UtcNow
                },
                new Account
                {
                    AccountID = 3,
                    PersonID = 3,
                    Login = "mykola_gp",
                    Password = "33..Cc",
                    LastSession = DateTime.UtcNow
                },
                new Account
                {
                    AccountID = 4,
                    PersonID = 4,
                    Login = "andriy_ss",
                    Password = "44..Dd",
                }
                );

            modelBuilder.Entity<Permission>().HasData(
                new Permission
                {
                    PermissionID = 1,
                    AccountID = 1,
                    Value = "Administrator",
                },
                new Permission {
                    PermissionID = 2,
                    AccountID = 1,
                    Value = "Secretary"
                },
                new Permission {
                    PermissionID = 3,
                    AccountID = 1,
                    Value = "User"
                },
                new Permission
                {
                    PermissionID = 4,
                    AccountID = 2,
                    Value = "Secretary",
                },
                new Permission {
                    PermissionID = 5,
                    AccountID = 2,
                    Value = "User"
                },
                new Permission
                {
                    PermissionID = 6,
                    AccountID = 3,
                    Value = "User",
                },
                new Permission
                {
                    PermissionID = 7,
                    AccountID = 4,
                    Value = "User",
                });

            modelBuilder.Entity<AccessDevice>().HasData(
                new AccessDevice
                {
                    AccessDeviceID = 1,
                    MacAddress = "88:88:88:88:88:88",
                    Name = "Test device",
                    Description = "Test device description"
                });

            modelBuilder.Entity<Access>().HasData(
                new Access
                {
                    AccessID = 1,
                    ProviderAccountID = 1,
                    ReceiverAccountID = 1,
                    AccessDeviceID = 1,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddDays(2),
                    AllowProvide = true
                },
                new Access
                {
                    AccessID = 2,
                    ProviderAccountID = 1,
                    ReceiverAccountID = 3,
                    AccessDeviceID = 1,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddDays(1),
                    AllowProvide = true
                });
        }
    }
}
