using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using LinksTask.Models;
using LinksTask.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LinksTask.Controllers
{
  [Route("[controller]")]
  public class LinkController : Controller
  {
    private readonly LinkContext _context;

    private int stringLength = 8;

    public LinkController(LinkContext context)
    {
      _context = context;
    }

    // GET: /link/
    [HttpGet("/link")]
    public string Index()
    {

      string response = "[";
      string cookies = Request.Cookies["shortlink"];
      if (cookies != null)
      {
        string[] values = cookies.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        bool first = true;
        foreach (string shortLink in values)
        {
          if (!first) response += ',';
          else first = !first;
          Link dbInfo = _context.Links.FirstOrDefault(e => e.ShortLink == shortLink);
          response += "{\"shortLink\":\"" + shortLink + "\",\"longLink\":\"" + dbInfo.LongLink + "\",\"viewCount\":" + dbInfo.ViewCount + "}";
        }
      }
      response += ']';
      return response;
    }

    [HttpGet("/link/NewLink/{longLink?}")]
    public string NewLink([Bind("longlink")] string longLink)
    {
      string shortLink;
      Link link = new Link();
      int viewCount;
      if (!longLink.StartsWith("https://"))
      {
        if (longLink.StartsWith("http://"))
        {
          longLink = longLink.Insert(4, "s");
        }
        else
        {
          longLink = "https://" + longLink;
        }
      }
      int count = _context.Links.Count(e => e.LongLink == longLink);
      if (count != 0)
      {
        link = _context.Links.FirstOrDefault(e => e.LongLink == longLink);
        shortLink = link.ShortLink;
        viewCount = link.ViewCount;
      }
      else
      {
        shortLink = StringGenerator.RandomString(stringLength);
        while (_context.Links.Count(e => e.ShortLink == shortLink) > 0)
          shortLink = StringGenerator.RandomString(stringLength);
        viewCount = 0;
        link.ShortLink = shortLink;
        link.LongLink = longLink;
        link.ViewCount = 0;
        _context.Links.Add(link);
        _context.SaveChanges();
      }

      string cookies = Request.Cookies["shortlink"];
      bool exists = false;
      if (cookies != null)
      {
        string[] values = cookies.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string inCookies in values)
        {
          if (inCookies == shortLink)
            exists = true;
        }
        if (!exists)
          cookies += ',' + shortLink;
      }
      else
      {
        cookies = shortLink;
      }
      CookieOptions cookieOptions = new CookieOptions();
      cookieOptions.Expires = (DateTime.Now.AddDays(1));
      Response.Cookies.Append("shortlink", cookies, cookieOptions);

      string response = "{\"exists\":\"" + exists + "\",\"shortLink\":\"" + shortLink + "\",\"longLink\":\"" + longLink + "\",\"viewCount\":" + viewCount + "}";

      return response;
    }

    [HttpGet("/link/{shortLink}")]
    public void Index([Bind("shortLink")]string shortLink)
    {
      if (shortLink == null || shortLink.Length != stringLength)
      {
        NotFound();
        return;
      }
      var linkToUpdate = _context.Links.SingleOrDefault(e => e.ShortLink == shortLink);
      string longLink = linkToUpdate.LongLink;
      if (longLink == null)
      {
        NotFound();
        return;
      }
      if (linkToUpdate != null)
      {
        linkToUpdate.ViewCount++;
        _context.SaveChanges();
      }
      Response.Redirect(longLink, false);
    }
  }
}
