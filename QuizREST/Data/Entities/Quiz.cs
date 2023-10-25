using System.Collections.Generic;
using System;
using QuizREST.Auth.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizREST.Data.Entities;

public class Quiz : IUserOwnedRecourse
{
    public int Id { get; set; }
    [Column(TypeName = "text")]
    public string Name { get; set; }

    [Column(TypeName = "text")]
    public string Description { get; set; }

    public string Category { get; set; }
    public DateTime CreatedDate { get; set; }
    [Required]
    public string UserId { get; set; }
    public QuizRestUser User { get; set; }
}