using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdressAccounting.Models;

public partial class AdressAccountingContext : DbContext
{
    public AdressAccountingContext()
    {
    }

    public AdressAccountingContext(DbContextOptions<AdressAccountingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adress> Adresses { get; set; }

    public virtual DbSet<AdressRecord> AdressRecords { get; set; }

    public virtual DbSet<AreaBuilding> AreaBuildings { get; set; }

    public virtual DbSet<MergeRecord> MergeRecords { get; set; }

    public virtual DbSet<MergedStreet> MergedStreets { get; set; }

    public virtual DbSet<SplitRecord> SplitRecords { get; set; }

    public virtual DbSet<SplitResult> SplitResults { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<StreetNameRecord> StreetNameRecords { get; set; }

    public virtual DbSet<StreetNameRecordsStreet> StreetNameRecordsStreets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Adress_pkey");

            entity.ToTable("Adress");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("areaId");
            entity.Property(e => e.IsActual).HasColumnName("isActual");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.StreetId).HasColumnName("streetId");

            entity.HasOne(d => d.Area).WithMany(p => p.Adresses)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("Adress_areaId_fkey");

            entity.HasOne(d => d.Street).WithMany(p => p.Adresses)
                .HasForeignKey(d => d.StreetId)
                .HasConstraintName("Adress_streetId_fkey");
        });

        modelBuilder.Entity<AdressRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AdressRecords_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AdressId).HasColumnName("adressId");
            entity.Property(e => e.AreaId).HasColumnName("areaId");
            entity.Property(e => e.DateFrom).HasColumnName("dateFrom");
            entity.Property(e => e.DateTo).HasColumnName("dateTo");
            entity.Property(e => e.Number).HasColumnName("number");

            entity.HasOne(d => d.Adress).WithMany(p => p.AdressRecords)
                .HasForeignKey(d => d.AdressId)
                .HasConstraintName("AdressRecords_adressId_fkey");

            entity.HasOne(d => d.Area).WithMany(p => p.AdressRecords)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("AdressRecords_areaId_fkey");
        });

        modelBuilder.Entity<AreaBuilding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Area(Building)_pkey");

            entity.ToTable("Area(Building)");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
        });

        modelBuilder.Entity<MergeRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MergeRecords_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.StreetIdResultOfMerging).HasColumnName("streetId(result of merging)");

            entity.HasOne(d => d.StreetIdResultOfMergingNavigation).WithMany(p => p.MergeRecords)
                .HasForeignKey(d => d.StreetIdResultOfMerging)
                .HasConstraintName("MergeRecords_streetId(result of merging)_fkey");
        });

        modelBuilder.Entity<MergedStreet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MergedStreets_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.MergeRecordsId).HasColumnName("mergeRecordsId");
            entity.Property(e => e.StreetId).HasColumnName("streetId");

            entity.HasOne(d => d.MergeRecords).WithMany(p => p.MergedStreets)
                .HasForeignKey(d => d.MergeRecordsId)
                .HasConstraintName("MergedStreets_mergeRecordsId_fkey");

            entity.HasOne(d => d.Street).WithMany(p => p.MergedStreets)
                .HasForeignKey(d => d.StreetId)
                .HasConstraintName("MergedStreets_streetId_fkey");
        });

        modelBuilder.Entity<SplitRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SplitRecords_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.StreetIdSplittedStreet).HasColumnName("streetId(splitted street)");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.SplitRecord)
                .HasForeignKey<SplitRecord>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SplitRecords_id_fkey");
        });

        modelBuilder.Entity<SplitResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SplitResults_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.SplitRecordsId).HasColumnName("splitRecordsId");
            entity.Property(e => e.StreetId).HasColumnName("streetId");

            entity.HasOne(d => d.SplitRecords).WithMany(p => p.SplitResults)
                .HasForeignKey(d => d.SplitRecordsId)
                .HasConstraintName("SplitResults_splitRecordsId_fkey");

            entity.HasOne(d => d.Street).WithMany(p => p.SplitResults)
                .HasForeignKey(d => d.StreetId)
                .HasConstraintName("SplitResults_streetId_fkey");
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Street_pkey");

            entity.ToTable("Street");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<StreetNameRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StreetNameRecords_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DateFrom).HasColumnName("dateFrom");
            entity.Property(e => e.DateTo).HasColumnName("dateTo");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<StreetNameRecordsStreet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StreetNameRecordsStreet_pkey");

            entity.ToTable("StreetNameRecordsStreet");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.StreetId).HasColumnName("streetId");
            entity.Property(e => e.StreetNameRecordsId).HasColumnName("streetNameRecordsId");

            entity.HasOne(d => d.Street).WithMany(p => p.StreetNameRecordsStreets)
                .HasForeignKey(d => d.StreetId)
                .HasConstraintName("StreetNameRecordsStreet_streetId_fkey");

            entity.HasOne(d => d.StreetNameRecords).WithMany(p => p.StreetNameRecordsStreets)
                .HasForeignKey(d => d.StreetNameRecordsId)
                .HasConstraintName("StreetNameRecordsStreet_streetNameRecordsId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
