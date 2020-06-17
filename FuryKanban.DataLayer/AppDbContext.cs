using FuryKanban.DataLayer.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FuryKanban.Common;

namespace FuryKanban.DataLayer
{
	public class AppDbContext : DbContext
	{
		public DbSet<IssueDto> Issues { get; set; }
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

		public void CreateDefaultProject(int userId)
		{
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
				Body = "Описание того как добавить",
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
				Body = "Описание того как добавить",
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
				Body = "Вай молодец",
				Order = 3
			};
			thirdStage.Issues.Add(thisrdIssue);
			this.SaveChanges();
		}

		//todo перенести в logic
		public UserDto AddAccountOwnerUser(string login, string password)
		{
			// добавим пользователя
			string salt = Guid.NewGuid().ToString();
			string passHash = Hashing.GetPasswordHash(password, salt);
			var user = new UserDto()
			{
				Login = login.ToLower(),
				Password = passHash,
				CreateDate = DateTime.Now,
				Salt = salt
			};
			this.Add(user);
			this.SaveChanges();
			CreateDefaultProject(user.Id);
			return user;
		}




	}
}
