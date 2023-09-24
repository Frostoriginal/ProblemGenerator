using System.ComponentModel.DataAnnotations;

namespace ProblemGenerator;

public class ProblemSQL
{
    public ProblemSQL()
    {
        TimeElapsed = (int)(DateCreated - DateTime.Now).TotalDays * -1;
    }
    #region Basic properties
    [Key]
    public int Id { get; set; }
    
    
    [Required]
    [MinLength(2, ErrorMessage = "Must be at least 2 characters long")]
    [StringLength(30, ErrorMessage = "Be more concise, write additional information in detailed description, maximum field length is 20 characters")]
    public string What { get; set; } = "";
    
    
    [Required]
    [MinLength(2, ErrorMessage = "Must be at least 2 characters long")]
    [StringLength(20, ErrorMessage = "Be more concise, write additional information in detailed description, maximum field length is 20 characters")]
    public string Where { get; set; } = "";
    
    
    [StringLength(500, ErrorMessage = "Be more concise, maximum field length is 500 characters")]
    public string DetailedDescription { get; set; } = "";

    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime DateSolved { get; set; } = new DateTime(1753, 1, 1, 0, 0, 0);
    public bool IsSolved { get; set; } = false;
    public bool IsArchived { get; set; } = false;
    
    
    [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
    public int problemPriority { get; set; } = 1;
    public int TimeElapsed { get; set; } = 0;

    public string ImgPath { get; set; } = "";

    #endregion

    #region Recurrent task relevant
    public bool isRecurrentTask { get; set; } = false;
    public DateTime lastTimeAdded { get; set; } = new DateTime(1753, 1, 1, 0, 0, 0); //lowest value for SQL Server

    public bool repeatOnMonday { get; set; } = false;
    public bool repeatOnTuesday { get; set; } = false;
    public bool repeatOnWednesday { get; set; } = false;
    public bool repeatOnThursday { get; set; } = false;
    public bool repeatOnFriday { get; set; } = false;
    public bool repeatOnSaturday { get; set; } = false;
    public bool repeatOnSunday { get; set; } = false;

    public bool repeatedDaily { get; set; } = false;
    public bool repeatedWeekly { get; set; } = false;
    public bool repeatedMonthly { get; set; } = false;
    public bool repeatedYearly { get; set; } = false;

    public DateTime startFromDate { get; set; } = new DateTime(1753, 1, 1, 0, 0, 0);
    public DateTime repeatOnDate { get; set; } = new DateTime(1753, 1, 1, 0, 0, 0);
    public int daysBeforeRepetition { get; set; } = 0;
    public bool isPausedTask { get; set; } = false;

}
#endregion


