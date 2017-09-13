using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using demo.MysqlRepositoryLib;

namespace demo.MysqlRepositoryLib.Migrations
{
    [DbContext(typeof(DemoContext))]
    [Migration("20170228024455_addUser")]
    partial class addUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("ChaosCore.ModelBase.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<long>("Creator");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<bool>("Lock");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<long>("UpdateUserID");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });
        }
    }
}
