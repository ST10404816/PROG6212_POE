// Lisha Naidoo
// ST10404816
// Group 1

// References
// https://www.bytehide.com/blog/data-annotations-in-csharp

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Claim
{
    //------------------------------------------------------------------------------------------------------------------------//
    // Primary key for the Claim entity.
    [Key]
    public int Id { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // FirstName is required and must not exceed 50 characters.
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
    public string FirstName { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Surname is required and must not exceed 50 characters.
    [Required(ErrorMessage = "Surname is required")]
    [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
    public string Surname { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // HoursWorked is required and must be between 1 and 100.
    [Required(ErrorMessage = "Hours Worked is required")]
    [Range(1, 100, ErrorMessage = "Hours Worked must be between 1 and 100")]
    public int HoursWorked { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // HourlyRate is required and must be between 1 and 100.
    [Required(ErrorMessage = "Hourly Rate is required")]
    [Range(1, 100, ErrorMessage = "Hourly Rate must be between 1 and 100")]
    public decimal HourlyRate { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Cell phone number is required and must not exceed 20 characters.
    [Required(ErrorMessage = "Cellphone number is required")]
    [StringLength(20, ErrorMessage = "Cellphone number cannot exceed 20 characters")]
    public string CellNumber { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Email address must not exceed 50 characters.
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
    public string? Email { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Optional field for any additional notes, limited to 500 characters.
    [StringLength(500, ErrorMessage = "Additional notes cannot exceed 500 characters")]
    public string? AdditionalNotes { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Total amount, must be a positive value.
    // This value is calculated as: HoursWorked * HourlyRate.
    [NotMapped]
    [Range(0, double.MaxValue, ErrorMessage = "Total must be a positive number")]
    public decimal Total { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Status of the claim (e.g., pending, approved, rejected).
    // Default value is set to "pending" in the constructor.
    public string status { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Optional path to the uploaded file associated with the claim.
    public string? UploadedFilePath { get; set; }

    //------------------------------------------------------------------------------------------------------------------------//
    // Constructor to initialize the default status of a new claim to "pending".
    public Claim()
    {
        status = "pending";
    }
}
//------------------------------------------...ooo000 END OF FILE 000ooo...------------------------------------------------------//