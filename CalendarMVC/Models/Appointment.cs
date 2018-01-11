namespace CalendarMVC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appointment")]
    public partial class Appointment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Appointment()
        {
            Attendances = new HashSet<Attendance>();
        }
        [Key]
        public Guid AppointmentID { get; set; }

        [Required]
        [StringLength(16)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [DisplayFormat(DataFormatString = @"hh\:mm", ApplyFormatInEditMode = true)]
        public TimeSpan StartTime { get; set; }

        [DisplayFormat(DataFormatString = @"hh\:mm", ApplyFormatInEditMode = true)]
        public TimeSpan EndTime { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] timestamp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }

        public void copy(Appointment a)
        {
            this.AppointmentDate = a.AppointmentDate;
            this.AppointmentID = a.AppointmentID;
            this.Attendances = a.Attendances;
            this.EndTime = a.EndTime;
            this.StartTime = a.StartTime;
            this.Title = a.Title;
            this.Description = a.Description;
        }
    }
}
