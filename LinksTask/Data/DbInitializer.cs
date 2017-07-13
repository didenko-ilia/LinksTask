using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinksTask.Data
{
  public class DbInitializer
  {
    public static void Initialize(LinkContext context)
    {
      context.Database.EnsureCreated();
    }
  }
}
