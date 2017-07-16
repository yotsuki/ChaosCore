using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using demo.SqlRepositoryLib;
using ChaosCore.ModelBase;

namespace demo.SqlRepositoryLib.Migrations
{
    [DbContext(typeof(DemoContext))]
    partial class MDMContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChaosCore.ModelBase.Function", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<long>("Creator");

                    b.Property<string>("FuncCode")
                        .HasMaxLength(64);

                    b.Property<string>("FuncName")
                        .HasMaxLength(64);

                    b.Property<string>("Icon")
                        .HasMaxLength(64);

                    b.Property<bool>("IsMenu");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("OrderNo");

                    b.Property<long>("ParentFuncID");

                    b.Property<long>("UpdateUserID");

                    b.Property<string>("Url")
                        .HasMaxLength(256);

                    b.HasKey("ID");

                    b.ToTable("Functions");
                });

            modelBuilder.Entity("ChaosCore.ModelBase.Role", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<long>("Creator");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("RoleName")
                        .HasMaxLength(32);

                    b.Property<int>("Type");

                    b.Property<long>("UpdateUserID");

                    b.HasKey("ID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ChaosCore.ModelBase.RoleFunction", b =>
                {
                    b.Property<long>("RoleId");

                    b.Property<long>("FunctionId");

                    b.HasKey("RoleId", "FunctionId");

                    b.HasIndex("FunctionId");

                    b.ToTable("RoleFunction");
                });

            modelBuilder.Entity("ChaosCore.ModelBase.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias")
                        .HasMaxLength(32);

                    b.Property<DateTime>("Birthday");

                    b.Property<DateTime>("CreateTime");

                    b.Property<long>("Creator");

                    b.Property<string>("Email");

                    b.Property<decimal>("Eyes")
                        .HasAnnotation("SqlServer:ColumnType", "decimal(12,4)");

                    b.Property<string>("Face")
                        .HasMaxLength(64);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<bool>("Lock");

                    b.Property<string>("Memo")
                        .HasMaxLength(1024);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("Sex");

                    b.Property<string>("Tel")
                        .HasMaxLength(32);

                    b.Property<long>("UpdateUserID");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChaosCore.ModelBase.UserRole", b =>
                {
                    b.Property<long>("RoleId");

                    b.Property<long>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("ChaosCore.ModelBase.RoleFunction", b =>
                {
                    b.HasOne("ChaosCore.ModelBase.Function", "Function")
                        .WithMany("Roles")
                        .HasForeignKey("FunctionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChaosCore.ModelBase.Role", "Role")
                        .WithMany("Functions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChaosCore.ModelBase.UserRole", b =>
                {
                    b.HasOne("ChaosCore.ModelBase.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChaosCore.ModelBase.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
