using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace WcfServiceOffer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        string strAPPVersion = System.Configuration.ConfigurationManager.AppSettings["APPVersion"].ToString();

        #region Basic

        private DbType ConvertNullableIntoDatatype(PropertyInfo PropInfoCol)
        {
            DbType dbt = new DbType();
            if (PropInfoCol.PropertyType.Name.Contains("Nullable"))
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("DateTime"))
                    {
                        dbt = DbType.DateTime;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Int32"))
                    {
                        dbt = DbType.Int32;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Decimal"))
                    {
                        dbt = DbType.Decimal;
                    }

                    else if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Byte;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("String"))
                    {
                        dbt = DbType.String;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Char") || PropInfoCol.PropertyType.ToString().Contains("char"))
                    {
                        dbt = DbType.String;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Time"))
                    {
                        dbt = DbType.Time;
                    }
                }
            }
            else
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Byte;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Char") || PropInfoCol.PropertyType.ToString().Contains("char"))
                    {
                        dbt = DbType.String;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Time"))
                    {
                        dbt = DbType.Time;
                    }
                    else
                    {
                        dbt = (DbType)Enum.Parse(typeof(DbType), PropInfoCol.PropertyType.Name);
                    }
                }
            }
            return dbt;
        }

        private Boolean ValidColumn(string str)
        {

            if (str.Length <= 2)
                str = str + "modified";
            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
            {
                return true;
            }
            return false;
        }

        private DbType ConvertNullable(PropertyInfo PropInfoCol)
        {
            DbType dbt = new DbType();
            if (PropInfoCol.PropertyType.Name.Contains("Nullable"))
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("DateTime"))
                    {
                        dbt = DbType.DateTime;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Int32"))
                    {
                        dbt = DbType.Int32;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("Decimal"))
                    {
                        dbt = DbType.Decimal;
                    }

                    else if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Binary;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("String"))
                    {
                        dbt = DbType.String;
                    }
                }
            }
            else
            {
                if (PropInfoCol.Name == "DbId" || PropInfoCol.Name == "DBId")
                {
                    dbt = DbType.Int32;
                }
                else
                {
                    if (PropInfoCol.PropertyType.ToString().Contains("Byte"))
                    {
                        dbt = DbType.Binary;
                    }
                    else if (PropInfoCol.PropertyType.ToString().Contains("bool"))
                    {
                        dbt = DbType.Boolean;
                    }
                    else
                    {
                        dbt = (DbType)Enum.Parse(typeof(DbType), PropInfoCol.PropertyType.Name);
                    }
                }
            }

            return dbt;
        }

        private static T ConvertTableToListNew<T>(DataRow dr) where T : new()
        {
            string pname = "";
            T objMaster = new T();
            try
            {

                foreach (PropertyInfo property in objMaster.GetType().GetProperties())
                {


                    if (dr.Table.Columns.Contains(property.Name.ToString()))
                    {
                        pname = property.Name;
                        if (dr[property.Name] == DBNull.Value)
                        {
                            property.SetValue(objMaster, null, null);
                        }
                        else if (property.PropertyType.ToString() == "System.Nullable`1[System.Decimal]")
                        {
                            Type Primitive = Nullable.GetUnderlyingType(property.PropertyType);
                            object Temp = Convert.ChangeType(dr[property.Name], Type.GetType(Primitive.FullName), CultureInfo.InvariantCulture);
                            property.SetValue(objMaster, Temp, null);

                        }
                        else
                        {
                            if (property.PropertyType.FullName.ToLower() == "system.char" || (Nullable.GetUnderlyingType(property.PropertyType) != null && Nullable.GetUnderlyingType(property.PropertyType).FullName.ToLower() == "system.char"))
                            {
                                property.SetValue(objMaster, Convert.ToChar(dr[property.Name]), null);
                            }
                            else
                                if (property.PropertyType.FullName.ToLower() == "system.int32" || (Nullable.GetUnderlyingType(property.PropertyType) != null && Nullable.GetUnderlyingType(property.PropertyType).FullName.ToLower() == "system.int32"))
                                {
                                    property.SetValue(objMaster, Convert.ToInt32(dr[property.Name]), null);
                                }
                                else
                                {
                                    property.SetValue(objMaster, dr[property.Name], null);
                                }

                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMaster;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


        #endregion

        #region Set connections

        private string StrSetConnection()
        {
            string MyString;
            //MyString = "Database=" + strDatabaseName + ";Server=" + strDBServerName + ";User id= " + strDBUserName + ";password= " + strDBUserPassword + ";Connect Timeout=8000000";
            //MyString = "Data Source=Admin-PC\\SQLEXPRESS;Initial Catalog=Offer;Integrated Security=true";

           MyString = "Data Source=sql5021.myasp.net;Initial Catalog=DB_A2B667_make14;User Id=DB_A2B667_make14_admin;Password=Makepri14;";
            //MyString = "Data Source=SQL5025.myASP.NET;Initial Catalog=DB_A28F3D_shubhamggupta;User Id=DB_A28F3D_shubhamggupta_admin;Password=45677123abhi123;";
           
            return MyString;
        }

        #endregion

        #region ChkVersion
        public dbmlStatus CheckVersion()
        {
            dbmlStatus objdbmlStatus = new dbmlStatus();

            objdbmlStatus.Status = strAPPVersion.ToString();
            objdbmlStatus.StatusId = 1;
            return objdbmlStatus;

        }

        #endregion

        #region USER
        public returndbmlUser  UserGetByMobileNoPassword(string MobileNo,string Password)
        {
            returndbmlUser objreturnResponse = new returndbmlUser();
            ObservableCollection<dbmlUser> List = new ObservableCollection<dbmlUser>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.UserGetByMobileNoPassword", MobileNo, Password);
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "User" });
                List = new ObservableCollection<dbmlUser>(from dRow in ds.Tables["User"].AsEnumerable()
                                                              select (ConvertTableToListNew<dbmlUser>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlUser = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlUser = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlUser UserInsert(returndbmlUser objUser)
        {
            returndbmlUser objreturndbmlUser = new returndbmlUser();
            dbmlUser objCategory1 = objUser.objdbmlUser.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[UserInsert]");
                foreach (PropertyInfo propInfocol in objCategory1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objCategory1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlUser.StatusId = 1;
                objreturndbmlUser.Status = "User account successfully Created!";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlUser.StatusId = 99;
                objreturndbmlUser.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlUser;
        }
        #endregion

        #region Shop
        public returndbmlShop ShopGetByVendorId(string VendorId)
        {
            returndbmlShop objreturnResponse = new returndbmlShop();
            ObservableCollection<dbmlShop> List = new ObservableCollection<dbmlShop>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ShopGetByVendorId", VendorId);
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Shop" });
                List = new ObservableCollection<dbmlShop>(from dRow in ds.Tables["Shop"].AsEnumerable()
                                                          select (ConvertTableToListNew<dbmlShop>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlShop = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlShop = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlShop ShopInsert(returndbmlShop objShop)
        {
            returndbmlShop objreturndbmlShop1 = new returndbmlShop();
           
            try
            {
                ////---Chk Shop---//
                bool chk = true;                
                objreturndbmlShop1 = ShopGetByVendorId(objShop.objdbmlShop.FirstOrDefault().VendorId.ToString());
                if (objreturndbmlShop1.objdbmlShop != null)
                {
                    if (objreturndbmlShop1.objdbmlShop.Count > 0)
                    {
                        chk = false;
                    }
                }
                //---End---//

                if (chk)
                {
                    returndbmlShop objreturndbmlShop = new returndbmlShop();
                    dbmlShop objCategory1 = objShop.objdbmlShop.FirstOrDefault();
                    DbCommand dbCommond = null;
                    DbTransaction trans;
                    DbConnection con;
                    Database db = new SqlDatabase(StrSetConnection());
                    con = db.CreateConnection();
                    con.Open();
                    trans = con.BeginTransaction();
                    try
                    {
                        dbCommond = db.GetStoredProcCommand("[Master].[ShopInsert]");
                        foreach (PropertyInfo propInfocol in objCategory1.GetType().GetProperties())
                        {
                            if (ValidColumn(propInfocol.Name))
                            {
                                DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                                db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objCategory1, null));
                            }
                        }
                        db.ExecuteNonQuery(dbCommond, trans);
                        trans.Commit();
                        objreturndbmlShop.StatusId = 1;
                        objreturndbmlShop.Status = "Record Save successfully";

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objreturndbmlShop.StatusId = 99;
                        objreturndbmlShop.Status = ex.Message.ToString() + ex.StackTrace.ToString();

                    }
                    finally { con.Close(); }
                    return objreturndbmlShop;
                }
                else
                {
                    objreturndbmlShop1.StatusId = 2;
                    objreturndbmlShop1.Status = "Shop is allready register";
                }
            }
            catch (Exception ex)
            {               
                objreturndbmlShop1.StatusId = 99;
                objreturndbmlShop1.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }

            return objreturndbmlShop1;
        }

        public returndbmlShop ShopUpdate(returndbmlShop objShop)
        {            
                returndbmlShop objreturndbmlShop = new returndbmlShop();
                dbmlShop objCategory1 = objShop.objdbmlShop.FirstOrDefault();
                DbCommand dbCommond = null;
                DbTransaction trans;
                DbConnection con;
                Database db = new SqlDatabase(StrSetConnection());
                con = db.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                try
                {
                    dbCommond = db.GetStoredProcCommand("[Master].[ShopUpdate]");
                    db.AddInParameter(dbCommond, "@ShopId", DbType.Int32, Convert.ToInt32(objShop.objdbmlShop.FirstOrDefault().ShopId));
                    db.AddInParameter(dbCommond, "@VendorId", DbType.Int32, Convert.ToInt32(objShop.objdbmlShop.FirstOrDefault().VendorId));
                    db.AddInParameter(dbCommond, "@ShopName", DbType.String, objShop.objdbmlShop.FirstOrDefault().ShopName);
                    db.AddInParameter(dbCommond, "@Address", DbType.String, objShop.objdbmlShop.FirstOrDefault().Address);
                    db.AddInParameter(dbCommond, "@Location", DbType.String, objShop.objdbmlShop.FirstOrDefault().Location);
                    db.AddInParameter(dbCommond, "@ContactNo", DbType.String, objShop.objdbmlShop.FirstOrDefault().ContactNo);
                    db.AddInParameter(dbCommond, "@Lat", DbType.String, objShop.objdbmlShop.FirstOrDefault().Lat);
                    db.AddInParameter(dbCommond, "@Long", DbType.String, objShop.objdbmlShop.FirstOrDefault().Long);
                    db.AddInParameter(dbCommond, "@WorkingDay", DbType.String, objShop.objdbmlShop.FirstOrDefault().WorkingDay);
                    db.AddInParameter(dbCommond, "@OpeningTime", DbType.String, objShop.objdbmlShop.FirstOrDefault().OpeningTime);
                    db.AddInParameter(dbCommond, "@ClosingTime", DbType.String, objShop.objdbmlShop.FirstOrDefault().ClosingTime);
                    db.AddInParameter(dbCommond, "@UserId", DbType.String, objShop.objdbmlShop.FirstOrDefault().UserId);
                    db.AddInParameter(dbCommond, "@UpdateDate", DbType.DateTime, objShop.objdbmlShop.FirstOrDefault().UpdateDate);
                    db.ExecuteNonQuery(dbCommond, trans);
                    objreturndbmlShop.StatusId = 1;
                    objreturndbmlShop.Status = "Successful";
                    trans.Commit();
                    con.Close();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    objreturndbmlShop.StatusId = 99;
                    objreturndbmlShop.Status = ex.Message.ToString() + ex.StackTrace.ToString();

                }
                finally
                {
                    con.Close();
                }
                return objreturndbmlShop;

            }

        public returndbmlShop ShopGetAll()
        {
            returndbmlShop objreturnResponse = new returndbmlShop();
            ObservableCollection<dbmlShop> List = new ObservableCollection<dbmlShop>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ShopGetAll");
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Vendor" });
                List = new ObservableCollection<dbmlShop>(from dRow in ds.Tables["Vendor"].AsEnumerable()
                                                          select (ConvertTableToListNew<dbmlShop>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlShop = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlShop = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
           
        

        #endregion

        #region Offer
        public returndbmlOffer OfferGetByCatId(string CatId)
        {
            returndbmlOffer objreturnResponse = new returndbmlOffer();
            ObservableCollection<dbmlOfferResult> List = new ObservableCollection<dbmlOfferResult>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferGetByCatId", Convert.ToInt32(CatId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Offer" });
                List = new ObservableCollection<dbmlOfferResult>(from dRow in ds.Tables["Offer"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlOfferResult>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offres not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public returndbmlOffer OfferGetBySubCatId(string SubCatId)
        {
            returndbmlOffer objreturnResponse = new returndbmlOffer();
            ObservableCollection<dbmlOfferResult> List = new ObservableCollection<dbmlOfferResult>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("[Master].[OfferGetbySubCatId]", Convert.ToInt32(SubCatId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Offer" });
                List = new ObservableCollection<dbmlOfferResult>(from dRow in ds.Tables["Offer"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlOfferResult>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offres not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public returndbmlOffer OfferInsert(returndbmlOffer objOffer)
        {
            returndbmlOffer objreturndbmlOffer = new returndbmlOffer();
            dbmlOffer objCategory1 = objOffer.objdbmlOffer.FirstOrDefault();

            //objCategory1.OfferSourceId = 1;

            if (objCategory1.Type == "Cashback")
            {
                objCategory1.Cashback = objCategory1.Discount;
                objCategory1.Discount = null;
            }

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[OfferInsert]");
                foreach (PropertyInfo propInfocol in objCategory1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objCategory1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlOffer.StatusId = 1;
                objreturndbmlOffer.Status = "Offer successfully Posted ! View after some time";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlOffer.StatusId = 99;
                objreturndbmlOffer.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlOffer;
        }
        public returndbmlOffer OfferUpdate(returndbmlOffer objOffer)
        {
            returndbmlOffer objreturndbmlOffer = new returndbmlOffer();
            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[OfferUpdate]");
                db.AddInParameter(dbCommond, "@OfferId", DbType.Int32, Convert.ToInt32(objOffer.objdbmlOffer.FirstOrDefault().OfferId));
                db.AddInParameter(dbCommond, "@VendorId", DbType.Int32, Convert.ToInt32(objOffer.objdbmlOffer.FirstOrDefault().VendorId));
                db.AddInParameter(dbCommond, "@ShopId", DbType.Int32, Convert.ToInt32(objOffer.objdbmlOffer.FirstOrDefault().ShopId));
                db.AddInParameter(dbCommond, "@CatId", DbType.Int32, Convert.ToInt32(objOffer.objdbmlOffer.FirstOrDefault().CatId));
                db.AddInParameter(dbCommond, "@Title", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().Title);
                db.AddInParameter(dbCommond, "@Discription", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().Discription);
                db.AddInParameter(dbCommond, "@Discount", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().Discount);
                db.AddInParameter(dbCommond, "@CashBack", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().Cashback);
                db.AddInParameter(dbCommond, "@Amount", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().Amount);
                db.AddInParameter(dbCommond, "@Image", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().Image);
                db.AddInParameter(dbCommond, "@FromTime", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().FromTime);
                db.AddInParameter(dbCommond, "@ToTime", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().ToTime);
                db.AddInParameter(dbCommond, "@UserId", DbType.String, objOffer.objdbmlOffer.FirstOrDefault().UserId);
                db.AddInParameter(dbCommond, "@UpdateDate", DbType.DateTime, objOffer.objdbmlOffer.FirstOrDefault().UpdateDate);
                db.ExecuteNonQuery(dbCommond, trans);
                objreturndbmlOffer.StatusId = 1;
                objreturndbmlOffer.Status = "Offer Successfully updated!";
                trans.Commit();
                con.Close();
                
            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlOffer.StatusId = 99;
                objreturndbmlOffer.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlOffer;
        }
        public returndbmlOfferDetailGetbyOfferIdResult OfferDetailGetbyOfferId(string OfferId)
        {
            returndbmlOfferDetailGetbyOfferIdResult objreturnResponse = new returndbmlOfferDetailGetbyOfferIdResult();
            ObservableCollection<OfferDetailGetbyOfferIdResult> List = new ObservableCollection<OfferDetailGetbyOfferIdResult>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferDetailGetbyOfferId", Convert.ToInt32(OfferId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "TopVendorOffer" });
                List = new ObservableCollection<OfferDetailGetbyOfferIdResult>(from dRow in ds.Tables["TopVendorOffer"].AsEnumerable()
                                                                               select (ConvertTableToListNew<OfferDetailGetbyOfferIdResult>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objOfferDetailGetbyOfferIdResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objOfferDetailGetbyOfferIdResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlOffer OfferGetByDiscount(string Discount)
        {
            returndbmlOffer objreturnResponse = new returndbmlOffer();
            ObservableCollection<dbmlOfferResult> List = new ObservableCollection<dbmlOfferResult>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferGetByDiscount", Discount);
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Offer" });
                List = new ObservableCollection<dbmlOfferResult>(from dRow in ds.Tables["Offer"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlOfferResult>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlOffer OfferGetByVendorId(string VendorId)
        {
            returndbmlOffer objreturnResponse = new returndbmlOffer();
            ObservableCollection<dbmlOfferResult> List = new ObservableCollection<dbmlOfferResult>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferGetByVendorId", Convert.ToInt32(VendorId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Offer" });
                List = new ObservableCollection<dbmlOfferResult>(from dRow in ds.Tables["Offer"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlOfferResult>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlOffer OfferGetByLocation(string Location)
        {
            returndbmlOffer objreturnResponse = new returndbmlOffer();
            ObservableCollection<dbmlOfferResult> List = new ObservableCollection<dbmlOfferResult>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferGetByLocation",Location);
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Offer" });
                List = new ObservableCollection<dbmlOfferResult>(from dRow in ds.Tables["Offer"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlOfferResult>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOfferResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offers not available for this Location";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlOffer OfferGetByShopId(string ShopId)
        {
            returndbmlOffer objreturnResponse = new returndbmlOffer();
            ObservableCollection<dbmlOffer> List = new ObservableCollection<dbmlOffer>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferGetByShopId", Convert.ToInt32(ShopId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Offer" });
                List = new ObservableCollection<dbmlOffer>(from dRow in ds.Tables["Offer"].AsEnumerable()
                                                           select (ConvertTableToListNew<dbmlOffer>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOffer = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOffer = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }


        #endregion

        #region Vendor
        public returndbmlVendor VendorGetAll()
        {
            returndbmlVendor objreturnResponse = new returndbmlVendor();
            ObservableCollection<dbmlVendor> List = new ObservableCollection<dbmlVendor>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.VendorGetAll");
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Vendor" });
                List = new ObservableCollection<dbmlVendor>(from dRow in ds.Tables["Vendor"].AsEnumerable()
                                                          select (ConvertTableToListNew<dbmlVendor>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlVendor = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlVendor = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlVendor VendorInsert(returndbmlVendor objVendor)
        {
            returndbmlVendor objreturndbmlVendor = new returndbmlVendor();
            dbmlVendor objCategory1 = objVendor.objdbmlVendor.FirstOrDefault();
            
            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[VendorInsert]");
                foreach (PropertyInfo propInfocol in objCategory1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objCategory1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlVendor.StatusId = 1;
                objreturndbmlVendor.Status = "Vendor account successfully Created!";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendor.StatusId = 99;
                objreturndbmlVendor.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendor;
        }
        public returndbmlVendor VendorGetByMobileNoPassword(string MobileNo, string Password,string NewFcmId)
        {
            returndbmlVendor objreturnResponse = new returndbmlVendor();
            returndbmlVendor objreturndbmlVendor1 = new returndbmlVendor();
            ObservableCollection<dbmlVendor> List = new ObservableCollection<dbmlVendor>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.VendorGetByMobileNoPassword", MobileNo, Password);
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Vendor" });
                List = new ObservableCollection<dbmlVendor>(from dRow in ds.Tables["Vendor"].AsEnumerable()
                                                            select (ConvertTableToListNew<dbmlVendor>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlVendor = List;
                    string VendorId=objreturnResponse.objdbmlVendor.FirstOrDefault().VendorId.ToString();
                    string OldFcmId=objreturnResponse.objdbmlVendor.FirstOrDefault().FcmId;
                    if (NewFcmId != OldFcmId)
                    {
                        objreturndbmlVendor1 = VendorFcmIdUpdate(VendorId, NewFcmId);
                        if (objreturndbmlVendor1.StatusId == 1)
                        {
                            objreturnResponse.StatusId = 1;
                            objreturnResponse.Status = "Successful";
                        }
                    }
                    else
                    {
                        objreturnResponse.StatusId = 1;
                        objreturnResponse.Status = "Successful";
                    }

                }
                else
                {
                    objreturnResponse.objdbmlVendor = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlVendor VendorDetailGetbyOfferId(string OfferId)
        {
            returndbmlVendor objreturnResponse = new returndbmlVendor();
            ObservableCollection<dbmlVendor> List = new ObservableCollection<dbmlVendor>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.VendorDetailGetbyOfferId",OfferId);
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Vendor" });
                List = new ObservableCollection<dbmlVendor>(from dRow in ds.Tables["Vendor"].AsEnumerable()
                                                            select (ConvertTableToListNew<dbmlVendor>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlVendor = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlVendor = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public returndbmlVendor VendorUpdate(returndbmlVendor objVendor)
        {
            returndbmlVendor objreturndbmlVendor = new returndbmlVendor();
            dbmlVendor objCategory1 = objVendor.objdbmlVendor.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                
                dbCommond = db.GetStoredProcCommand("[Master].[VendorUpdate]");
                db.AddInParameter(dbCommond, "@VendorId", DbType.Int32, Convert.ToInt32(objVendor.objdbmlVendor.FirstOrDefault().VendorId));
                db.AddInParameter(dbCommond, "@VendorName", DbType.String, objVendor.objdbmlVendor.FirstOrDefault().VendorName);
                db.AddInParameter(dbCommond, "@Address", DbType.String, objVendor.objdbmlVendor.FirstOrDefault().Address);                
               // db.AddInParameter(dbCommond, "@Status", DbType.Boolean, objVendor.objdbmlVendor.FirstOrDefault().Status);
                db.AddInParameter(dbCommond, "@UserId", DbType.String, objVendor.objdbmlVendor.FirstOrDefault().UserId);
                db.AddInParameter(dbCommond, "@UpdateDate", DbType.DateTime, objVendor.objdbmlVendor.FirstOrDefault().UpdateDate);
                db.ExecuteNonQuery(dbCommond, trans);
                objVendor.StatusId = 1;
                objVendor.Status = "Successful";
                trans.Commit();
                con.Close();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objVendor.StatusId = 99;
                objVendor.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objVendor;
        }

        public returndbmlVendor VendorFcmIdUpdate(string VendorId,string FcmId)
        {
            returndbmlVendor objreturndbmlVendor = new returndbmlVendor();
            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {

                dbCommond = db.GetStoredProcCommand("[Master].[VendorFcmIdUpdate]");
                db.AddInParameter(dbCommond, "@VendorId", DbType.Int32, Convert.ToInt32(VendorId));
                db.AddInParameter(dbCommond, "@FcmId", DbType.String,FcmId);
                db.AddInParameter(dbCommond, "@UpdateDate", DbType.DateTime, DateTime.Now);
                db.ExecuteNonQuery(dbCommond, trans);
                objreturndbmlVendor.StatusId = 1;
                objreturndbmlVendor.Status = "Successful";
                trans.Commit();
                con.Close();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendor.StatusId = 99;
                objreturndbmlVendor.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendor;
        }

        #endregion

        #region Category
        public returndbmlCategory CategoryGetAll()
        {
            returndbmlCategory objreturnResponse = new returndbmlCategory();
            ObservableCollection<dbmlCategory> List = new ObservableCollection<dbmlCategory>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.CategoryGetAll");
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Category" });
                List = new ObservableCollection<dbmlCategory>(from dRow in ds.Tables["Category"].AsEnumerable()
                                                            select (ConvertTableToListNew<dbmlCategory>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlCategory = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlCategory = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public returndbmlCategory CategoryInsert(returndbmlCategory objCategory)
        {
            returndbmlCategory objreturndbmlCategory = new returndbmlCategory();
            dbmlCategory objCategory1 = objCategory.objdbmlCategory.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[CategoryInsert]");
                foreach (PropertyInfo propInfocol in objCategory1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objCategory1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlCategory.StatusId = 1;
                objreturndbmlCategory.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlCategory.StatusId = 99;
                objreturndbmlCategory.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlCategory;
        }

        public returndbmlCategory CategoryUpdate(returndbmlCategory obj)
        {
            returndbmlCategory objreturndbmlShop = new returndbmlCategory();
            dbmlCategory objCategory1 = obj.objdbmlCategory.FirstOrDefault();
            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[CategoryUpdate]");
                db.AddInParameter(dbCommond, "@CatId", DbType.Int32, Convert.ToInt32(obj.objdbmlCategory.FirstOrDefault().CatId));
                db.AddInParameter(dbCommond, "@Image", DbType.String, Convert.ToInt32(obj.objdbmlCategory.FirstOrDefault().Image));
                db.AddInParameter(dbCommond, "@Name", DbType.String, obj.objdbmlCategory.FirstOrDefault().Name);
                db.AddInParameter(dbCommond, "@CatType", DbType.String, obj.objdbmlCategory.FirstOrDefault().CatType);
                db.AddInParameter(dbCommond, "@UserId", DbType.String, obj.objdbmlCategory.FirstOrDefault().UserId);
                db.AddInParameter(dbCommond, "@UpdateDate", DbType.DateTime, obj.objdbmlCategory.FirstOrDefault().UpdateDate);
                db.ExecuteNonQuery(dbCommond, trans);
                objreturndbmlShop.StatusId = 1;
                objreturndbmlShop.Status = "Successful";
                trans.Commit();
                con.Close();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlShop.StatusId = 99;
                objreturndbmlShop.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally
            {
                con.Close();
            }
            return objreturndbmlShop;

        }
        
        public returndbmlCategory CategoryGetByCatType(string CatType)
        {
            returndbmlCategory objreturnResponse = new returndbmlCategory();
            ObservableCollection<dbmlCategory> List = new ObservableCollection<dbmlCategory>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.CategoryGetByCatType",Convert.ToInt32(CatType));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Category" });
                List = new ObservableCollection<dbmlCategory>(from dRow in ds.Tables["Category"].AsEnumerable()
                                                              select (ConvertTableToListNew<dbmlCategory>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlCategory = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlCategory = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        #endregion

        #region SubCategory
        public returndbmlSubCategory SubCategoryGetByCatId(string CatId)
        {
            returndbmlSubCategory objreturnResponse = new returndbmlSubCategory();
            ObservableCollection<dbmlSubCategory> List = new ObservableCollection<dbmlSubCategory>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.SubCategoryGetByCatId",Convert.ToInt32(CatId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "SubCategory" });
                List = new ObservableCollection<dbmlSubCategory>(from dRow in ds.Tables["SubCategory"].AsEnumerable()
                                                              select (ConvertTableToListNew<dbmlSubCategory>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlSubCategory = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlSubCategory = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlSubCategory SubCategoryInsert(returndbmlSubCategory objSubCategory)
        {
            returndbmlSubCategory objreturndbmlSubCategory = new returndbmlSubCategory();
            dbmlSubCategory objSubCategory1 = objSubCategory.objdbmlSubCategory.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[SubCategoryInsert]");
                foreach (PropertyInfo propInfocol in objSubCategory1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objSubCategory1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlSubCategory.StatusId = 1;
                objreturndbmlSubCategory.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlSubCategory.StatusId = 99;
                objreturndbmlSubCategory.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlSubCategory;
        }             

        #endregion

        #region TopVendorOffer
        public returndbmlTopVendorOfferGetAllResult TopVendorOfferGetAll()
        {
            returndbmlTopVendorOfferGetAllResult objreturnResponse = new returndbmlTopVendorOfferGetAllResult();
            ObservableCollection<TopVendorOfferGetAllResult> List = new ObservableCollection<TopVendorOfferGetAllResult>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.TopVendorOfferGetAll");
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "TopVendorOffer" });
                List = new ObservableCollection<TopVendorOfferGetAllResult>(from dRow in ds.Tables["TopVendorOffer"].AsEnumerable()
                                                                            select (ConvertTableToListNew<TopVendorOfferGetAllResult>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlTopVendorOfferGetAllResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlTopVendorOfferGetAllResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlTopVendorOffer TopVendorOfferInsert(returndbmlTopVendorOffer objTopVendorOffer)
        {
            returndbmlTopVendorOffer objreturndbmlTopVendorOffer = new returndbmlTopVendorOffer();
            dbmlTopVendorOffer objTopVendorOffer1 = objTopVendorOffer.objdbmlTopVendorOffer.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[TopVendorOfferInsert]");
                foreach (PropertyInfo propInfocol in objTopVendorOffer1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objTopVendorOffer1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlTopVendorOffer.StatusId = 1;
                objreturndbmlTopVendorOffer.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlTopVendorOffer.StatusId = 99;
                objreturndbmlTopVendorOffer.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlTopVendorOffer;
        }
        #endregion

        #region Location
        public returndbmlLocation LocationGetAll()
        {
            returndbmlLocation objreturnResponse = new returndbmlLocation();
            ObservableCollection<dbmlLocation> List = new ObservableCollection<dbmlLocation>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.LocationGetAll");
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Location" });
                List = new ObservableCollection<dbmlLocation>(from dRow in ds.Tables["Location"].AsEnumerable()
                                                                            select (ConvertTableToListNew<dbmlLocation>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlLocation = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlLocation = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        #endregion

        #region Campaign

        public returndbmlCampaign CampaignGetbyOfferId(string OfferId)
        {
            returndbmlCampaign objreturnResponse = new returndbmlCampaign();
            ObservableCollection<dbmlCampaign> List = new ObservableCollection<dbmlCampaign>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.CampaignGetbyOfferId",Convert.ToInt32(OfferId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Campaign" });
                List = new ObservableCollection<dbmlCampaign>(from dRow in ds.Tables["Campaign"].AsEnumerable()
                                                              select (ConvertTableToListNew<dbmlCampaign>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlCampaign = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlCampaign = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }
        public returndbmlCampaign CampaignInsert(returndbmlCampaign objCampaign)
        {
            returndbmlCampaign objreturndbmlTopVendorOffer = new returndbmlCampaign();
            dbmlCampaign objCampaign1 = objCampaign.objdbmlCampaign.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[CampaignInsert]");
                foreach (PropertyInfo propInfocol in objCampaign1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objCampaign1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlTopVendorOffer.StatusId = 1;
                objreturndbmlTopVendorOffer.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlTopVendorOffer.StatusId = 99;
                objreturndbmlTopVendorOffer.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlTopVendorOffer;
        }

        #endregion

        #region RedeemOffer

        public returndbmlRedeemOffer RedeemOfferInsert(returndbmlRedeemOffer objRedeemOffer)
        {
            returndbmlRedeemOffer objreturndbmlRedeemOffer = new returndbmlRedeemOffer();
            dbmlRedeemOffer objRedeemOffer1 = objRedeemOffer.objdbmlRedeemOffer.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[RedeemOfferInsert]");
                foreach (PropertyInfo propInfocol in objRedeemOffer1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objRedeemOffer1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlRedeemOffer.StatusId = 1;
                objreturndbmlRedeemOffer.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlRedeemOffer.StatusId = 99;
                objreturndbmlRedeemOffer.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlRedeemOffer;
        }

        public returndbmlRedeemOffer RedeemOfferGetbyOfferIdUserId(string OfferId,string UserId)
        {
            returndbmlRedeemOffer objreturnResponse = new returndbmlRedeemOffer();
            ObservableCollection<dbmlRedeemOffer> List = new ObservableCollection<dbmlRedeemOffer>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.RedeemOfferGetbyOfferIdUserId", Convert.ToInt32(OfferId), Convert.ToInt32(UserId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Campaign" });
                List = new ObservableCollection<dbmlRedeemOffer>(from dRow in ds.Tables["Campaign"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlRedeemOffer>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlRedeemOffer = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlRedeemOffer = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public dbmlSendSMS SendOfferCodeByContactNoOfferId(string MobileNo, string OfferId,string UserId)
        {
            
            dbmlSendSMS obj = new dbmlSendSMS();
            returndbmlCampaign obj1 = new returndbmlCampaign();
            try
            {
                returndbmlVendor objvendor = new returndbmlVendor();
                objvendor = VendorDetailGetbyOfferId(OfferId);
                string VendorT = "";
                if (objvendor.objdbmlVendor.Count > 0)
                {
                   VendorT = objvendor.objdbmlVendor.FirstOrDefault().Type;
                }
                if (VendorT != "free1")
                {
                    returndbmlRedeemOffer objnew = new returndbmlRedeemOffer();
                    objnew = RedeemOfferGetbyOfferIdUserId(OfferId, UserId);
                    if (objnew.objdbmlRedeemOffer.Count <= 0)
                    {
                        obj1 = CampaignGetbyOfferId(OfferId);

                        string Msg = "";
                        if (obj1.objdbmlCampaign.Count > 0)
                        {

                            Msg = obj1.objdbmlCampaign.FirstOrDefault().OfferCode;
                            //string result = sendSMS(MobileNo, Msg);
                            obj.StatusId = 1;
                            obj.Status = "Sucess";
                        }
                        else
                        {
                            obj.StatusId = 2;
                            obj.Status = "Offer is not Available";
                        }

                        if (obj.StatusId == 1)
                        {
                            returndbmlRedeemOffer objredeemoffer = new returndbmlRedeemOffer();
                            dbmlRedeemOffer objdbml = new dbmlRedeemOffer();
                            objdbml.RedeemOfferId = 0;
                            objdbml.OfferId = Convert.ToInt32(OfferId);
                            objdbml.UserId = Convert.ToInt32(UserId);
                            objdbml.OfferCode = obj1.objdbmlCampaign.FirstOrDefault().OfferCode;
                            objdbml.MobileNo = MobileNo;
                            objdbml.UpdateDate = System.DateTime.Now;
                            objredeemoffer.objdbmlRedeemOffer = new ObservableCollection<dbmlRedeemOffer>();
                            objredeemoffer.objdbmlRedeemOffer.Add(objdbml);
                            objredeemoffer = RedeemOfferInsert(objredeemoffer);
                            if (objredeemoffer.StatusId == 1)
                            {
                                obj.StatusId = 1;
                                obj.Status = "Sucess";
                            }


                        }

                    }
                    else
                    {
                        obj.StatusId = 3;
                        obj.Status = "Offer allready Redeem One time";
                    }
                }
                else
                {
                    obj.StatusId = 4;
                    obj.Status = "This Offer is open you may avail it directly at shop.";
                }

               

            }
            catch (Exception ex)
            {
                obj.StatusId = 99;
                obj.Status = ex.Message;
            }
            return obj;
        }

        public string sendSMS(string MobileNo, string Msg)
        {
            String message = HttpUtility.UrlEncode(Msg);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "9v9+Bqyy8kI-wSilW18or78ER59pEKEAuHzdGauCHI"},
                {"numbers" , MobileNo},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;
            }
        }

        #endregion

        #region BannerImage
        public returndbmlBannerImage BannerImageGetAll()
        {
            returndbmlBannerImage objreturnResponse = new returndbmlBannerImage();
            ObservableCollection<dbmlBannerImage> List = new ObservableCollection<dbmlBannerImage>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.BannerImageGetAll");
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "BannerImage" });
                List = new ObservableCollection<dbmlBannerImage>(from dRow in ds.Tables["BannerImage"].AsEnumerable()
                                                              select (ConvertTableToListNew<dbmlBannerImage>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlBannerImage = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlBannerImage = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public returndbmlBannerImage BannerImageGetByCategoryId(string CategoryId)
        {
            returndbmlBannerImage objreturnResponse = new returndbmlBannerImage();
            ObservableCollection<dbmlBannerImage> List = new ObservableCollection<dbmlBannerImage>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.BannerImageGetByCategoryId",Convert.ToInt32(CategoryId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "BannerImage" });
                List = new ObservableCollection<dbmlBannerImage>(from dRow in ds.Tables["BannerImage"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlBannerImage>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlBannerImage = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlBannerImage = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        #endregion

        #region OfferSource
        public returndbmlOfferSource OfferSourceGetAll()
        {
            returndbmlOfferSource objreturnResponse = new returndbmlOfferSource();
            ObservableCollection<dbmlOfferSource> List = new ObservableCollection<dbmlOfferSource>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferSourceGetAll");
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "OfferSource" });
                List = new ObservableCollection<dbmlOfferSource>(from dRow in ds.Tables["OfferSource"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlOfferSource>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOfferSource = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOfferSource = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        #endregion

        #region OfferCount

        public returndbmlOfferCount OfferCount(string OfferId, string Count,string FcmId)
        {
            //FcmId = "dSLlkjoz6Vw:APA91bE0DCwj_L1yG9KhPvElYh0GGYOEyXoZoOhhc9OJk36kYTam6z1bJkW5J5UDT8aA4D27vC5l3cHoqX_KeKr8gaBw0tPXAjuBMiEr70pd_Fh0u4YIZgvFXhkxV6kEZldBvexwFPRu";
            returndbmlOfferCount obj = new returndbmlOfferCount();

            try
            {
                int count1 = Convert.ToInt32(Count);
                if (count1 == 0)
                {
                    obj.objdbmlOfferCount = new ObservableCollection<dbmlOfferCount>();
                    dbmlOfferCount obj1 = new dbmlOfferCount();
                    obj1.OfferCountId = 0;
                    obj1.OfferId = Convert.ToInt32(OfferId);
                    obj1.Count = count1 + 1;
                    obj.objdbmlOfferCount.Add(obj1);
                    obj = OfferCountInsert(obj);
                }
                else
                {
                    obj.objdbmlOfferCount = new ObservableCollection<dbmlOfferCount>();
                    dbmlOfferCount obj1 = new dbmlOfferCount();
                    obj1.OfferCountId = 0;
                    obj1.OfferId = Convert.ToInt32(OfferId);
                    obj1.Count = count1 + 1;
                    obj.objdbmlOfferCount.Add(obj1);
                    obj = OfferCountUpdate(obj);
                }

                if (obj.StatusId == 1)
                {
                    obj = OfferCountGetByOfferId(OfferId);
                    if (FcmId != "abhi")
                    {
                        SendNotificationFromFirebaseCloud(FcmId);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                obj.StatusId = 99;
            }
            return obj;
        
        }


        public returndbmlOfferCount OfferCountInsert(returndbmlOfferCount objOfferCount)
        {
            returndbmlOfferCount objreturndbmlOfferCount = new returndbmlOfferCount();
            dbmlOfferCount objOfferCount1 = objOfferCount.objdbmlOfferCount.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[OfferCountInsert]");
                foreach (PropertyInfo propInfocol in objOfferCount1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objOfferCount1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlOfferCount.StatusId = 1;
                objreturndbmlOfferCount.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlOfferCount.StatusId = 99;
                objreturndbmlOfferCount.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlOfferCount;
        }

        public returndbmlOfferCount OfferCountUpdate(returndbmlOfferCount objOfferCount)
        {
            returndbmlOfferCount objreturndbmlOfferCount = new returndbmlOfferCount();
            dbmlOfferCount objCategory1 = objOfferCount.objdbmlOfferCount.FirstOrDefault();
            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[OfferCountUpdate]");
                db.AddInParameter(dbCommond, "@OfferCountId", DbType.Int32, Convert.ToInt32(objOfferCount.objdbmlOfferCount.FirstOrDefault().OfferCountId));
                db.AddInParameter(dbCommond, "@OfferId", DbType.Int32, Convert.ToInt32(objOfferCount.objdbmlOfferCount.FirstOrDefault().OfferId));
                db.AddInParameter(dbCommond, "@Count", DbType.Int32, Convert.ToInt32(objOfferCount.objdbmlOfferCount.FirstOrDefault().Count));
               
                db.ExecuteNonQuery(dbCommond, trans);
                objreturndbmlOfferCount.StatusId = 1;
                objreturndbmlOfferCount.Status = "Successful";
                trans.Commit();
                con.Close();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlOfferCount.StatusId = 99;
                objreturndbmlOfferCount.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally
            {
                con.Close();
            }
            return objreturndbmlOfferCount;

        }

        public returndbmlOfferCount OfferCountGetByOfferId(string OfferId)
        {
            returndbmlOfferCount objreturnResponse = new returndbmlOfferCount();
            ObservableCollection<dbmlOfferCount> List = new ObservableCollection<dbmlOfferCount>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.OfferCountGetByOfferId",Convert.ToInt32(OfferId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "OfferCount" });
                List = new ObservableCollection<dbmlOfferCount>(from dRow in ds.Tables["OfferCount"].AsEnumerable()
                                                                 select (ConvertTableToListNew<dbmlOfferCount>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlOfferCount = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlOfferCount = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public string SendNotificationFromFirebaseCloud(string deviceId)
        {
            
            //-----------------Notification for single device-----------//

           //string ServerId = "AAAAcZiUj2Q:APA91bE0aWK0luaqKy82Ucl2STBaxin2dsatsnrhSLIBl9sJCL-UZ2a5HukNMw84gHWKyGskGrnD_An6YDqnvKO3peQ6X0etfLIxz8OzwBoGyQueMNSF1ajLLpajIbPa9JeAWFFWq2Fv";
            string ServerId = "AAAA9nDszoE:APA91bEh7pADNV9K-847fDXaRjMr_foL6UGIwnkw43JswEaXKtHhZUEggOVzQtNGxUoc_xVX0QSgtLMLb_xZWccCFlYIEGi-zABZeeYEBZmUCVkSsTUPff2Slw-oCqNnFld_2p1c2zWd";
            string senderId = "1058456522369";// "487891177316";
            //string deviceId = "cnom5aBctKs:APA91bHGy3yOq0OP0nMRX_432VxixuDoaCAn7ivBxZ_OY_VpDVhzmpg1cTBInlgDpXB_mGEg5E6quPkM9ErKh6_WtEkukez95Zm6POiiuCHJ-lgZDjP9wPXWN2by4b1QcIUoQHBTg-Dy";
            //string _topic = "hiii";
            String sResponseFromServer="";
            try
            {
                string url = @"https://fcm.googleapis.com/fcm/send";
                WebRequest tRequest = WebRequest.Create(url);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + "This is the message" + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
                var data = new
                {
                    to = deviceId,//"/topics/" + _topic,//deviceId,
                    notification = new
                    {
                        body = "Dear Vendor,Your Offer is like by Some User please update Offer Daily to more responce",
                        title = "BillaSale-Like your Offer"

                    }
                };
                string jsonss = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonss);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ServerId));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                sResponseFromServer = tReader.ReadToEnd();
                                Console.Write(sResponseFromServer);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
                {
                    var sss = ex.Message;
                    if (ex.InnerException != null)
                    {
                        var ss = ex.InnerException;
                    }
                }

            }

            return sResponseFromServer;

        }



        #endregion

        #region BuyNow

        public returndbmlBuyNow InsertBuyNow(returndbmlBuyNow objreturndbmlBuyNow)
        {
            returndbmlBuyNow objreturndbmlBuyNow1 = new returndbmlBuyNow();
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            System.Data.Common.DbCommand CmdGetParameter = null;
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {

                int intOrderHeaderId = 0;
                dbmlOrderHeader objdbmlOrderHeader = objreturndbmlBuyNow.objdbmlOfferHeader.FirstOrDefault();
                CmdGetParameter = db.GetStoredProcCommand("Master.OrderHeaderInsert");
                CmdGetParameter.CommandTimeout = 8000000;

                //-----------OrderHeader----------//

                foreach (PropertyInfo PropInfoCol in objdbmlOrderHeader.GetType().GetProperties())
                {                    
                        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(objdbmlOrderHeader, null));
                    
                }

                db.AddOutParameter(CmdGetParameter, "OrderHeaderIdOut", DbType.Int32, 0);
                db.ExecuteNonQuery(CmdGetParameter, trans);
                intOrderHeaderId = (int)db.GetParameterValue(CmdGetParameter, "@OrderHeaderIdOut");

                //-----------OrderDetail----------//

                foreach (var ODetail in objreturndbmlBuyNow.objdbmlOfferDetail)
                {
                    CmdGetParameter = db.GetStoredProcCommand("Master.OrderDetailInsert");
                    CmdGetParameter.CommandTimeout = 8000000;

                    ODetail.OrderHeaderId = intOrderHeaderId;
                    foreach (PropertyInfo PropInfoCol in ODetail.GetType().GetProperties())
                    {
                            DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(ODetail, null));

                       
                    }
                    db.ExecuteNonQuery(CmdGetParameter, trans);
                }


                ////-----------OrderSupplimentary----------//

                //foreach (var ODetail in objreturndbmlBuyNow.objdbmlOfferSupplimentary)
                //{
                //    CmdGetParameter = db.GetStoredProcCommand("Master.OrderSupplimentaryInsert");
                //    CmdGetParameter.CommandTimeout = 8000000;

                //    ODetail.OrderHeaderId = intOrderHeaderId;
                //    foreach (PropertyInfo PropInfoCol in ODetail.GetType().GetProperties())
                //    {
                //        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                //        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(ODetail, null));


                //    }
                //    db.ExecuteNonQuery(CmdGetParameter, trans);
                //}

                //----------Gatway Trans------------//

                //foreach (var GatWayTrans in objreturndbmlBuyNow.objdbmlGatwayTrans)
                //{
                //    CmdGetParameter = db.GetStoredProcCommand("Master.GatwayTransInsert");
                //    CmdGetParameter.CommandTimeout = 8000000;

                //    GatWayTrans.OrderHeaderId = intOrderHeaderId;
                //    foreach (PropertyInfo PropInfoCol in GatWayTrans.GetType().GetProperties())
                //    {
                //        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                //        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(GatWayTrans, null));

                //    }
                //    db.ExecuteNonQuery(CmdGetParameter, trans);
                //}                
               
                //----------ShippingAddress------------//

                foreach (var SAddress in objreturndbmlBuyNow.objdbmlShippingAddress)
                {
                    CmdGetParameter = db.GetStoredProcCommand("Master.ShippingAddressInsert");
                    CmdGetParameter.CommandTimeout = 8000000;

                    SAddress.OrderHeaderId = intOrderHeaderId;
                    foreach (PropertyInfo PropInfoCol in SAddress.GetType().GetProperties())
                    {
                        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(SAddress, null));

                    }
                    db.ExecuteNonQuery(CmdGetParameter, trans);
                }

                              
                    
                objreturndbmlBuyNow.objdbmlOfferHeader.FirstOrDefault().OrderHeaderId = intOrderHeaderId;                
                  
              
               
                trans.Commit();
                objreturndbmlBuyNow1 = objreturndbmlBuyNow;
                objreturndbmlBuyNow1.StatusId = 1;
               // objreturndbmlBuyNow1.Status = Convert.ToString(intOrderHeaderId);


            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlBuyNow1.Status = ex.Message + ", " + ex.StackTrace.ToString();
                objreturndbmlBuyNow1.StatusId = 99;

            }
            finally
            {
                con.Close();

            }

            return objreturndbmlBuyNow1;

        }



        #endregion

        #region Product

        public returndbmlProduct ProductInsert(returndbmlProduct objProduct)
        {
            returndbmlProduct objreturndbmlProduct = new returndbmlProduct();
            dbmlProduct objProduct1 = objProduct.objdbmlProduct.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[ProductInsert]");
                foreach (PropertyInfo propInfocol in objProduct1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objProduct1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlProduct.StatusId = 1;
                objreturndbmlProduct.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlProduct.StatusId = 99;
                objreturndbmlProduct.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlProduct;
        }

        public returndbmlProduct ProductGetByCatId(string CatId)
        {
            returndbmlProduct objreturnResponse = new returndbmlProduct();
            ObservableCollection<dbmlProduct> List = new ObservableCollection<dbmlProduct>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ProductGetByCatId", Convert.ToInt32(CatId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Product" });
                List = new ObservableCollection<dbmlProduct>(from dRow in ds.Tables["Product"].AsEnumerable()
                                                             select (ConvertTableToListNew<dbmlProduct>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlProduct = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlProduct = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offres not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public returndbmlProduct ProductGetBySubCatId(string SubCatId)
        {
            returndbmlProduct objreturnResponse = new returndbmlProduct();
            ObservableCollection<dbmlProduct> List = new ObservableCollection<dbmlProduct>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ProductGetBySubCatId", Convert.ToInt32(SubCatId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Product" });
                List = new ObservableCollection<dbmlProduct>(from dRow in ds.Tables["Product"].AsEnumerable()
                                                             select (ConvertTableToListNew<dbmlProduct>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlProduct = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlProduct = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offres not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        public returndbmlProduct ProductGetByProductName(string ProductName)
        {
            returndbmlProduct objreturnResponse = new returndbmlProduct();
            ObservableCollection<dbmlProduct> List = new ObservableCollection<dbmlProduct>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ProductGetbyProductName", ProductName);
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Product" });
                List = new ObservableCollection<dbmlProduct>(from dRow in ds.Tables["Product"].AsEnumerable()
                                                             select (ConvertTableToListNew<dbmlProduct>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlProduct = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlProduct = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Product not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        
        public returndbmlProductGetbyCatIdResult ProductDetailGetByProductId(string ProductId)
        {
            returndbmlProductGetbyCatIdResult objreturnResponse = new returndbmlProductGetbyCatIdResult();
            ObservableCollection<ProductDetailGetbyProductIdResult> List = new ObservableCollection<ProductDetailGetbyProductIdResult>();
            ObservableCollection<dbmlProductTag> ProductTagList = new ObservableCollection<dbmlProductTag>();

            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ProductDetailGetByProductId", Convert.ToInt32(ProductId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Product" });
                List = new ObservableCollection<ProductDetailGetbyProductIdResult>(from dRow in ds.Tables["Product"].AsEnumerable()
                                                                                   select (ConvertTableToListNew<ProductDetailGetbyProductIdResult>(dRow)));

                if (List.Count > 0)
                {
                    ds = new DataSet();
                    db = new SqlDatabase(StrSetConnection());
                    CmdGetParameter = null;
                    CmdGetParameter = db.GetStoredProcCommand("Master.ProductTagGetByProductId", Convert.ToInt32(ProductId));
                    db.LoadDataSet(CmdGetParameter, ds, new string[] { "ProductTag" });
                    ProductTagList = new ObservableCollection<dbmlProductTag>(from dRow in ds.Tables["ProductTag"].AsEnumerable()
                                                                    select (ConvertTableToListNew<dbmlProductTag>(dRow)));
                    string Tag = "";
                    if (ProductTagList.Count > 0)
                    {                       
                        for (int i = 0; i < ProductTagList.Count;i++)
                        {
                            if (i == 0)
                            {
                                Tag = ProductTagList[i].Tag;
                            }
                            else
                            {                               

                                Tag = Tag + "," + ProductTagList[i].Tag;
                            }
                        }
                    }

                    List.FirstOrDefault().ProductTag = Tag;
                    objreturnResponse.objProductDetailGetbyProductIdResult = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objProductDetailGetbyProductIdResult = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Product not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }



        #endregion

        #region VendorOtherInfo

        public returndbmlVendorOtherInfo VendorOtherInfoInsert(returndbmlVendorOtherInfo objVendorInfo)
        {
            returndbmlVendorOtherInfo objreturndbmlVendorInfo = new returndbmlVendorOtherInfo();
            dbmlVendorOtherInfo objVendorOtherInfo1 = objVendorInfo.objdbmlVendorOtherInfo.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[VendorOtherInfoInsert]");
                foreach (PropertyInfo propInfocol in objVendorOtherInfo1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objVendorOtherInfo1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlVendorInfo.StatusId = 1;
                objreturndbmlVendorInfo.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendorInfo.StatusId = 99;
                objreturndbmlVendorInfo.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendorInfo;
        }

        #endregion

        #region ProductImage

        public returndbmlProductImage ProductImageInsert(returndbmlProductImage objVendorInfo)
        {
            returndbmlProductImage objreturndbmlVendorInfo = new returndbmlProductImage();
            dbmlProductImage objProductImage1 = objVendorInfo.objdbmlProductImage.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[ProductImageInsert]");
                foreach (PropertyInfo propInfocol in objProductImage1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objProductImage1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlVendorInfo.StatusId = 1;
                objreturndbmlVendorInfo.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendorInfo.StatusId = 99;
                objreturndbmlVendorInfo.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendorInfo;
        }

        public returndbmlProductImage ProductImageGetByProductId(string ProductId)
        {
            returndbmlProductImage objreturnResponse = new returndbmlProductImage();
            ObservableCollection<dbmlProductImage> List = new ObservableCollection<dbmlProductImage>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ProductImageGetByProductId", Convert.ToInt32(ProductId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Product" });
                List = new ObservableCollection<dbmlProductImage>(from dRow in ds.Tables["Product"].AsEnumerable()
                                                                  select (ConvertTableToListNew<dbmlProductImage>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlProductImage = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlProductImage = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offres not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }


        #endregion

        #region Specification

        public returndbmlSpecification SpecificationInsert(returndbmlSpecification objVendorInfo)
        {
            returndbmlSpecification objreturndbmlVendorInfo = new returndbmlSpecification();
            dbmlSpecification objSpecification1 = objVendorInfo.objdbmlSpecification.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[SpecificationInsert]");
                foreach (PropertyInfo propInfocol in objSpecification1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objSpecification1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlVendorInfo.StatusId = 1;
                objreturndbmlVendorInfo.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendorInfo.StatusId = 99;
                objreturndbmlVendorInfo.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendorInfo;
        }

        #endregion

        #region ProductSpecification

        public returndbmlProductSpecification ProductSpecificationInsert(returndbmlProductSpecification objVendorInfo)
        {
            returndbmlProductSpecification objreturndbmlVendorInfo = new returndbmlProductSpecification();
            dbmlProductSpecification objProductSpecification1 = objVendorInfo.objdbmlProductSpecification.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[ProductSpecificationInsert]");
                foreach (PropertyInfo propInfocol in objProductSpecification1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objProductSpecification1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlVendorInfo.StatusId = 1;
                objreturndbmlVendorInfo.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendorInfo.StatusId = 99;
                objreturndbmlVendorInfo.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendorInfo;
        }

        public returndbmlProductSpecification ProductSpecificationGetByProductId(string ProductId)
        {
            returndbmlProductSpecification objreturnResponse = new returndbmlProductSpecification();
            ObservableCollection<dbmlProductSpecification> List = new ObservableCollection<dbmlProductSpecification>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ProductSpecificationGetByProductId", Convert.ToInt32(ProductId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Product" });
                List = new ObservableCollection<dbmlProductSpecification>(from dRow in ds.Tables["Product"].AsEnumerable()
                                                                          select (ConvertTableToListNew<dbmlProductSpecification>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlProductSpecification = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlProductSpecification = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offres not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        #endregion

        #region SpecificationCatIdLink

        public returndbmlSpecificationCatIdLink SpecificationCatIdLinkInsert(returndbmlSpecificationCatIdLink objVendorInfo)
        {
            returndbmlSpecificationCatIdLink objreturndbmlVendorInfo = new returndbmlSpecificationCatIdLink();
            dbmlSpecificationCatIdLink objSpecificationCatIdLink1 = objVendorInfo.objdbmlSpecificationCatIdLink.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[SpecificationCatIdLinkInsert]");
                foreach (PropertyInfo propInfocol in objSpecificationCatIdLink1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objSpecificationCatIdLink1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlVendorInfo.StatusId = 1;
                objreturndbmlVendorInfo.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendorInfo.StatusId = 99;
                objreturndbmlVendorInfo.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendorInfo;
        }

        #endregion

        #region ProductTag

        public returndbmlProductTag ProductTagInsert(returndbmlProductTag objVendorInfo)
        {
            returndbmlProductTag objreturndbmlVendorInfo = new returndbmlProductTag();
            dbmlProductTag objProductTag1 = objVendorInfo.objdbmlProductTag.FirstOrDefault();

            DbCommand dbCommond = null;
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            try
            {
                dbCommond = db.GetStoredProcCommand("[Master].[ProductTagInsert]");
                foreach (PropertyInfo propInfocol in objProductTag1.GetType().GetProperties())
                {
                    if (ValidColumn(propInfocol.Name))
                    {
                        DbType dbt = ConvertNullableIntoDatatype(propInfocol);
                        db.AddInParameter(dbCommond, propInfocol.Name.ToString(), dbt, propInfocol.GetValue(objProductTag1, null));
                    }
                }
                db.ExecuteNonQuery(dbCommond, trans);
                trans.Commit();
                objreturndbmlVendorInfo.StatusId = 1;
                objreturndbmlVendorInfo.Status = "Record Save successfully";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlVendorInfo.StatusId = 99;
                objreturndbmlVendorInfo.Status = ex.Message.ToString() + ex.StackTrace.ToString();

            }
            finally { con.Close(); }
            return objreturndbmlVendorInfo;
        }

        public returndbmlProductTag ProductTagGetbyProductId(string ProductId)
        {
            returndbmlProductTag objreturnResponse = new returndbmlProductTag();
            ObservableCollection<dbmlProductTag> List = new ObservableCollection<dbmlProductTag>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ProductTagGetByProductId", Convert.ToInt32(ProductId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "Product" });
                List = new ObservableCollection<dbmlProductTag>(from dRow in ds.Tables["Product"].AsEnumerable()
                                                             select (ConvertTableToListNew<dbmlProductTag>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlProductTag = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlProductTag = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Sry! Offres not available for this Category";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }



        #endregion

        #region ShippingAddress


        public returndbmlShippingAddress ShippingAddressGetByUserId(string UserId)
        {
            returndbmlShippingAddress objreturnResponse = new returndbmlShippingAddress();
            ObservableCollection<dbmlShippingAddress> List = new ObservableCollection<dbmlShippingAddress>();
            try
            {
                DataSet ds = new DataSet();
                Database db = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;
                CmdGetParameter = db.GetStoredProcCommand("Master.ShippingAddressGetByUserId", Convert.ToInt32(UserId));
                db.LoadDataSet(CmdGetParameter, ds, new string[] { "User" });
                List = new ObservableCollection<dbmlShippingAddress>(from dRow in ds.Tables["User"].AsEnumerable()
                                                          select (ConvertTableToListNew<dbmlShippingAddress>(dRow)));

                if (List.Count > 0)
                {
                    objreturnResponse.objdbmlShippingAddress = List;
                    objreturnResponse.StatusId = 1;
                    objreturnResponse.Status = "Successful";
                }
                else
                {
                    objreturnResponse.objdbmlShippingAddress = List;
                    objreturnResponse.StatusId = 2;
                    objreturnResponse.Status = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                objreturnResponse.StatusId = 99;
                objreturnResponse.Status = ex.ToString();
            }
            return objreturnResponse;


        }

        #endregion

        #region Gateway Trans
        public returndbmlGatewayTrans GatewayTransInsert(returndbmlGatewayTrans objreturndbmlGatewayTrans)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            returndbmlGatewayTrans objreturndbmlGatewayTransTemp = new returndbmlGatewayTrans();

            //returndbmlGatewayTrans objreturndbmlGatewayTrans = new returndbmlGatewayTrans();   
            //objreturndbmlGatewayTrans.objdbmlGatewayTrans = new ObservableCollection<dbmlGatewayTrans>();
            //dbmlGatewayTrans objGT = new dbmlGatewayTrans();
            //objGT.GatewayTransId = 0;
            //objGT.OrderHeaderId = 0;
            //objGT.OrderNo = "120";
            //objGT.TransDate = System.DateTime.Today;
            //objGT.PaymentReceiveType = "1";
            //objGT.UserId = 1;
            //objGT.Amount = Convert.ToDecimal(10);
            //objGT.StatusId = 0;
            //objGT.StatusDescription = "";
            //objGT.NetworkIP = "";
            //objGT.UpdateId = 1;
            //objGT.CreateDate = System.DateTime.Today;
            //objGT.UpdateDate = System.DateTime.Now;

            //objreturndbmlGatewayTrans.objdbmlGatewayTrans.Add(objGT);

            try
            {
                string statusMsr = "";
                string strMsg = "";
                DataSet dsCustomer = new DataSet();
                Database dbOrder = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;



                con = dbOrder.CreateConnection();
                con.Open();


                trans = con.BeginTransaction();
                int intOutId = 0;

                dbmlGatwayTrans objdbmlGatewayTrans = objreturndbmlGatewayTrans.objdbmlGatewayTrans.FirstOrDefault();

                CmdGetParameter = dbOrder.GetStoredProcCommand("[Master].[GatwayTransInsert]");
                CmdGetParameter.CommandTimeout = 8000000;

                foreach (PropertyInfo PropInfoCol in objdbmlGatewayTrans.GetType().GetProperties())
                {
                    string str = PropInfoCol.Name;
                    if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
                    {
                        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                        dbOrder.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(objdbmlGatewayTrans, null));
                    }
                }

                dbOrder.AddOutParameter(CmdGetParameter, "IDOut", DbType.Int32, 0);
                dbOrder.ExecuteNonQuery(CmdGetParameter, trans);
                intOutId = (int)dbOrder.GetParameterValue(CmdGetParameter, "@IDOut");
                objreturndbmlGatewayTransTemp.Status = Convert.ToString(intOutId);
                objreturndbmlGatewayTransTemp.StatusId = 1;
                trans.Commit();


                if (intOutId != 0)
                {
                    //PayuMoneyResponceStatus objResult = new PayuMoneyResponceStatus();
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    //string URI = "https://www.payumoney.com/payment/payment/chkMerchantTxnStatus?";
                    //string myParameters = "merchantKey=OGBPmOqZ&merchantTransactionIds=" + intOutId.ToString();
                    //WebClient wc = new WebClient();
                    //wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    //wc.Headers.Add("Authorization", "N7FamtF+G2NOhh5vw5brFQC4twPr13auLcBRQJhK8+Y=");
                    //string str1 = wc.UploadString(URI, myParameters);
                    //var jss = new JavaScriptSerializer();
                    //objResult = jss.Deserialize<PayuMoneyResponceStatus>(str1);
                    //if (objResult.result != null)
                    //{
                    //    objreturndbmlGatewayTransTemp.StatusId = 2;
                    //    objreturndbmlGatewayTransTemp.Status = Convert.ToString(intOutId);
                    //}
                    //else
                    //{
                    //    objreturndbmlGatewayTransTemp.StatusId = 1;
                    //    objreturndbmlGatewayTransTemp.Status = Convert.ToString(intOutId);

                    //}
                }




            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlGatewayTransTemp.Status = ex.Message + ", " + ex.StackTrace.ToString();
                objreturndbmlGatewayTransTemp.StatusId = 99;
            }
            finally
            {
                con.Close();

            }
            return objreturndbmlGatewayTransTemp;
        }


        public returndbmlGatewayTrans GatewayTransUpdate(string GatewayTransId, string StatusId, string StatusDescription, string RefGateway, string Amount)
        {
            returndbmlGatewayTrans objreturndbmlGatewayTransTemp = new returndbmlGatewayTrans();
            DbConnection con = null;
            DbTransaction trans = null;


            try
            {
                string statusMsr = "";
                string strMsg = "";
                DataSet dsCustomer = new DataSet();
                Database dbOrder = new SqlDatabase(StrSetConnection());
                System.Data.Common.DbCommand CmdGetParameter = null;



                con = dbOrder.CreateConnection();
                con.Open();

                string[] Obj = RefGateway.Split('_');
                trans = con.BeginTransaction();
                int intOutId = 0;

                if (Obj[0].ToString() == GatewayTransId.ToString())
                {
                    returndbmlGatewayTrans obj = new returndbmlGatewayTrans();
                    PayuMoneyResponceStatus objResult = new PayuMoneyResponceStatus();
                    //List<result> ObjList = new List<result>();

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    string URI = "https://www.payumoney.com/payment/payment/chkMerchantTxnStatus?";
                    string myParameters = "merchantKey=OGBPmOqZ&merchantTransactionIds=" + GatewayTransId.ToString();
                    WebClient wc = new WebClient();
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                    wc.Headers.Add("Authorization", "N7FamtF+G2NOhh5vw5brFQC4twPr13auLcBRQJhK8+Y=");

                    string str = wc.UploadString(URI, myParameters);

                    var jss = new JavaScriptSerializer();

                    objResult = jss.Deserialize<PayuMoneyResponceStatus>(str);

                    if (objResult.result != null)
                    {

                        DataSet ds = new DataSet();
                        Database db = new SqlDatabase(StrSetConnection());
                        System.Data.Common.DbCommand CmdGetParameterSMS = null;

                        CmdGetParameterSMS = db.GetStoredProcCommand("[Trans].[GatewayTransGetById]", Convert.ToInt32(GatewayTransId));
                        db.LoadDataSet(CmdGetParameterSMS, ds, new string[] { "GatewayTrans" });



                        if (objResult.result[0].amount == Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]) || Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]) == Convert.ToDecimal(0))
                        {
                            if (objResult.result[0].status == "Settlement in Progress" || objResult.result[0].status == "Money with Payumoney" || objResult.result[0].status == "Settlement in Process" || objResult.result[0].status == "Money Settled" || objResult.result[0].status == "Settlement in process")
                            {
                                DataSet dsCustomerSMS = new DataSet();
                                Database dbOrderSMS = new SqlDatabase(StrSetConnection());
                                CmdGetParameterSMS = null;

                                CmdGetParameterSMS = dbOrderSMS.GetStoredProcCommand("[Security].[GatewayTransPayStatusUpdate]", Convert.ToInt32(GatewayTransId), Convert.ToInt32(StatusId), StatusDescription, RefGateway, objResult.result[0].amount);
                                dbOrderSMS.LoadDataSet(CmdGetParameterSMS, dsCustomerSMS, new string[] { "GatewayTrans" });
                                trans.Commit();
                                objreturndbmlGatewayTransTemp.StatusId = 1;
                                objreturndbmlGatewayTransTemp.Status = "Successfully";
                            }
                            else
                            {
                                objreturndbmlGatewayTransTemp.StatusId = 2;
                                objreturndbmlGatewayTransTemp.Status = "Something went wrong";
                            }
                        }
                        else
                        {
                            objreturndbmlGatewayTransTemp.StatusId = 2;
                            objreturndbmlGatewayTransTemp.Status = "Something went wrong";
                        }
                    }
                    else
                    {
                        objreturndbmlGatewayTransTemp.StatusId = 2;
                        objreturndbmlGatewayTransTemp.Status = "GatewayId Not Match";
                    }
                }
                else
                {
                    objreturndbmlGatewayTransTemp.StatusId = 2;
                    objreturndbmlGatewayTransTemp.Status = "GatewayId Not Match";
                }

            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlGatewayTransTemp.Status = ex.Message + ", " + ex.StackTrace.ToString();

                objreturndbmlGatewayTransTemp.StatusId = 99;
            }
            finally
            {
                con.Close();

            }
            return objreturndbmlGatewayTransTemp;
        }

        #endregion

        #region OrderInsertTemp

        public returndbmlBuyNow InsertBuyNowTemp(returndbmlBuyNow objreturndbmlBuyNow)
        {
            returndbmlBuyNow objreturndbmlBuyNow1 = new returndbmlBuyNow();
            DbTransaction trans;
            DbConnection con;
            Database db = new SqlDatabase(StrSetConnection());
            System.Data.Common.DbCommand CmdGetParameter = null;
            con = db.CreateConnection();
            con.Open();
            trans = con.BeginTransaction();
            int intOrderHeaderId = 0;
            try
            {

                
                dbmlOrderHeader objdbmlOrderHeader = objreturndbmlBuyNow.objdbmlOfferHeader.FirstOrDefault();
                CmdGetParameter = db.GetStoredProcCommand("Master.OrderHeaderInsertTemp");
                CmdGetParameter.CommandTimeout = 8000000;

                //-----------OrderHeader----------//

                foreach (PropertyInfo PropInfoCol in objdbmlOrderHeader.GetType().GetProperties())
                {
                    DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                    db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(objdbmlOrderHeader, null));

                }

                db.AddOutParameter(CmdGetParameter, "OrderHeaderIdOut", DbType.Int32, 0);
                db.ExecuteNonQuery(CmdGetParameter, trans);
                intOrderHeaderId = (int)db.GetParameterValue(CmdGetParameter, "@OrderHeaderIdOut");

                //-----------OrderDetail----------//

                foreach (var ODetail in objreturndbmlBuyNow.objdbmlOfferDetail)
                {
                    CmdGetParameter = db.GetStoredProcCommand("Master.OrderDetailInsertTemp");
                    CmdGetParameter.CommandTimeout = 8000000;

                    ODetail.OrderHeaderId = intOrderHeaderId;
                    foreach (PropertyInfo PropInfoCol in ODetail.GetType().GetProperties())
                    {
                        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(ODetail, null));


                    }
                    db.ExecuteNonQuery(CmdGetParameter, trans);
                }


                ////-----------OrderSupplimentary----------//

                //foreach (var ODetail in objreturndbmlBuyNow.objdbmlOfferSupplimentary)
                //{
                //    CmdGetParameter = db.GetStoredProcCommand("Master.OrderSupplimentaryInsert");
                //    CmdGetParameter.CommandTimeout = 8000000;

                //    ODetail.OrderHeaderId = intOrderHeaderId;
                //    foreach (PropertyInfo PropInfoCol in ODetail.GetType().GetProperties())
                //    {
                //        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                //        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(ODetail, null));


                //    }
                //    db.ExecuteNonQuery(CmdGetParameter, trans);
                //}

                //----------Gatway Trans------------//

                //foreach (var GatWayTrans in objreturndbmlBuyNow.objdbmlGatwayTrans)
                //{
                //    CmdGetParameter = db.GetStoredProcCommand("Master.GatwayTransInsert");
                //    CmdGetParameter.CommandTimeout = 8000000;

                //    GatWayTrans.OrderHeaderId = intOrderHeaderId;
                //    foreach (PropertyInfo PropInfoCol in GatWayTrans.GetType().GetProperties())
                //    {
                //        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                //        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(GatWayTrans, null));

                //    }
                //    db.ExecuteNonQuery(CmdGetParameter, trans);
                //}                

                //----------ShippingAddress------------//

                foreach (var SAddress in objreturndbmlBuyNow.objdbmlShippingAddress)
                {
                    CmdGetParameter = db.GetStoredProcCommand("Master.ShippingAddressInsertTemp");
                    CmdGetParameter.CommandTimeout = 8000000;

                    SAddress.OrderHeaderId = intOrderHeaderId;
                    foreach (PropertyInfo PropInfoCol in SAddress.GetType().GetProperties())
                    {
                        DbType dbt = ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(CmdGetParameter, PropInfoCol.Name.ToString(), dbt, PropInfoCol.GetValue(SAddress, null));

                    }
                    db.ExecuteNonQuery(CmdGetParameter, trans);
                }

                trans.Commit();
                objreturndbmlBuyNow1 = objreturndbmlBuyNow;
                objreturndbmlBuyNow1.StatusId = 1;
                //objreturndbmlBuyNow1.Status = Convert.ToString(intOrderHeaderId);


            }
            catch (Exception ex)
            {
                trans.Rollback();
                objreturndbmlBuyNow1.Status = ex.Message + ", " + ex.StackTrace.ToString();
                objreturndbmlBuyNow1.StatusId = 99;

            }
            finally
            {
                con.Close();

            }

            return objreturndbmlBuyNow1;

        }

        #endregion


        #region SendOTP

        public dbmlStatus SendOTP(string MobileNo,string OTP)
        {
            dbmlStatus obj = new dbmlStatus();
             double value;
             if (double.TryParse(MobileNo, out value))
             {
                 if (MobileNo.Length == 10)
                 {
                     string msg = "Welcome to BillaSale. Use OTP : " + OTP + " to Sign up on BillaSale";
                     //if (SendSMS(MobileNo, msg))
                     //{
                         obj.StatusId = 1;
                         obj.Status = OTP;
                     //}
                 }
             }

            return obj;
        
        }

        #endregion


        #region SMS

        public bool SendSMS(string strMobileNo, string strMsg)
        {

            //  string authKey = "86595AqYFjZW7pIN557a8b5e";
            string authKey = "116975Ay9hK7OTB05768f1b0";
            //Multiple mobiles numbers separated by comma
            string mobileNumber = strMobileNo;// "7898703946";
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderId = "yumOYE";
            //Your message to send, Add URL encoding here.
            // string message = HttpUtility.UrlEncode("How r u Nalin? This message from Prizm kiosk, agrar mila to reply do whatsApp par");
            string message = HttpUtility.UrlEncode(strMsg);

            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&sender={0}", senderId);
            // sbPostData.AppendFormat("&route={0}", "default");
            sbPostData.AppendFormat("&route={0}", 4);

            try
            {
                //Call Send SMS API
                string sendSMSUri = "http://api.msg91.com/sendhttp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
                return true;
            }
            catch
            {
                return false;
            }
            // Response.Write("Message sent, cheers!");

        }

        #endregion

    }
}
