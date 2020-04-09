using CourierApplication.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierApplication.DAL
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Courier>().HasData(
                new Courier
                {
                    CourierId = 1,
                    FirstName = "Marcin",
                    LastName = "Kowalski",
                    isFree = true
                },
                new Courier
                {
                    CourierId = 2,
                    FirstName = "Tomasz",
                    LastName = "Nowak",
                    isFree = true
                },
                new Courier
                {
                    CourierId = 3,
                    FirstName = "Tomasz",
                    LastName = "Gajda",
                    isFree = false
                }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    ClientId = 1,
                    AdressId = 1,
                },
                new Client
                {
                    ClientId = 2,
                    AdressId = 2,
                },
                new Client
                {
                    ClientId = 3,
                    AdressId = 3,
                },
                new Client
                {
                    ClientId = 4,
                    AdressId = 4,
                },
                new Client
                {
                    ClientId = 5,
                    AdressId = 5,
                },
                new Client
                {
                    ClientId = 6,
                    AdressId = 6,
                },
                new Client
                {
                    ClientId = 7,
                    AdressId = 7,
                },
                new Client
                {
                    ClientId = 8,
                    AdressId = 8,
                },
                new Client
                {
                    ClientId = 9,
                    AdressId = 9,
                },
                new Client
                {
                    ClientId = 10,
                    AdressId = 10,
                },
                new Client
                {
                    ClientId = 11,
                    AdressId = 11,
                },
                new Client
                {
                    ClientId = 12,
                    AdressId = 12
                },
                new Client
                {
                    ClientId = 13,
                    AdressId = 13
                },
                new Client
                {
                    ClientId = 14,
                    AdressId = 14
                },
                new Client
                {
                    ClientId = 15,
                    AdressId = 15
                },
                new Client
                {
                    ClientId = 16,
                    AdressId = 16
                },
                new Client
                {
                    ClientId = 17,
                    AdressId = 17
                }
            );

            modelBuilder.Entity<Order>().HasData(
               new Order
               {
                   OrderId = 1,
                   AdressId = 2,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 2,
                   AdressId = 10,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 3,
                   AdressId = 5,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 4,
                   AdressId = 8,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 5,
                   AdressId = 14,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 6,
                   AdressId = 10,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 7,
                   AdressId = 4,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 8,
                   AdressId = 15,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 9,
                   AdressId = 11,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 10,
                   AdressId = 7,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 11,
                   AdressId = 12,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 12,
                   AdressId = 6,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 13,
                   AdressId = 3,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 14,
                   AdressId = 17,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 15,
                   AdressId = 13,
                   isCompleted = false
               },
               new Order
               {
                    OrderId = 16,
                    AdressId = 9,
                    isCompleted = false
               },
               new Order
               {
                   OrderId = 17,
                   AdressId = 16,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 18,
                   AdressId = 4,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 19,
                   AdressId = 11,
                   isCompleted = false
               },
               new Order
               {
                   OrderId = 20,
                   AdressId = 8,
                   isCompleted = false
               }
           );


            modelBuilder.Entity<Adress>().HasData(
              new Adress //1
              {
                  AdressId = 1,
                  Name = "Sienkiewicza 13/4",
                  Latitude = 51.117193M,
                  Longitude = 17.044819M,
              },
              new Adress //2
              {
                  AdressId = 2,
                  Name = "Grunwaldzka 71/4",
                  Latitude = 51.114836M,
                  Longitude = 17.068532M,
              },
              new Adress //3
              {
                  AdressId = 3,
                  Name = "Legnicka 42/18",
                  Latitude = 51.114899M,
                  Longitude = 17.001978M,
              },
              new Adress //4
              {
                  AdressId = 4,
                  Name = "Nowowiejska 31/10",
                  Latitude = 51.123066M,
                  Longitude = 17.049608M,
              },
              new Adress //5
              {
                  AdressId = 5,
                  Name = "Rakietowa 20/9",
                  Latitude = 51.098380M,
                  Longitude = 16.936670M,
              },
              new Adress //6
              {
                  AdressId = 6,
                  Name = "Targowa 87/11",
                  Latitude = 51.065627M,
                  Longitude = 16.957833M,
              },
              new Adress //7
              {
                  AdressId = 7,
                  Name = "Bystrzycka 64/3",
                  Latitude = 51.119083M,
                  Longitude = 16.978771M,
              },
              new Adress //8
              {
                  AdressId = 8,
                  Name = "Bajana 82/16",
                  Latitude = 51.124014M,
                  Longitude = 16.960451M,
              },
              new Adress //9
              {
                  AdressId = 9,
                  Name = "Terenowa 8/2",
                  Latitude = 51.069190M,
                  Longitude = 17.040798M,
              },
              new Adress //10
              {
                  AdressId = 10,
                  Name = "Kolista 32/5",
                  Latitude = 51.135035M,
                  Longitude = 16.973569M,
              },
              new Adress //11
              {
                  AdressId = 11,
                  Name = "Pretficza 27/19",
                  Latitude = 51.091435M,
                  Longitude = 17.008427M,
              },
              new Adress
              {
                  AdressId = 12,
                  Name = "Drzewieckiego 24/29",
                  Latitude = 51.1259106M,
                  Longitude = 16.9693086M,
              },
              new Adress
              {
                  AdressId = 13,
                  Name = "Gazowa 50/11",
                  Latitude = 51.078977M,
                  Longitude = 17.066367M,
              },
              new Adress
              {
                  AdressId = 14,
                  Name = "Edwarda Dembowskiego 13/4",
                  Latitude = 51.106747M,
                  Longitude = 17.086136M,
              },
              new Adress
              {
                  AdressId = 15,
                  Name = "Lekarska 44/12",
                  Latitude = 51.158208M,
                  Longitude = 17.032974M,
              },
              new Adress
              {
                  AdressId = 16,
                  Name = "Raclawicka 61/3",
                  Latitude = 51.080764M,
                  Longitude = 16.997120M, 
              },
              new Adress
              {
                  AdressId = 17,
                  Name = "Brzozowa 10a",
                  Latitude = 51.057948M,
                  Longitude = 17.058763M,
              }

           );




        }
    }
}
