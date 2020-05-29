using System;
using System.Collections.Generic;
using System.Text;
// khai báo các thư viện cần dùng
using LTCSDL.DAL;

namespace LTCSDL.DAL
{
    using System.Linq;
    using LTCSDL.Common.DAL;
    using LTCSDL.DAL.Models;
    using LTCSDL.Common.Rsp;
    //khai báo thư việc cần dùng

    // mình cần dùng public nên để public hàm
    public class ProductsRep : GenericRep<NorthwindContext, Products> //đối tượng generic đc match zs northwind và products
    {
        #region -- Overrides --
        public override Products Read(int id)
        {
            var res = All.FirstOrDefault(p => p.ProductId == id); // lấy tất cả thông tin từ bảng products từ id products
            return res;
        }
        public int Remove(int id)
        {
            var m = base.All.FirstOrDefault(i => i.ProductId == id);
            m = base.Delete(m);
            return m.ProductId;
        }
        #endregion

        // viết hàm create product
        #region --Methods--
        /// <summary>
        /// Intialize
        /// </summary>

        public SimpleRsp CreateProduct(Products pro)
        {
            var res = new SimpleRsp();
            using (var context = new NorthwindContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        var t = context.Products.Add(pro);
                        context.SaveChanges();
                        tran.Commit(); //  có j thay đổi sẽ tự động update
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        res.SetError(ex.StackTrace);
                    }
                }
            }
            return res;
        }

        // viết hàm Update
        public SimpleRsp UpdateProduct(Products pro)
        {
            var res = new SimpleRsp();
            using (var context = new NorthwindContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        var t = context.Products.Update(pro);
                        context.SaveChanges();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        res.SetError(ex.StackTrace);
                    }
                }
            }
            return res;
        }
        #endregion
    }
}