using DocumentTranslationCenter.API.Core.Domain;
using DocumentTranslationCenter.API.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DocumentTranslationCenter.API.Core.Database
{
    public static class DtcDbContextExtensions
    {
        public static void SeedData(this DtcDbContext context)
        {
            User[] users = context.Users.ToArray();
            context.Users.RemoveRange(users);
            context.SaveChanges();

            string defaultPassword = "pass123";
            string defaultSalt = Utilities.Cryptography.Get128BitBase64Salt();
            string defaultHashedPassword = Utilities.Cryptography.HashPassword(defaultPassword, defaultSalt);
            users = new User[]
            {
                new User{Email="marcus.hetzer@gmail.com",   Salt=defaultSalt, Password=defaultHashedPassword},
                new User{Email="steven.gerlat@gmail.com",   Salt=defaultSalt, Password=defaultHashedPassword},
                new User{Email="leslie.lorian@gmail.com",   Salt=defaultSalt, Password=defaultHashedPassword},
                new User{Email="jacob.smith@gmail.com",     Salt=defaultSalt, Password=defaultHashedPassword},
                new User{Email="dennis.roddick@gmail.com",  Salt=defaultSalt, Password=defaultHashedPassword},
                new User{Email="wyl.ward@gmail.com",        Salt=defaultSalt, Password=defaultHashedPassword},
                new User{Email="walter.peters@gmail.com",   Salt=defaultSalt, Password=defaultHashedPassword},
                new User{Email="francis.urbano@gmail.com",  Salt=defaultSalt, Password=defaultHashedPassword}
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        public static void CreateTriggers(this DtcDbContext context)
        {
            PropertyInfo[] propertyInfos = typeof(DtcDbContext).GetProperties();
            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                Type genericType = propertyInfo.PropertyType;
                try
                {
                    Type concreteType = genericType.GetGenericArguments().Single();
                    if (concreteType.IsSubclassOf(typeof(BaseModel)))
                    {
                        string triggerCode = GetTriggerSQLCode(propertyInfo.Name);
                        context.Database.ExecuteSqlCommand(triggerCode);
                    }
                }
                catch (Exception)
                {
                    // TODO: log if needed ...
                }
            }
        }

        private static string GetTriggerSQLCode(string tableName)
        {
            return $"CREATE TRIGGER trigger_updated_{tableName} ON {tableName} AFTER UPDATE AS UPDATE {tableName} SET UpdatedAt = GETDATE() WHERE id IN (SELECT DISTINCT Id FROM Inserted)";
        }
    }
}
