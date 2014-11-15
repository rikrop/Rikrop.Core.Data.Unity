using System;
using System.ComponentModel.DataAnnotations;
using Rikrop.Core.Data.Entities;
using Rikrop.Core.Data.Entities.Contracts;

namespace Rikrop.Core.Data.TestApp
{
    public class Person : DeactivatableEntity<Int32>, IRetrievableEntity<Person, int>
    {
        [Required]
        public string Name { get; set; }
    }
}
