using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name).HasColumnType("NVarchar").HasMaxLength(50);
            
            builder.Property(x => x.Email).HasColumnType("NVarchar").HasMaxLength(100);

            builder.HasIndex(x => x.Phone).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PhoneConstraint", "Phone Like '011%' Or Phone Like '010%'");
                tb.HasCheckConstraint("EmailConstraint", "Email Like '_%@_%._%'");

            });

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street");
                address.Property(a => a.City).HasColumnName("City");
                address.Property(a => a.BuildingNumber).HasColumnName("BuildingNumber");
            });




        }
    }
}
