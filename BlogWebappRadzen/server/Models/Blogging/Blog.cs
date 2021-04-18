using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebappRadzen.Models.Blogging
{
  [Table("Blogs", Schema = "dbo")]
  public partial class Blog
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BlogId
    {
      get;
      set;
    }

    public IEnumerable<Post> Posts { get; set; }
    public string Url
    {
      get;
      set;
    }
  }
}
