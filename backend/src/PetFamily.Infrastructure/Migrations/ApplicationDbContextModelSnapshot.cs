﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.Infrastructure;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.Domain.PetManagement.AggregateRoot.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.Domain.PetManagement.AggregateRoot.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .HasMaxLength(1000)
                                .HasColumnType("character varying(1000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "PetFamily.Domain.PetManagement.AggregateRoot.Volunteer.Email#EmailAddress", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Experience", "PetFamily.Domain.PetManagement.AggregateRoot.Volunteer.Experience#Experience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("experience");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "PetFamily.Domain.PetManagement.AggregateRoot.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("first_name");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("patronymic");

                            b1.Property<string>("Surname")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("surname");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetFamily.Domain.PetManagement.AggregateRoot.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.PetManagement.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp(6)")
                        .HasColumnName("created_at");

                    b.Property<string>("HelpStatus")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("help_status");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "PetFamily.Domain.PetManagement.Entities.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("address_city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("address_country");

                            b1.Property<string>("Flat")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("address_flat");

                            b1.Property<string>("House")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("address_house");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("address_street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.Domain.PetManagement.Entities.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .HasMaxLength(1000)
                                .HasColumnType("character varying(1000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Details", "PetFamily.Domain.PetManagement.Entities.Pet.Details#PetDetails", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateTime>("Birthday")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("birthday_date");

                            b1.Property<string>("Color")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("color");

                            b1.Property<string>("HealthInfo")
                                .IsRequired()
                                .HasMaxLength(8000)
                                .HasColumnType("character varying(8000)")
                                .HasColumnName("health_info");

                            b1.Property<double>("Height")
                                .HasColumnType("double precision")
                                .HasColumnName("height");

                            b1.Property<bool>("IsCastrated")
                                .HasColumnType("boolean")
                                .HasColumnName("is_castrated");

                            b1.Property<bool>("IsVaccinated")
                                .HasColumnType("boolean")
                                .HasColumnName("is_vaccinated");

                            b1.Property<double>("Weight")
                                .HasColumnType("double precision")
                                .HasColumnName("weight");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetFamily.Domain.PetManagement.Entities.Pet.Name#PetName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetFamily.Domain.PetManagement.Entities.Pet.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("character varying(250)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Position", "PetFamily.Domain.PetManagement.Entities.Pet.Position#Position", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("position");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SpeciesDetails", "PetFamily.Domain.PetManagement.Entities.Pet.SpeciesDetails#SpeciesDetails", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.PetManagement.ValueObjects.PetPhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("path");

                    b.Property<Guid?>("pet_id")
                        .HasColumnType("uuid")
                        .HasColumnName("pet_id");

                    b.HasKey("Id")
                        .HasName("pk_pet_photos");

                    b.HasIndex("pet_id")
                        .HasDatabaseName("ix_pet_photos_pet_id");

                    b.ToTable("pet_photos", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesManagement.AggregateRoot.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetFamily.Domain.SpeciesManagement.AggregateRoot.Species.Name#SpeciesName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesManagement.Entities.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("species_id")
                        .HasColumnType("uuid")
                        .HasColumnName("species_id");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetFamily.Domain.SpeciesManagement.Entities.Breed.Name#BreedName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_breeds");

                    b.HasIndex("species_id")
                        .HasDatabaseName("ix_breeds_species_id");

                    b.ToTable("breeds", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.PetManagement.AggregateRoot.Volunteer", b =>
                {
                    b.OwnsOne("PetFamily.Domain.PetManagement.ValueObjects.VolunteerRequisites", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("requisites");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetFamily.Domain.Shared.ValueObjects.Requisite", "Values", b2 =>
                                {
                                    b2.Property<Guid>("VolunteerRequisitesVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .HasMaxLength(1000)
                                        .HasColumnType("character varying(1000)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("VolunteerRequisitesVolunteerId", "Id")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("VolunteerRequisitesVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_volunteer_requisites_volunteer_id");
                                });

                            b1.Navigation("Values");
                        });

                    b.OwnsOne("PetFamily.Domain.PetManagement.ValueObjects.VolunteerSocialNetworks", "SocialNetworks", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("social_networks");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetFamily.Domain.PetManagement.ValueObjects.SocialNetwork", "Values", b2 =>
                                {
                                    b2.Property<Guid>("VolunteerSocialNetworksVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.Property<string>("Url")
                                        .IsRequired()
                                        .HasMaxLength(500)
                                        .HasColumnType("character varying(500)");

                                    b2.HasKey("VolunteerSocialNetworksVolunteerId", "Id")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("VolunteerSocialNetworksVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_volunteer_social_networks_volunteer_id");
                                });

                            b1.Navigation("Values");
                        });

                    b.Navigation("Requisites")
                        .IsRequired();

                    b.Navigation("SocialNetworks")
                        .IsRequired();
                });

            modelBuilder.Entity("PetFamily.Domain.PetManagement.Entities.Pet", b =>
                {
                    b.HasOne("PetFamily.Domain.PetManagement.AggregateRoot.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("PetFamily.Domain.PetManagement.ValueObjects.PaymentDetails", "PaymentDetails", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("payment_details");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("PetFamily.Domain.Shared.ValueObjects.Requisite", "Requisites", b2 =>
                                {
                                    b2.Property<Guid>("PaymentDetailsPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(1000)
                                        .HasColumnType("character varying(1000)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("PaymentDetailsPetId", "Id")
                                        .HasName("pk_pets");

                                    b2.ToTable("pets");

                                    b2.WithOwner()
                                        .HasForeignKey("PaymentDetailsPetId")
                                        .HasConstraintName("fk_pets_pets_payment_details_pet_id");
                                });

                            b1.Navigation("Requisites");
                        });

                    b.Navigation("PaymentDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("PetFamily.Domain.PetManagement.ValueObjects.PetPhoto", b =>
                {
                    b.HasOne("PetFamily.Domain.PetManagement.Entities.Pet", null)
                        .WithMany("Photos")
                        .HasForeignKey("pet_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("fk_pet_photos_pets_pet_id");
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesManagement.Entities.Breed", b =>
                {
                    b.HasOne("PetFamily.Domain.SpeciesManagement.AggregateRoot.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("species_id")
                        .HasConstraintName("fk_breeds_species_species_id");
                });

            modelBuilder.Entity("PetFamily.Domain.PetManagement.AggregateRoot.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });

            modelBuilder.Entity("PetFamily.Domain.PetManagement.Entities.Pet", b =>
                {
                    b.Navigation("Photos");
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesManagement.AggregateRoot.Species", b =>
                {
                    b.Navigation("Breeds");
                });
#pragma warning restore 612, 618
        }
    }
}
