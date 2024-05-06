﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyApp.DataBaseFolder;

#nullable disable

namespace MyApp.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyApp.Models.Article", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("date")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("likes")
                        .HasColumnType("int");

                    b.Property<string>("path_to_corer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userid");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("MyApp.Models.ArticleRequest", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("articleid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("articleid");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("MyApp.Models.Category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("Articleid")
                        .HasColumnType("int");

                    b.Property<string>("categoty_str")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Articleid");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("MyApp.Models.Comment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("Articleid")
                        .HasColumnType("int");

                    b.Property<string>("context")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("created")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("creatorid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("Articleid");

                    b.HasIndex("creatorid");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("MyApp.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Liked_articles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Userid")
                        .HasColumnType("int");

                    b.Property<string>("channelname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("path_to_icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("subscribers_num")
                        .HasColumnType("int");

                    b.Property<int>("user_info_id")
                        .HasColumnType("int");

                    b.Property<string>("user_role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Userid");

                    b.HasIndex("user_info_id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyApp.Models.UserInfo", b =>
                {
                    b.Property<int>("user_info_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("user_info_id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("user_info_id");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("MyApp.Models.Article", b =>
                {
                    b.HasOne("MyApp.Models.User", "user")
                        .WithMany("channel_articles")
                        .HasForeignKey("userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("MyApp.Models.ArticleRequest", b =>
                {
                    b.HasOne("MyApp.Models.Article", "article")
                        .WithMany()
                        .HasForeignKey("articleid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("article");
                });

            modelBuilder.Entity("MyApp.Models.Category", b =>
                {
                    b.HasOne("MyApp.Models.Article", null)
                        .WithMany("categories")
                        .HasForeignKey("Articleid");
                });

            modelBuilder.Entity("MyApp.Models.Comment", b =>
                {
                    b.HasOne("MyApp.Models.Article", null)
                        .WithMany("comments")
                        .HasForeignKey("Articleid");

                    b.HasOne("MyApp.Models.User", "creator")
                        .WithMany()
                        .HasForeignKey("creatorid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("creator");
                });

            modelBuilder.Entity("MyApp.Models.User", b =>
                {
                    b.HasOne("MyApp.Models.User", null)
                        .WithMany("my_subscribes")
                        .HasForeignKey("Userid");

                    b.HasOne("MyApp.Models.UserInfo", "user_info")
                        .WithMany()
                        .HasForeignKey("user_info_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user_info");
                });

            modelBuilder.Entity("MyApp.Models.Article", b =>
                {
                    b.Navigation("categories");

                    b.Navigation("comments");
                });

            modelBuilder.Entity("MyApp.Models.User", b =>
                {
                    b.Navigation("channel_articles");

                    b.Navigation("my_subscribes");
                });
#pragma warning restore 612, 618
        }
    }
}
