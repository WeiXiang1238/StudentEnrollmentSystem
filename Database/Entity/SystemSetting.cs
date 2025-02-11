using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentEnrollmentSystem.Database.Entity
{
    public class SystemSetting
    {
        [Key]
        public int SystemSettingID { get; set; }

        [Required]
        public int EnabledEnrollment { get; set; }

        [Required]
        public int CurrentSemester { get; set; }
    }
}
