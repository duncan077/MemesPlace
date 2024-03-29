﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MemesAPI.Data
{
    public class Meme
    {
        public Meme()
        {
           
        }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(125)]
        [MinLength(5)]
        public string Name { get; set; } = "";
        [MaxLength(100)]
        public string? Description { get; set; }
        [Required]
        [Url]
        public string URLIMG { get; set; } = "";
        [Required]
        public string Format { get; set; } = "";
        
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsVideo { get; set; }
        public virtual ICollection<TagMeme>? Tags { get; set; }
        public virtual ICollection<MemeLike>? Likes { get; set; }
        [Required]
        public  string UserName { get; set; } = "";
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual MemeUser User { get; set; }
    }
}
