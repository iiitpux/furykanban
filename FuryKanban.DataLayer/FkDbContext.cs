using FuryKanban.DataLayer.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FuryKanban.DataLayer
{
    public class FkDbContext : DbContext
    {
        public DbSet<AccountDto> Accounts { get; set; }
        public DbSet<IssueDto> Issues { get; set; }
        public DbSet<ProjectDto> Projects { get; set; }
        public DbSet<StageDto> Stages { get; set; }
        public DbSet<TokenDto> Tokens { get; set; }
        public DbSet<UserDto> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbName = "TestDatabase.db";
            //if (File.Exists(dbName))
            //{
            //    File.Delete(dbName);
            //}
            optionsBuilder.UseSqlite("Filename=" + dbName, options =>
            {
                //options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Author>()
            //    .HasOne(a => a.Biography)
            //    .WithOne(b => b.Author)
            //    .HasForeignKey<AuthorBiography>(b => b.AuthorRef);
            base.OnModelCreating(modelBuilder);
        }

        public void CreateDefaultProject(int accountId, int userId)
        {
            var project = new ProjectDto()
            {
                AccountId = accountId,
                Title = "Стартовая",
                IsSelected = true
            };
            var firstStage = new StageDto()
            {
                Order = 1,
                Title = "Новое"
            };
            firstStage.Issues = new List<IssueDto>();
            var firstIssue = new IssueDto()
            {
                AssignedDateTime = DateTime.Now,
                AssignedUserId = userId,
                CreatedDateTime = DateTime.Now,
                CreatedUserId = userId,
                Title = "Добавить свой проект",
                Description = "Описание того как добавить",
                Order = 1
            };
            firstStage.Issues.Add(firstIssue);
            var secondStage = new StageDto()
            {
                Order = 2,
                Title = "В работе"
            };
            secondStage.Issues = new List<IssueDto>();
            var secondIssue = new IssueDto()
            {
                AssignedDateTime = DateTime.Now,
                AssignedUserId = userId,
                CreatedDateTime = DateTime.Now,
                CreatedUserId = userId,
                Title = "Добавить задачу",
                Description = "Описание того как добавить",
                Order = 2
            };
            secondStage.Issues.Add(secondIssue);
            var thirdStage = new StageDto()
            {
                Order = 3,
                Title = "Готово"
            };
            thirdStage.Issues = new List<IssueDto>();
            var thisrdIssue = new IssueDto()
            {
                AssignedDateTime = DateTime.Now,
                AssignedUserId = userId,
                CreatedDateTime = DateTime.Now,
                CreatedUserId = userId,
                Title = "Зарегистрироваться в системе",
                Description = "Вай молодец",
                Order = 3
            };
            thirdStage.Issues.Add(thisrdIssue);
            project.Stages = new List<StageDto>();
            project.Stages.Add(firstStage);
            project.Stages.Add(secondStage);
            project.Stages.Add(thirdStage);
            this.Projects.Add(project);
            this.SaveChanges();
        }

        //todo перенести в logic
        public UserDto AddAccountOwnerUser(string login, string password)
        {
            var account = new AccountDto();
            this.Add(account);
            this.SaveChanges();
            // добавим пользователя
            string salt = Guid.NewGuid().ToString();
            string passHash = Hashing.GetPasswordHash(password, salt);
            var user = new UserDto()
            {
                Login = login.ToLower(),
                Password = passHash,
                CreateDate = DateTime.Now,
                Salt = salt,
                AccountId = account.Id
            };
            this.Add(user);
            this.SaveChanges();
            CreateDefaultProject(account.Id, user.Id);
            return user;
        }


        public static class Hashing
        {
            #region SHA1
            public static string GetSHA1(byte[] data)
            {
                SHA1 shaHasher = SHA1.Create();
                byte[] hash = shaHasher.ComputeHash(data);

                return BitConverter.ToString(hash).Replace("-", "");
            }
            public static string GetSHA1(string data)
            {
                byte[] byteData = System.Text.Encoding.UTF8.GetBytes(data);
                return GetSHA1(byteData);
            }
            #endregion
            public static string GetPasswordHash(string password, string salt)
            {
                return Hashing.GetSHA1(password + Hashing.GetSHA1(salt));
            }
        }

    }
}
