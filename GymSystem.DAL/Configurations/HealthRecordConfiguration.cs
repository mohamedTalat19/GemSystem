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
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealtRecord>
    {
        public void Configure(EntityTypeBuilder<HealtRecord> builder)
        {
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GetDate()");
        }
    }
}
