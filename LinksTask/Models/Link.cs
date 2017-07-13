using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LinksTask.Models
{
  public class Link
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    public string ShortLink { get; set; }
    public string LongLink { get; set; }
    public int ViewCount { get; set; }
  }
}
