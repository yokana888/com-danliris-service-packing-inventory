﻿using Com.Danliris.Service.Packing.Inventory.Data.Models.Garmentshipping.GarmentShippingCostStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Packing.Inventory.Infrastructure.EntityConfigurations.GarmentShipping.GarmentShippingCostStructure
{
    public class GarmentShippingCostStructureEntityTypeConfiguration : IEntityTypeConfiguration<GarmentShippingCostStructureModel>
    {
        public void Configure(EntityTypeBuilder<GarmentShippingCostStructureModel> builder)
        {
            /* StandardEntity */
            builder.HasKey(s => s.Id);
            builder.Property(s => s.CreatedAgent).HasMaxLength(128);
            builder.Property(s => s.CreatedBy).HasMaxLength(128);
            builder.Property(s => s.LastModifiedAgent).HasMaxLength(128);
            builder.Property(s => s.LastModifiedBy).HasMaxLength(128);
            builder.Property(s => s.DeletedAgent).HasMaxLength(128);
            builder.Property(s => s.DeletedBy).HasMaxLength(128);
            builder.HasQueryFilter(f => !f.IsDeleted);
            /* StandardEntity */

            builder
                .Property(s => s.InvoiceNo)
                .HasMaxLength(50);

            builder
                .HasIndex(i => i.InvoiceNo)
                .IsUnique()
                .HasFilter("[IsDeleted]=(0)");

            builder
                .Property(s => s.HsCode)
                .HasMaxLength(100);

            builder
                .Property(s => s.ComodityCode)
                .HasMaxLength(50);

            builder
                .Property(s => s.ComodityName)
                .HasMaxLength(255);

            builder
                .Property(s => s.Destination)
                .HasMaxLength(50);

            //builder
            //    .HasMany(h => h.Items)
            //    .WithOne()
            //    .HasForeignKey(f => f.CostStructureId);
        }
    }
}
