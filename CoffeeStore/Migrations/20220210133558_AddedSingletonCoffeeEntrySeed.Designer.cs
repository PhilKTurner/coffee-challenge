﻿// <auto-generated />
using CoffeeChallenge.CoffeeStore.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoffeeStore.Migrations
{
    [DbContext(typeof(CoffeeStoreContext))]
    [Migration("20220210133558_AddedSingletonCoffeeEntrySeed")]
    partial class AddedSingletonCoffeeEntrySeed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CoffeeChallenge.CoffeeStore.DataAccess.Coffee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Inventory")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Coffee");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Inventory = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}