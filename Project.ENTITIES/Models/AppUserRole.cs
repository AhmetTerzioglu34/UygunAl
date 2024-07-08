using Microsoft.AspNetCore.Identity;
using Project.ENTITIES.Enums;
using Project.ENTITIES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class AppUserRole : IdentityUserRole<int>,IEntity
    {
        public AppUserRole()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Inserted;

        }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; }

        //Relational Properties
        public virtual AppUser User { get; set; } //Property isminin User olduguna dikkat edin.Cünkü UserId property'sinden kaynaklanmaktadır
        public virtual AppRole Role { get; set; }
    }
}
