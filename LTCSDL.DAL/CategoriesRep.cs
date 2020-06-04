using LTCSDL.Common.DAL;
using System.Linq;

namespace LTCSDL.DAL
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection.Metadata.Ecma335;

    public class CategoriesRep : GenericRep<NorthwindContext, Categories>
    {
        #region -- Overrides --

        /// <summary>
        /// Read single object
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Return the object</returns>
        public override Categories Read(int id)
        {
            var res = All.FirstOrDefault(p => p.CategoryId == id);
            return res;
        }


        /// <summary>
        /// Remove and not restore
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Number of affect</returns>
        public int Remove(int id)
        {
            var m = base.All.First(i => i.CategoryId == id);
            m = base.Delete(m); //TODO
            return m.CategoryId;
        }

        #endregion


        #region -- Methods --

        /// <summary>
        /// Initialize
        /// </summary>
        /// 
        // thay vì làm ado.net có sằn trong sql thì làm bằng Lin_Q
        public object getCustOrderList_LinQ(string cusID)
        {
            var res = Context.Products
                .Join(Context.OrderDetails, a => a.ProductId, b => b.ProductId, (a, b) => new
                {
                    a.ProductId,
                    a.ProductName,
                    b.Quantity,
                    b.OrderId
                })
                .Join(Context.Orders, a => a.OrderId, b => b.OrderId, (a, b) => new
                {
                    a.ProductId,
                    a.ProductName,
                    a.OrderId,
                    a.Quantity,
                    b.CustomerId
                }).Where(x=>x.CustomerId == cusID).ToList();
            var data = res.GroupBy(x => x.ProductName).Select(x => new
            {
                ProductName = x.First().ProductName,
                Total = x.Sum(p => p.Quantity)
            });
            return data;
        }

        //làm LinQ

        public object getCustOrderDetail_LinQ(int odrID)
        {
            var res = Context.OrderDetails
                .Join(Context.Products, a=> a.ProductId, b=>b.ProductId, (a,b) => new 
                { 
                    a.Quantity,
                    a.UnitPrice,
                    a.Discount,
                    a.OrderId,
                    b.ProductId,
                    b.ProductName,
                    ExtendedPrice = a.Quantity * (1 - (decimal)a.Discount) * a.UnitPrice
                }).Where(x=>x.OrderId == odrID).ToList();
        return res;
    }

        // làm ado.net để lấy storeprocedure trong sql
        public object getCustOrderList(string cusID)
        {
            List<object> res = new List<object>();
            var cnn = (SqlConnection)(Context.Database.GetDbConnection());
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet(); //để hứng data
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "CustOrderHist";
                cmd.CommandType = CommandType.StoredProcedure; //để nhận biết nó là store
                cmd.Parameters.AddWithValue("@CustomerID", cusID);
                //lấy dữ liệu đổ vào dataset
                da.SelectCommand = cmd; //lấy
                da.Fill(ds); //đổ
                //kiểm tra có dữ liệu để đổ, sl table >0 và cái table đó có số dòng >0 thì mới trả về dữ liệu
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // duyệt từng dòng 1
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //tạo list đối tượng để trả về
                        var x = new
                        {
                            ProductName = row["ProductName"],
                        };
                        res.Add(x); //trả về đối tượng x

                    }
                }
            }
            catch (Exception e)
            { 
                res = null;
            }
            return res;
            // sau khi xong sẽ viết qua lớp BLL
        }

        //làm ado.net để lấy store procedure
        public object getCustOrdersDetail(int odrID)
        {
            List<object> res = new List<object>();
            var cnn = (SqlConnection)(Context.Database.GetDbConnection());
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet(); //để hứng data
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "CustOrdersDetail";
                cmd.CommandType = CommandType.StoredProcedure; //để nhận biết nó là store
                cmd.Parameters.AddWithValue("@OrderID", odrID);
                //lấy dữ liệu đổ vào dataset
                da.SelectCommand = cmd; //lấy
                da.Fill(ds); //đổ
                //kiểm tra có dữ liệu để đổ, sl table >0 và cái table đó có số dòng >0 thì mới trả về dữ liệu
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // duyệt từng dòng 1
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //tạo list đối tượng để trả về
                        var x = new
                        {
                            ProductName = row["ProductName"],
                            UnitPrice = row["UnitPrice"],
                            Quantity = row["Quantity"],
                            Discount = row["Discount"],
                            ExtendedPrice = row["ExtendedPrice"],
                        };
                        res.Add(x); //trả về đối tượng x

                    }
                }
            }
            catch (Exception e)
            {
                res = null;
            }
            return res;
            // sau khi xong sẽ viết qua lớp BLL
        }

        public object getdoanhthutrongngay_LinQ(DateTime dateF)
        {
            var res = Context.Orders
                .Join(Context.OrderDetails, a => a.OrderId, b => b.OrderId, (a, b) => new
                {
                    a.OrderId,
                    a.EmployeeId,
                    a.OrderDate,
                    b.Quantity,
                    b.Discount,
                    b.UnitPrice,
                    doanhthu = (b.Quantity * (decimal)(1 - b.Discount) * b.UnitPrice)
                })
                .Join(Context.Employees, a => a.EmployeeId, b => b.EmployeeId, (a, b) => new
                {
                    a.EmployeeId,
                    a.doanhthu,
                    a.OrderDate,
                    b.FirstName,
                    b.LastName
                }).Where(x => x.OrderDate == dateF).ToList();
            var data = res.GroupBy(x => x.EmployeeId).Select(x => new
            {
                ProductName = x.First().FirstName,
                Total = x.Sum(p => p.doanhthu)
            }); ;
            return data;
        }

        public object getdoanhthutrongkhoangtg_LinQ(DateTime dateF, DateTime dateT)
        {
            var res = Context.Orders
                .Join(Context.OrderDetails, a => a.OrderId, b => b.OrderId, (a, b) => new
                {
                    a.OrderId,
                    a.EmployeeId,
                    a.OrderDate,
                    b.Quantity,
                    b.Discount,
                    b.UnitPrice,
                    doanhthu = (b.Quantity * (decimal)(1 - b.Discount) * b.UnitPrice)
                })
                .Join(Context.Employees, a => a.EmployeeId, b => b.EmployeeId, (a, b) => new
                {
                    a.doanhthu,
                    a.OrderDate,
                    b.FirstName,
                    b.LastName
                }).Where(x => x.OrderDate == dateF && x.OrderDate==dateT).ToList();
            var data = res.GroupBy(x => x.FirstName).Select(x => new
            {
                ProductName = x.First().FirstName,
                Total = x.Sum(p => p.doanhthu)
            }); ;
            return data;
        }

        #endregion
    }
}
