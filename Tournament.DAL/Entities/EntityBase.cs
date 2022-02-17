﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Tournament.DAL.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; init; }
    }
}
