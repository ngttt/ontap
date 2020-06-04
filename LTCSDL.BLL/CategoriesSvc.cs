using Newtonsoft.Json;

using LTCSDL.BLL;
using LTCSDL.Common.Rsp;
using LTCSDL.Common.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LTCSDL.BLL
{
    using DAL;
    using DAL.Models;

    public class CategoriesSvc : GenericSvc<CategoriesRep, Categories>
    {
        #region -- Overrides --

        /// <summary>
        /// Read single object
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Return the object</returns>
        public override SingleRsp Read(int id)
        {
            var res = new SingleRsp();

            var m = _rep.Read(id);
            res.Data = m;

            return res;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="m">The model</param>
        /// <returns>Return the result</returns>
        public override SingleRsp Update(Categories m)
        {
            var res = new SingleRsp();

            var m1 = m.CategoryId > 0 ? _rep.Read(m.CategoryId) : _rep.Read(m.Description);
            if (m1 == null)
            {
                res.SetError("EZ103", "No data.");
            }
            else
            {
                res = base.Update(m);
                res.Data = m;
            }

            return res;
        }
        #endregion

        #region -- Methods --

        /// <summary>
        /// Initialize
        /// </summary>
        public CategoriesSvc() { }


        #endregion

        public object getCustOrderList(string cusID)
        {
            return _rep.getCustOrderList(cusID);
            // sau đó qua lớp Rsp
        }

        public object getCustOrderDetail(int odrID)
        {
            return _rep.getCustOrdersDetail(odrID);
        }
        // sau đó qua lớp Rsp

        public object getCustOrderList_LinQ(string custID)
        {
            return _rep.getCustOrderList_LinQ(custID);
        }
        // sau đó qua lớp Rsp

        public object getCustOrderDetail_LinQ(int odrID)
        {
            return _rep.getCustOrderDetail_LinQ(odrID);
        }
        // sau đó qua lớp Rsp

        public object getdoanhthutrongngay_LinQ(DateTime dateF)
        {
            return _rep.getdoanhthutrongngay_LinQ(dateF);
        }
        // sau đó qua lớp Rsp

        public object getdoanhthutrongkhoangtg_LinQ(DateTime dateF, DateTime dateT)
        {
            return _rep.getdoanhthutrongkhoangtg_LinQ(dateF, dateT);
        }
        // sau đó qua lớp Rsp
    }
}
