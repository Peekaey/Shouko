﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Shouko.DataService;

#nullable disable

namespace Shouko.DataService.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250608023241_Add_Archived_Flag_To_ApiCounter_Tables")]
    partial class Add_Archived_Flag_To_ApiCounter_Tables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shouko.Models.DatabaseModels.ApiRequestCounter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ApiPromptType")
                        .HasColumnType("integer");

                    b.Property<int>("ApiType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("ApiRequestCounters");
                });

            modelBuilder.Entity("Shouko.Models.DatabaseModels.ApiRequestLimitCounter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ApiPromptType")
                        .HasColumnType("integer");

                    b.Property<int>("ApiType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<int>("LimitCounterType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ApiRequestLimitCounters");
                });

            modelBuilder.Entity("Shouko.Models.DatabaseModels.ApiResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ApiPromptType")
                        .HasColumnType("integer");

                    b.Property<int>("ApiType")
                        .HasColumnType("integer");

                    b.Property<int>("CandidatesTokenCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DiscordInteractionId")
                        .HasColumnType("integer");

                    b.Property<string>("InputText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("boolean");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PromptTokenCount")
                        .HasColumnType("integer");

                    b.Property<string>("ResponseId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ResponseImageContent")
                        .HasColumnType("text");

                    b.Property<string>("ResponseText")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DiscordInteractionId");

                    b.ToTable("ApiResponses");
                });

            modelBuilder.Entity("Shouko.Models.DatabaseModels.DiscordInteraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("InteractionId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("DiscordInteractions");
                });

            modelBuilder.Entity("Shouko.Models.DatabaseModels.ApiResponse", b =>
                {
                    b.HasOne("Shouko.Models.DatabaseModels.DiscordInteraction", "DiscordInteraction")
                        .WithMany()
                        .HasForeignKey("DiscordInteractionId");

                    b.Navigation("DiscordInteraction");
                });
#pragma warning restore 612, 618
        }
    }
}
