using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebappRadzen.Models.Blogging
{
  [Table("Posts", Schema = "dbo")]
  public partial class Post
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PostId
    {
      get;
      set;
    }
    public string Title
    {
      get;
      set;
    }
    public string Content
    {
      get;
      set;
    }
    public int? BlogId
    {
      get;
      set;
    }
    public Blog Blog { get; set; }
  }
}
