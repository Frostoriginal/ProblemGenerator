using ProblemGenerator.Controllers;
using ProblemGenerator.Data;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Security.Permissions;


namespace ProblemGenerator;


public class Problem
{
    #region Basic properties
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

    public DateTime DateCreated { get; set; }
    public DateTime DateSolved { get; set; }
    public bool IsSolved { get; set; }
    public bool IsArchived { get; set; }
    [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
    public int problemPriority { get; set; } = 1;
    public TimeSpan TimeElapsed => DateCreated - DateTime.Now;

    public string ImgPath { get; set; } = "";

    #endregion

    #region Recurrent task relevant
    public bool isRecurrentTask { get; set; }
    public DateTime lastTimeAdded { get; set; }

    public bool repeatOnMonday { get; set; }
    public bool repeatOnTuesday { get; set; }
    public bool repeatOnWednesday { get; set; }
    public bool repeatOnThursday { get; set; }
    public bool repeatOnFriday { get; set; }
    public bool repeatOnSaturday { get; set; }
    public bool repeatOnSunday { get; set; }

    public bool repeatedDaily { get; set; }
    public bool repeatedWeekly { get; set; }
    public bool repeatedMonthly { get; set; }
    public bool repeatedYearly { get; set; }

    public DateTime startFromDate { get; set; }
    public DateTime repeatOnDate { get; set; }
    public int daysBeforeRepetition { get; set; } = 0;
    public bool isPausedTask { get; set; } = false;
    
}
    #endregion

    