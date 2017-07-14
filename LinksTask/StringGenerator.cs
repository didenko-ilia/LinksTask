using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinksTask
{
  public class StringGenerator
  {
    private static Random random = new Random();

    //Generating new short link of set length
    public static string RandomString(int length)
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
  }
}
