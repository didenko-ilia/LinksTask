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
using Newtonsoft.Json;
using System.Net;

namespace LinksTask.Controllers
{
  [Route("[controller]")]
  public class LinkController : Controller
  {
    private readonly LinkContext _context;

    //Length of a short link in use
    private int stringLength = 8;

    public LinkController(LinkContext context)
    {
      _context = context;
    }

    // GET: /link/
    // Get initial data from database based on cookies
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
          response += JsonConvert.SerializeObject(dbInfo);
        }
      }
      response += ']';
      return response;
    }

    //Get: /link/NewLink?longLink=value
    [HttpGet("/link/NewLink/{longLink?}")]
    public string NewLink([Bind("longlink")] string longLink)
    {
      if (longLink == null)
      {
        Response.Redirect("/");
        return null;
      }
      string shortLink;
      Link link = new Link();

      //Transform the link to 'https://' format
      if (!longLink.StartsWith("http://"))
      {
        if (longLink.StartsWith("https://"))
        {
          longLink = longLink.Remove(4, 1);
        }
        else
        {
          longLink = "http://" + longLink;
        }
      }
      if (!longLink.EndsWith("/")) 
        longLink += '/';

      //Check if the link is already in database, get its short link if it is,
      //create new entry otherwise
      if (_context.Links.Count(e => e.LongLink == longLink) != 0)
      {
        link = _context.Links.FirstOrDefault(e => e.LongLink == longLink);
        shortLink = link.ShortLink;
      }
      else
      {
        shortLink = StringGenerator.RandomString(stringLength);
        while (_context.Links.Count(e => e.ShortLink == shortLink) > 0)
          shortLink = StringGenerator.RandomString(stringLength);
        link.ShortLink = shortLink;
        link.LongLink = longLink;
        link.ViewCount = 0;
        _context.Links.Add(link);
        _context.SaveChanges();
        link.ViewCount = -1;
      }

      //Check if the short link is already mentioned in cookies, add if not
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
      cookieOptions.Expires = (DateTime.Now.AddYears(1));
      Response.Cookies.Append("shortlink", cookies, cookieOptions);

      string response = JsonConvert.SerializeObject(link);

      return response;
    }

    //Redirect from the short link
    //GET: /link/shortlink
    [HttpGet("/link/{shortLink}")]
    public void Index([Bind("shortLink")]string shortLink)
    {
      //Check if short link is valid
      if (shortLink == null || shortLink.Length != stringLength)
      {
        Response.StatusCode = 404;
        return ;
      }
      string longLink = null;
      Link linkToUpdate = null;
      //Get corresponding long link from the database and check its validity
      if (_context.Links.Count(e => e.ShortLink == shortLink) > 0)
      {
        linkToUpdate = _context.Links.FirstOrDefault(e => e.ShortLink == shortLink);
        longLink = linkToUpdate.LongLink;
      }
      if (longLink == null)
      {
        Response.StatusCode = 404;
        return;
      }
      //View count is increased in database
      if (linkToUpdate != null)
      {
        linkToUpdate.ViewCount++;
        _context.SaveChanges();
      }
      Response.Redirect(longLink, false);
      return;
    }
  }
}
