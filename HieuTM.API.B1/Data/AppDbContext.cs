using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HieuTM.API.B1.Entities;
using Microsoft.EntityFrameworkCore;

namespace HieuTM.API.B1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<SinhVien> SinhVien { get; set; } = default!;
    }
}
