﻿// <auto-generated />
using System;
using BillingNextSys.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BillingNextSys.Migrations.BillingNextSys
{
    [DbContext(typeof(BillingNextSysContext))]
    [Migration("20190108075357_ownername")]
    partial class ownername
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("BillingNextSys.Models.Bill", b =>
                {
                    b.Property<string>("BillNumber");

                    b.Property<double>("BillAmount");

                    b.Property<string>("BilledTo")
                        .IsRequired();

                    b.Property<int>("CompanyID");

                    b.Property<int>("DebtorGroupID");

                    b.Property<DateTime>("InvoiceDate");

                    b.Property<string>("Note")
                        .HasMaxLength(500);

                    b.Property<int?>("PlaceOfSupply");

                    b.Property<string>("SeriesName");

                    b.HasKey("BillNumber");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DebtorGroupID");

                    b.HasIndex("SeriesName");

                    b.ToTable("Bill");
                });

            modelBuilder.Entity("BillingNextSys.Models.BillDetails", b =>
                {
                    b.Property<int>("BillDetailsID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<double>("BillAmountOutstanding");

                    b.Property<string>("BillNumber");

                    b.Property<double>("CGSTAmount");

                    b.Property<int>("CompanyID");

                    b.Property<int>("DebtorGroupID");

                    b.Property<int>("DebtorID");

                    b.Property<int>("ParticularsID");

                    b.Property<string>("ParticularsName")
                        .IsRequired();

                    b.Property<string>("Period")
                        .IsRequired();

                    b.Property<double>("SGSTAmount");

                    b.Property<double>("TaxableValue");

                    b.Property<int>("YearInfo");

                    b.HasKey("BillDetailsID");

                    b.HasIndex("BillNumber");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DebtorGroupID");

                    b.HasIndex("DebtorID");

                    b.HasIndex("ParticularsID");

                    b.ToTable("BillDetails");
                });

            modelBuilder.Entity("BillingNextSys.Models.BillSeries", b =>
                {
                    b.Property<string>("SeriesName");

                    b.HasKey("SeriesName");

                    b.ToTable("BillSeries");
                });

            modelBuilder.Entity("BillingNextSys.Models.Branch", b =>
                {
                    b.Property<int>("BranchID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BranchAddress")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("BranchEmail")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("BranchPhone")
                        .IsRequired();

                    b.Property<int>("CompanyID");

                    b.Property<DateTime>("CreationDate");

                    b.HasKey("BranchID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Branch");
                });

            modelBuilder.Entity("BillingNextSys.Models.Company", b =>
                {
                    b.Property<int>("CompanyID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountNumber")
                        .IsRequired();

                    b.Property<int>("AccountType");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("BillFormat");

                    b.Property<byte[]>("CompanyLogoImg");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("CompanyOwner")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("GSTIN")
                        .IsRequired();

                    b.Property<string>("IFSCcode")
                        .IsRequired();

                    b.Property<string>("PAN")
                        .IsRequired();

                    b.HasKey("CompanyID");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("BillingNextSys.Models.Debtor", b =>
                {
                    b.Property<int>("DebtorID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DebtorGroupID");

                    b.Property<string>("DebtorName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<double>("DebtorOutstanding");

                    b.HasKey("DebtorID");

                    b.HasIndex("DebtorGroupID");

                    b.ToTable("Debtor");
                });

            modelBuilder.Entity("BillingNextSys.Models.DebtorGroup", b =>
                {
                    b.Property<int>("DebtorGroupID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BranchID");

                    b.Property<string>("DebtorGSTIN");

                    b.Property<string>("DebtorGroupAddress")
                        .IsRequired()
                        .HasMaxLength(350);

                    b.Property<string>("DebtorGroupCity")
                        .IsRequired();

                    b.Property<string>("DebtorGroupMail")
                        .IsRequired();

                    b.Property<string>("DebtorGroupName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("DebtorGroupPhoneNumber")
                        .IsRequired();

                    b.Property<double>("DebtorOutstanding");

                    b.HasKey("DebtorGroupID");

                    b.HasIndex("BranchID");

                    b.ToTable("DebtorGroup");
                });

            modelBuilder.Entity("BillingNextSys.Models.Particulars", b =>
                {
                    b.Property<int>("ParticularsID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<string>("ParticularsName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("SACCode");

                    b.HasKey("ParticularsID");

                    b.ToTable("Particulars");
                });

            modelBuilder.Entity("BillingNextSys.Models.Received", b =>
                {
                    b.Property<int>("ReceivedID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BillDetailsID");

                    b.Property<string>("ChequeNumber");

                    b.Property<bool>("ChequePaymet");

                    b.Property<int>("CompanyID");

                    b.Property<int?>("DebtorGroupID");

                    b.Property<double>("ReceivedAmount");

                    b.Property<DateTime>("ReceivedDate");

                    b.HasKey("ReceivedID");

                    b.HasIndex("BillDetailsID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DebtorGroupID");

                    b.ToTable("Received");
                });

            modelBuilder.Entity("BillingNextSys.Models.Bill", b =>
                {
                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("Bills")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("Bills")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.BillSeries", "BillSeries")
                        .WithMany("Bills")
                        .HasForeignKey("SeriesName");
                });

            modelBuilder.Entity("BillingNextSys.Models.BillDetails", b =>
                {
                    b.HasOne("BillingNextSys.Models.Bill", "Bill")
                        .WithMany("BillDetails")
                        .HasForeignKey("BillNumber");

                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("BillDetails")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("BillDetails")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.Debtor", "Debtor")
                        .WithMany("BillDetails")
                        .HasForeignKey("DebtorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.Particulars", "Particulars")
                        .WithMany("BillDetails")
                        .HasForeignKey("ParticularsID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BillingNextSys.Models.Branch", b =>
                {
                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("Branches")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BillingNextSys.Models.Debtor", b =>
                {
                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("Debtors")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BillingNextSys.Models.DebtorGroup", b =>
                {
                    b.HasOne("BillingNextSys.Models.Branch", "Branch")
                        .WithMany("DebtorGroups")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BillingNextSys.Models.Received", b =>
                {
                    b.HasOne("BillingNextSys.Models.BillDetails", "BillDetails")
                        .WithMany("Receiveds")
                        .HasForeignKey("BillDetailsID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("Receiveds")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.DebtorGroup")
                        .WithMany("Receiveds")
                        .HasForeignKey("DebtorGroupID");
                });
#pragma warning restore 612, 618
        }
    }
}
