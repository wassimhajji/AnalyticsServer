﻿// <auto-generated />
using System;
using AnalyticsServer.MessagesDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AnalyticsServer.Migrations
{
    [DbContext(typeof(MessagesDb))]
    [Migration("20220217102228_StreamGroupingModifications")]
    partial class StreamGroupingModifications
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AnalyticsServer.MessagesDatabase.Hardware", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("CpuGnice")
                        .HasColumnType("float");

                    b.Property<double>("CpuGuest")
                        .HasColumnType("float");

                    b.Property<double>("CpuIRQ")
                        .HasColumnType("float");

                    b.Property<double>("CpuIdle")
                        .HasColumnType("float");

                    b.Property<double>("CpuIoWait")
                        .HasColumnType("float");

                    b.Property<double>("CpuNice")
                        .HasColumnType("float");

                    b.Property<double>("CpuSoft")
                        .HasColumnType("float");

                    b.Property<double>("CpuSteal")
                        .HasColumnType("float");

                    b.Property<double>("CpuSys")
                        .HasColumnType("float");

                    b.Property<double>("CpuUser")
                        .HasColumnType("float");

                    b.Property<int>("IODiskRead")
                        .HasColumnType("int");

                    b.Property<int>("IODiskWrite")
                        .HasColumnType("int");

                    b.Property<int>("IONetIn")
                        .HasColumnType("int");

                    b.Property<int>("IONetOut")
                        .HasColumnType("int");

                    b.Property<string>("IOTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RamBoot")
                        .HasColumnType("int");

                    b.Property<int>("RamCache")
                        .HasColumnType("int");

                    b.Property<int>("RamSwap")
                        .HasColumnType("int");

                    b.Property<int>("RamTotal")
                        .HasColumnType("int");

                    b.Property<int>("RamUsed")
                        .HasColumnType("int");

                    b.Property<string>("SlaveId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeAdded")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Hardware");
                });

            modelBuilder.Entity("AnalyticsServer.MessagesDatabase.HardwareDisks", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TimeAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Available")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileSystem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MontedOn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SlaveId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Use")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Used")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id", "TimeAdded");

                    b.ToTable("HardwareDisks");
                });

            modelBuilder.Entity("AnalyticsServer.MessagesDatabase.Stream", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SlaveId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("StreamId")
                        .HasColumnType("int");

                    b.Property<string>("AudioBitrate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AudioCodec")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fps")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Height")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Speed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("VideoBitrate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoCodec")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Width")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("state")
                        .HasColumnType("int");

                    b.HasKey("Id", "SlaveId", "StreamId");

                    b.ToTable("Streams");
                });

            modelBuilder.Entity("AnalyticsServer.MessagesDatabase.StreamGrouping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StreamId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeAdded")
                        .HasColumnType("datetime2");

                    b.Property<int>("UsersNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("StreamsGrouping");
                });

            modelBuilder.Entity("AnalyticsServer.MessagesDatabase.UsersConnectionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ConnectionsNumber")
                        .HasColumnType("int");

                    b.Property<string>("SlaveId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsersNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UsersConnection");
                });

            modelBuilder.Entity("AnalyticsServer.MessagesDatabase.Vod", b =>
                {
                    b.Property<Guid>("VodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ExistantList")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SlaveId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VodId");

                    b.ToTable("Vod");
                });
#pragma warning restore 612, 618
        }
    }
}
