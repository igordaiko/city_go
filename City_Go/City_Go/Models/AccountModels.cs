using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;
namespace City_Go.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext() : base("DefaultConnection")
        {

        }
        public DbSet<UserProfile> UsersProfiles { get; set; }
    }
    [Table("UserProfile")]
    public class UserProfile
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
    }
    public class LoginModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool Remember { get; set; }
    }
    public class RegisterModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
    public class UsersProfiles
    {
        public List<UserProfile> Users { get; set; }
        public string[] Roles { get; set; }
    }
}