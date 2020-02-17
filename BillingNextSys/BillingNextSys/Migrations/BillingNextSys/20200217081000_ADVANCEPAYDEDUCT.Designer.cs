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
    [Migration("20200217081000_ADVANCEPAYDEDUCT")]
    partial class ADVANCEPAYDEDUCT
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("BillingNextSys.Models.AdvancePay", b =>
                {
                    b.Property<int>("AdvancePayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("AdvanceAmount")
                        .HasColumnType("double precision");

                    b.Property<int>("BranchID")
                        .HasColumnType("integer");

                    b.Property<string>("ChequeNumber")
                        .HasColumnType("text");

                    b.Property<bool>("ChequePaymet")
                        .HasColumnType("boolean");

                    b.Property<int>("CompanyID")
                        .HasColumnType("integer");

                    b.Property<int>("DebtorGroupID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("AdvancePayID");

                    b.HasIndex("BranchID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DebtorGroupID");

                    b.ToTable("AdvancePay");
                });

            modelBuilder.Entity("BillingNextSys.Models.AdvancePayDeduct", b =>
                {
                    b.Property<int>("AdvancePayDeductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("AdvanceAmountDeducted")
                        .HasColumnType("double precision");

                    b.Property<int>("BillDetailsID")
                        .HasColumnType("integer");

                    b.Property<int>("BranchID")
                        .HasColumnType("integer");

                    b.Property<int>("CompanyID")
                        .HasColumnType("integer");

                    b.Property<int>("DebtorGroupID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DeductDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("AdvancePayDeductID");

                    b.HasIndex("BillDetailsID");

                    b.HasIndex("BranchID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DebtorGroupID");

                    b.ToTable("AdvancePayDeduct");
                });

            modelBuilder.Entity("BillingNextSys.Models.Bill", b =>
                {
                    b.Property<string>("BillNumber")
                        .HasColumnType("text");

                    b.Property<int?>("BillActNum")
                        .HasColumnType("integer");

                    b.Property<double>("BillAmount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("BillDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("BillDelivered")
                        .HasColumnType("boolean");

                    b.Property<string>("BilledTo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BranchID")
                        .HasColumnType("integer");

                    b.Property<int>("CompanyID")
                        .HasColumnType("integer");

                    b.Property<int>("DebtorGroupID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool?>("MessageSent")
                        .HasColumnType("boolean");

                    b.Property<string>("Note")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<int?>("PlaceOfSupply")
                        .HasColumnType("integer");

                    b.Property<int>("SecretUnlockCode")
                        .HasColumnType("integer");

                    b.Property<string>("SeriesName")
                        .HasColumnType("text");

                    b.HasKey("BillNumber");

                    b.HasIndex("BillActNum")
                        .IsUnique();

                    b.HasIndex("BranchID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DebtorGroupID");

                    b.HasIndex("SeriesName");

                    b.ToTable("Bill");
                });

            modelBuilder.Entity("BillingNextSys.Models.BillDetails", b =>
                {
                    b.Property<int>("BillDetailsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<double>("BillAmountOutstanding")
                        .HasColumnType("double precision");

                    b.Property<string>("BillNumber")
                        .HasColumnType("text");

                    b.Property<double>("CGSTAmount")
                        .HasColumnType("double precision");

                    b.Property<int>("CompanyID")
                        .HasColumnType("integer");

                    b.Property<int>("DebtorGroupID")
                        .HasColumnType("integer");

                    b.Property<int?>("DebtorID")
                        .HasColumnType("integer");

                    b.Property<int>("ParticularsID")
                        .HasColumnType("integer");

                    b.Property<string>("ParticularsName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Period")
                        .HasColumnType("text");

                    b.Property<double>("SGSTAmount")
                        .HasColumnType("double precision");

                    b.Property<double>("TaxableValue")
                        .HasColumnType("double precision");

                    b.Property<int?>("YearInfo")
                        .HasColumnType("integer");

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
                    b.Property<string>("SeriesName")
                        .HasColumnType("text");

                    b.HasKey("SeriesName");

                    b.ToTable("BillSeries");
                });

            modelBuilder.Entity("BillingNextSys.Models.Branch", b =>
                {
                    b.Property<int>("BranchID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("BranchAddress")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.Property<string>("BranchEmail")
                        .IsRequired()
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.Property<string>("BranchManagerName")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<byte[]>("BranchManagerSign")
                        .HasColumnType("bytea");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<string>("BranchPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CompanyID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("BranchID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Branch");
                });

            modelBuilder.Entity("BillingNextSys.Models.Company", b =>
                {
                    b.Property<int>("CompanyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AccountType")
                        .HasColumnType("integer");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("BillFormat")
                        .HasColumnType("integer");

                    b.Property<byte[]>("CompanyLogoImg")
                        .HasColumnType("bytea");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<string>("CompanyOwner")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("GSTIN")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IFSCcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PAN")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CompanyID");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("BillingNextSys.Models.Debtor", b =>
                {
                    b.Property<int>("DebtorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DebtorGroupID")
                        .HasColumnType("integer");

                    b.Property<string>("DebtorName")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<double>("DebtorOutstanding")
                        .HasColumnType("double precision");

                    b.HasKey("DebtorID");

                    b.HasIndex("DebtorGroupID");

                    b.ToTable("Debtor");
                });

            modelBuilder.Entity("BillingNextSys.Models.DebtorGroup", b =>
                {
                    b.Property<int>("DebtorGroupID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("AdvancePayAmount")
                        .HasColumnType("double precision");

                    b.Property<int>("BranchID")
                        .HasColumnType("integer");

                    b.Property<string>("DebtorGSTIN")
                        .HasColumnType("text");

                    b.Property<string>("DebtorGroupAddress")
                        .IsRequired()
                        .HasColumnType("character varying(350)")
                        .HasMaxLength(350);

                    b.Property<string>("DebtorGroupCity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DebtorGroupMail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DebtorGroupName")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<string>("DebtorGroupPhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("DebtorOutstanding")
                        .HasColumnType("double precision");

                    b.HasKey("DebtorGroupID");

                    b.HasIndex("BranchID");

                    b.ToTable("DebtorGroup");
                });

            modelBuilder.Entity("BillingNextSys.Models.Particulars", b =>
                {
                    b.Property<int>("ParticularsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("ParticularsName")
                        .IsRequired()
                        .HasColumnType("character varying(150)")
                        .HasMaxLength(150);

                    b.Property<int>("SACCode")
                        .HasColumnType("integer");

                    b.HasKey("ParticularsID");

                    b.ToTable("Particulars");
                });

            modelBuilder.Entity("BillingNextSys.Models.Received", b =>
                {
                    b.Property<int>("ReceivedID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BillDetailsID")
                        .HasColumnType("integer");

                    b.Property<string>("ChequeNumber")
                        .HasColumnType("text");

                    b.Property<bool>("ChequePaymet")
                        .HasColumnType("boolean");

                    b.Property<int>("CompanyID")
                        .HasColumnType("integer");

                    b.Property<int>("DebtorGroupID")
                        .HasColumnType("integer");

                    b.Property<double>("ReceivedAmount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ReceivedID");

                    b.HasIndex("BillDetailsID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("DebtorGroupID");

                    b.ToTable("Received");
                });

            modelBuilder.Entity("BillingNextSys.Models.AdvancePay", b =>
                {
                    b.HasOne("BillingNextSys.Models.Branch", "Branch")
                        .WithMany("AdvancePays")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("AdvancePays")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("AdvancePays")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillingNextSys.Models.AdvancePayDeduct", b =>
                {
                    b.HasOne("BillingNextSys.Models.BillDetails", "BillDetails")
                        .WithMany("AdvancePayDeducts")
                        .HasForeignKey("BillDetailsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.Branch", "Branch")
                        .WithMany("AdvancePayDeducts")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("AdvancePayDeducts")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("AdvancePayDeducts")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillingNextSys.Models.Bill", b =>
                {
                    b.HasOne("BillingNextSys.Models.Branch", "Branch")
                        .WithMany("Bills")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("Bills")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("Bills")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.BillSeries", "BillSeries")
                        .WithMany("Bills")
                        .HasForeignKey("SeriesName");
                });

            modelBuilder.Entity("BillingNextSys.Models.BillDetails", b =>
                {
                    b.HasOne("BillingNextSys.Models.Bill", "Bill")
                        .WithMany("BillDetails")
                        .HasForeignKey("BillNumber")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("BillDetails")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("BillDetails")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.Debtor", "Debtor")
                        .WithMany("BillDetails")
                        .HasForeignKey("DebtorID");

                    b.HasOne("BillingNextSys.Models.Particulars", "Particulars")
                        .WithMany("BillDetails")
                        .HasForeignKey("ParticularsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillingNextSys.Models.Branch", b =>
                {
                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("Branches")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillingNextSys.Models.Debtor", b =>
                {
                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("Debtors")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillingNextSys.Models.DebtorGroup", b =>
                {
                    b.HasOne("BillingNextSys.Models.Branch", "Branch")
                        .WithMany("DebtorGroups")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BillingNextSys.Models.Received", b =>
                {
                    b.HasOne("BillingNextSys.Models.BillDetails", "BillDetails")
                        .WithMany("Receiveds")
                        .HasForeignKey("BillDetailsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.Company", "Company")
                        .WithMany("Receiveds")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillingNextSys.Models.DebtorGroup", "DebtorGroup")
                        .WithMany("Receiveds")
                        .HasForeignKey("DebtorGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
