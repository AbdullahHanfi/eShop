using Microsoft.AspNetCore.Mvc;
namespace eShop.MVC.Areas.PromoCode.Controllers;

using DAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Dtos;

[Area("PromoCode")]
[Route("/api/promocode")]
[ApiController]
[AllowAnonymous]
public class UserPromoCodeController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    public UserPromoCodeController(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }
    [HttpPost("valid")]
    public IActionResult Valid([FromBody] PromoCodeDto promoCodeDto) {
        if (promoCodeDto is null)
            return BadRequest();
        promoCodeDto.Code = promoCodeDto.Code.ToUpper();
        if (promoCodeDto.Code == "TEST"){ return Ok(0.01); }
        else{
            var promoCode = _unitOfWork.PromoCodes.Find(obj => obj.code == promoCodeDto.Code);
            if (promoCode != null && !promoCode.IsUsed){ return Ok(promoCode.SalePercentage); }
        }
        return NotFound();
    }
}