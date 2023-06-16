using System.ComponentModel.DataAnnotations;

namespace Buytopia.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CouponType { get; set; }
        
        public enum ECouponType {Percent=0, Dollar=1 }
        [Required]
        public double Discount { get; set; }

        [Required]
        public double MinimumAmount { get; set; }   
        //image saved on database not on server so byte
        public byte[] Picture { get; set; }
        public bool IsActive { get; set; }  
    }
}
