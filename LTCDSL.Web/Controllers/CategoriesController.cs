using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTCSDL.Web.Controllers
{
    using BLL;
    using DAL.Models;
    using Common.Req;
    using System.Collections.Generic;
    //using BLL.Req;
    using Common.Rsp;

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController()
        {
            _svc = new CategoriesSvc();
        }

        [HttpPost("get-by-id")]
        public IActionResult getCategoryById([FromBody]SimpleReq req)
        {
            var res = new SingleRsp();
            res = _svc.Read(req.Id);
            return Ok(res);
        }

        [HttpPost("get-all")]
        public IActionResult getAllCategories()
        {
            var res = new SingleRsp();
            res.Data = _svc.All;
            return Ok(res);
        }

        [HttpPost("get-cust-order-hist")]
        public IActionResult getCustOrderHist([FromBody]SimpleReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.getCustOrderList(req.Keyword);
            return Ok(res);
        }

        [HttpPost("get-cust-order-detail")]
        public IActionResult getCustOrderDetail([FromBody]SimpleReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.getCustOrderDetail(req.Id);
            return Ok(res);
        }

        [HttpPost("get-cust-order-hist-linq")]
        public IActionResult getCustOrderList_LinQt([FromBody]SimpleReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.getCustOrderList_LinQ(req.Keyword);
            return Ok(res);
        }

        [HttpPost("get-cust-order-details-linq")]
        public IActionResult getCustOrderDetail_LinQ([FromBody]SimpleReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.getCustOrderDetail_LinQ(req.Id);
            return Ok(res);
        }

        [HttpPost("get-doanhthutrongngay-LinQ")]
        public IActionResult getdoanhthutrongngay_LinQ([FromBody]doanhthuReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.getdoanhthutrongngay_LinQ(req.dateF);
            return Ok(res);
        }

        [HttpPost("get-doanhthutrongkhoangtg_LinQ")]
        public IActionResult getdoanhthutrongkhoangtg_LinQ([FromBody]doanhthuReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.getdoanhthutrongkhoangtg_LinQ(req.dateF, req.dateT);
            return Ok(res);
        }


        private readonly CategoriesSvc _svc;
    }
}