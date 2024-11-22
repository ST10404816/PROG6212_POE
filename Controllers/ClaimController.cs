// Lisha Naidoo
// ST10404816
// Group 1

// References
// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
// https://dotnethow.net/a-step-by-step-guide-to-configuring-entity-framework-in-your-net-web-api-project/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POE.Data;

namespace POE.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _context;  // Dependency Injection of AppDbContext.

        //------------------------------------------------------------------------------------------------------------------------//
        // Constructor to inject the AppDbContext dependency.
        public ClaimController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult AutoApproveClaims(int minHoursWorked, int maxHoursWorked, int minHourlyRate, int maxHourlyRate)
        {
            try
            {
                // Fetch all claims
                var claims = _context.Claims.ToList();

                // Iterate through claims and evaluate based on criteria
                foreach (var claim in claims)
                {
                    if ((claim.HoursWorked >= minHoursWorked && claim.HoursWorked <= maxHoursWorked) && (claim.HourlyRate >= minHourlyRate && claim.HourlyRate <= maxHourlyRate))
                    {
                        claim.status = "Approved"; // Approve if criteria are met
                    }
                    else
                    {
                        claim.status = "Declined"; // Decline otherwise
                    }
                }

                // Save changes to the database
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Claims processed successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Edit");
        }



        // Action to generate a report of approved claims
        public IActionResult GenerateReport()
        {
            // Filter approved claims
            var approvedClaims = _context.Claims.Where(c => c.status == "Approved").ToList();

            if (!approvedClaims.Any())
            {
                TempData["ErrorMessage"] = "No approved claims available to generate a report.";
                return RedirectToAction("ViewClaims");
            }

            // Generate CSV content
            var csvContent = "First Name,Surname,Hours Worked,Hourly Rate,Additional Notes,Status\n";

            foreach (var claim in approvedClaims)
            {
                csvContent += $"{claim.FirstName},{claim.Surname},{claim.HoursWorked},{claim.HourlyRate},{claim.AdditionalNotes},{claim.status}\n";
            }

            // Convert the CSV content to a memory stream
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(csvContent);
            writer.Flush();
            stream.Position = 0;

            return File(stream, "text/csv", "ApprovedClaimsReport.csv");
        }


        //------------------------------------------------------------------------------------------------------------------------//
        // Updates the status of a claim by ID.
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            // Find the claim with the given ID.
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound(); // Return 404 if not found.

            // Update the status of the claim.
            claim.status = status;
            _context.Update(claim);
            await _context.SaveChangesAsync();  // Save changes to the database.

            // Set a success message to display on the next page.
            TempData["SuccessMessage"] = $"Claim has been {status.ToLower()}!";
            return RedirectToAction("ViewClaims");  // Redirect to ViewClaims page.
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Retrieves and displays a list of all claims.
        public async Task<IActionResult> ViewClaims()
        {
            // Get all claims from the database.
            var claims = await _context.Claims.ToListAsync();
            return View(claims);  // Pass claims to the view.
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Creates a new claim with an optional file upload.
        [HttpPost]
        public async Task<IActionResult> Create(Claim claim, IFormFile? uploadedFile)
        {

            claim.Total = claim.HoursWorked * claim.HourlyRate;

            // Check if a file was uploaded and its size is greater than 0.
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                // Define the allowed file extensions.
                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();

                // Check if the uploaded file has a valid extension.
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("UploadedFilePath",
                        "Only PDF, DOCX, and XLSX files are allowed.");
                }
                else
                {
                    // Generate a unique file path to save the uploaded file.
                    var filePath = Path.Combine("wwwroot/uploads", Guid.NewGuid() + fileExtension);

                    try
                    {
                        // Save the uploaded file to the generated path.
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(stream);
                        }
                        // Save the file path to the claim.
                        claim.UploadedFilePath = "/uploads/" + Path.GetFileName(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Log the error and display an error message to the user.
                        ModelState.AddModelError("",
                            $"An error occurred while uploading the file: {ex.Message}");
                    }
                }
            }

            // Check if the model state is valid before saving the claim.
            if (ModelState.IsValid)
            {
                _context.Add(claim);               // Add the claim to the database.
                await _context.SaveChangesAsync();  // Save changes to the database.
                return RedirectToAction("Success"); // Redirect to success page.
            }

            // Log validation errors if any.
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                Console.WriteLine(string.Join(", ", errors));  // Output errors to console.
                return View(claim);  // Return the view with validation errors.
            }

            // Return the view if there were issues during claim creation.
            return View(claim);
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Displays the claim creation form.
        public IActionResult Create()
        {
            return View();  // Return the claim creation view.
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Displays the success page.
        public IActionResult Success()
        {
            return View();  
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Downloads a file based on the provided file path.
        public IActionResult DownloadFile(string filePath)
        {
            // Generate the full path to the file on the server.
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

            // Check if the file exists.
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("File not found.");  // Return 404 if file is missing.
            }

            // Read the file into a byte array and return it for download.
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/octet-stream", Path.GetFileName(fullPath));
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Displays the main page.
        public IActionResult Index()
        {
            return View();
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Displays a form to edit claims.
        public async Task<IActionResult> Edit()
        {
            // Get all claims from the database.
            var claims = await _context.Claims.ToListAsync();

            // If no claims are found, display an error message and redirect.
            if (!claims.Any())
            {
                TempData["ErrorMessage"] = "No claims found.";
                return RedirectToAction("ViewClaims");
            }

            return View(claims);  // Return the view with the list of claims.
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // Updates an existing claim with new information.
        [HttpPost]
        public async Task<IActionResult> Update(Claim claim)
        {
            // Ensure the model is valid before proceeding.
            if (!ModelState.IsValid) return View(claim);

            try
            {
                // Calculate the total based on hours worked and hourly rate.
                claim.Total = claim.HoursWorked * claim.HourlyRate;

                // Update the claim in the database.
                _context.Claims.Update(claim);
                await _context.SaveChangesAsync();  // Save changes to the database.

                TempData["SuccessMessage"] = "Claim updated successfully.";
                return RedirectToAction("Success");  // Redirect to success page.
            }
            catch (Exception ex)
            {
                // Handle exceptions and display error message to the user.
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(claim);
            }
        }

    }
}
//------------------------------------------...ooo000 END OF FILE 000ooo...------------------------------------------------------//