using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using acme.Models;

namespace acme.Controllers;

public class CatalogController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public CatalogController(ILogger<HomeController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Redeem(string prodid)
    {
        var response = new RedeemResponse();

        try
        {
          response = await _httpClient.GetFromJsonAsync<RedeemResponse>("redeem");
        } catch (HttpRequestException ex)
        {
          if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
          {
            return Redirect("/Account/Login");
          }
        }
        
        response.ProdId = prodid;
        return View(response);
    }
}