﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using rgproj.Data;

#nullable disable

namespace rgproj.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250205060737_AddGeneratedReport")]
    partial class AddGeneratedReport
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("rgproj.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("rgproj.Models.EnvironmentalistForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActionsTaken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSubmitted")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FollowUpAction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsReported")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReportedDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubmittedByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WitnessDetails")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubmittedByUserId");

                    b.ToTable("EnvironmentalistForms");
                });

            modelBuilder.Entity("rgproj.Models.GeneratedReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("GeneratedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("ModelUsed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GeneratedReports");
                });

            modelBuilder.Entity("rgproj.Models.HealthOfficerForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ComplianceStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSubmitted")
                        .HasColumnType("datetime2");

                    b.Property<string>("DiseaseVectorPresent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnforcementMeasures")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FacilityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InspectionResults")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicHealthGuidance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicHealthRisk")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SanitationStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubmittedByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WasteDisposalEvaluation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WaterQualityAssessment")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubmittedByUserId");

                    b.ToTable("HealthOfficerForms");
                });

            modelBuilder.Entity("rgproj.Models.SpecialistForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AffectedDemographic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("AntibioticResistanceObserved")
                        .HasColumnType("bit");

                    b.Property<string>("CaseType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContainmentMeasures")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSubmitted")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExposureHistory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LaboratoryFindings")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RequiresNCDCNotification")
                        .HasColumnType("bit");

                    b.Property<string>("SeverityLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpecialistComments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubmittedByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TransmissionPattern")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubmittedByUserId");

                    b.ToTable("SpecialistForms");
                });

            modelBuilder.Entity("rgproj.Models.VeterinaryDoctorForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AnimalSpecies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClinicalSymptoms")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSubmitted")
                        .HasColumnType("datetime2");

                    b.Property<string>("FollowUpProtocol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HealthStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PotentialZoonoticRisk")
                        .HasColumnType("bit");

                    b.Property<string>("PreliminaryDiagnosis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("QuarantineRecommended")
                        .HasColumnType("bit");

                    b.Property<string>("SubmittedByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SuspectedDisease")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VaccinationDetails")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubmittedByUserId");

                    b.ToTable("VeterinaryForms");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("rgproj.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("rgproj.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rgproj.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("rgproj.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("rgproj.Models.EnvironmentalistForm", b =>
                {
                    b.HasOne("rgproj.Models.AppUser", "SubmittedByUser")
                        .WithMany()
                        .HasForeignKey("SubmittedByUserId");

                    b.Navigation("SubmittedByUser");
                });

            modelBuilder.Entity("rgproj.Models.HealthOfficerForm", b =>
                {
                    b.HasOne("rgproj.Models.AppUser", "SubmittedByUser")
                        .WithMany()
                        .HasForeignKey("SubmittedByUserId");

                    b.Navigation("SubmittedByUser");
                });

            modelBuilder.Entity("rgproj.Models.SpecialistForm", b =>
                {
                    b.HasOne("rgproj.Models.AppUser", "SubmittedByUser")
                        .WithMany()
                        .HasForeignKey("SubmittedByUserId");

                    b.Navigation("SubmittedByUser");
                });

            modelBuilder.Entity("rgproj.Models.VeterinaryDoctorForm", b =>
                {
                    b.HasOne("rgproj.Models.AppUser", "SubmittedByUser")
                        .WithMany()
                        .HasForeignKey("SubmittedByUserId");

                    b.Navigation("SubmittedByUser");
                });
#pragma warning restore 612, 618
        }
    }
}
