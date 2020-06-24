using FuryKanban.DataLayer.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FuryKanban.Common;
using Microsoft.Extensions.Logging;

namespace FuryKanban.DataLayer
{
	public class AppDbContext : DbContext
	{
		public DbSet<IssueDto> Issues { set; get; }
		public DbSet<StageDto> Stages { set; get; }
		public DbSet<TokenDto> Tokens { set; get; }
		public DbSet<UserDto> Users { set; get; }
		public DbSet<HistoryDto> History { set; get; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string dbName = "furykanbandatabase.db";

			optionsBuilder.UseSqlite("Filename=" + dbName);

			optionsBuilder.UseLoggerFactory(MyLoggerFactory);

			base.OnConfiguring(optionsBuilder);
		}

		public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
		{
			builder.AddConsole();
		});
	}
}
