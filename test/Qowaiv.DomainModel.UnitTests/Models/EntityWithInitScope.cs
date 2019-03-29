﻿using Qowaiv.ComponentModel;
using Qowaiv.ComponentModel.DataAnnotations;
using System;

namespace Qowaiv.DomainModel.UnitTests.Models
{
    public class EntityWithInitScope : Entity<EntityWithInitScope>
    {
        [Mandatory]
        public string Name
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        [Mandatory]
        public Date StartDate
        {
            get => GetProperty<Date>();
            set => SetProperty(value);
        }

        public static Result<EntityWithInitScope> FromData(Guid id, string name, Date startDate)
        {
            var entity = new EntityWithInitScope();

            return entity.SetProperties((e) =>
            {
                e.Id = id;
                e.Name = name;
                e.StartDate = startDate;
            });
        }
    }
}
