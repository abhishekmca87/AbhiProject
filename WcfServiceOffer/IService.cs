using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceOffer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        #region User
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/UserInsert")]
        returndbmlUser UserInsert(returndbmlUser objUser);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "UserGetByMobileNoPassword/{MobileNo}/{Password}")]
        returndbmlUser UserGetByMobileNoPassword(string MobileNo, string Password);
        #endregion

        #region Offer
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/OfferInsert")]
        returndbmlOffer OfferInsert(returndbmlOffer objOffer);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/OfferUpdate")]
        returndbmlOffer OfferUpdate(returndbmlOffer objOffer);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferGetByCatId/{CatId}")]
        returndbmlOffer OfferGetByCatId(string CatId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferGetBySubCatId/{SubCatId}")]
        returndbmlOffer OfferGetBySubCatId(string SubCatId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferDetailGetbyOfferId/{OfferId}")]
        returndbmlOfferDetailGetbyOfferIdResult OfferDetailGetbyOfferId(string OfferId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferGetByDiscount/{Discount}")]
        returndbmlOffer OfferGetByDiscount(string Discount);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferGetByVendorId/{VendorId}")]
        returndbmlOffer OfferGetByVendorId(string VendorId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferGetByLocation/{Location}")]
        returndbmlOffer OfferGetByLocation(string Location);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferGetByShopId/{ShopId}")]
        returndbmlOffer OfferGetByShopId(string ShopId);

        #endregion

        #region Vendor
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/VendorInsert")]
        returndbmlVendor VendorInsert(returndbmlVendor objVendor);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/VendorUpdate")]
        returndbmlVendor VendorUpdate(returndbmlVendor objVendor);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "VendorGetAll")]
        returndbmlVendor VendorGetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "VendorGetByMobileNoPassword/{MobileNo}/{Password}/{NewFcmId}")]
        returndbmlVendor VendorGetByMobileNoPassword(string MobileNo, string Password,string NewFcmId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "VendorDetailGetbyOfferId/{OfferId}")]
        returndbmlVendor VendorDetailGetbyOfferId(string OfferId);



        #endregion

        #region Category
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CategoryInsert")]
        returndbmlCategory CategoryInsert(returndbmlCategory objCategory);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CategoryGetAll/")]
        returndbmlCategory CategoryGetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CategoryGetByCatType/{CatType}")]
        returndbmlCategory CategoryGetByCatType(string CatType);

        #endregion

        #region SubCategory
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SubCategoryInsert")]
        returndbmlSubCategory SubCategoryInsert(returndbmlSubCategory objSubCategory);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SubCategoryGetByCatId/{CatId}")]
        returndbmlSubCategory SubCategoryGetByCatId(string CatId);

       

        #endregion

        #region TopVendorOffer
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/TopVendorOfferInsert")]
        returndbmlTopVendorOffer TopVendorOfferInsert(returndbmlTopVendorOffer objTopVendorOffer);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "TopVendorOfferGetAll")]
        returndbmlTopVendorOfferGetAllResult TopVendorOfferGetAll();
        #endregion

        #region Shop
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ShopInsert")]
        returndbmlShop ShopInsert(returndbmlShop objShop);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ShopUpdate")]
        returndbmlShop ShopUpdate(returndbmlShop objShop);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ShopGetByVendorId/{VendorId}")]
        returndbmlShop ShopGetByVendorId(string VendorId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ShopGetAll")]
        returndbmlShop ShopGetAll();

        #endregion

        #region Location

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "LocationGetAll")]
        returndbmlLocation LocationGetAll();

        #endregion

        #region Campaign

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CampaignGetbyOfferId/{OfferId}")]
        returndbmlCampaign CampaignGetbyOfferId(string OfferId);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CampaignInsert")]
        returndbmlCampaign CampaignInsert(returndbmlCampaign objCampaign);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendOfferCodeByContactNoOfferId/{MobileNo}/{OfferId}/{UserId}")]
        dbmlSendSMS SendOfferCodeByContactNoOfferId(string MobileNo, string OfferId, string UserId);

        #endregion

        #region BannerImage

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "BannerImageGetAll")]
        returndbmlBannerImage BannerImageGetAll();

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "BannerImageGetByCategoryId/{CategoryId}")]
        returndbmlBannerImage BannerImageGetByCategoryId(string CategoryId);

        #endregion

        #region OfferCount
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferCount/{OfferId}/{Count}/{FcmId}")]
        returndbmlOfferCount OfferCount(string OfferId, string Count,string FcmId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferCountGetByOfferId/{OfferId}")]
        returndbmlOfferCount OfferCountGetByOfferId(string OfferId);

        #endregion

        #region OfferSource

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "OfferSourceGetAll")]
        returndbmlOfferSource OfferSourceGetAll();

        #endregion

        #region ChkVersion
        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json, UriTemplate = "CheckVersion/")]
        dbmlStatus CheckVersion();

        #endregion

        #region BuyNow

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/InsertBuyNow")]
        returndbmlBuyNow InsertBuyNow(returndbmlBuyNow objreturndbmlBuyNow);

        #endregion

        #region BuyNowTemp

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/InsertBuyNowTemp")]
        returndbmlBuyNow InsertBuyNowTemp(returndbmlBuyNow objreturndbmlBuyNow);

        #endregion

        #region Product

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ProductInsert")]
        returndbmlProduct ProductInsert(returndbmlProduct objreturndbmlProduct);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ProductGetByCatId/{CatId}")]
        returndbmlProduct ProductGetByCatId(string CatId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ProductGetBySubCatId/{SubCatId}")]
        returndbmlProduct ProductGetBySubCatId(string SubCatId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ProductGetByProductName/{ProductName}")]
        returndbmlProduct ProductGetByProductName(string ProductName);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ProductDetailGetByProductId/{ProductId}")]
        returndbmlProductGetbyCatIdResult ProductDetailGetByProductId(string ProductId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ProductImageGetByProductId/{ProductId}")]
        returndbmlProductImage ProductImageGetByProductId(string ProductId);

        #endregion

        #region VendorOtherInfo

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/VendorOtherInfoInsert")]
        returndbmlVendorOtherInfo VendorOtherInfoInsert(returndbmlVendorOtherInfo objreturndbmlVendorOtherInfo);

        #endregion

        #region ProductImage

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ProductImageInsert")]
        returndbmlProductImage ProductImageInsert(returndbmlProductImage objreturndbmlProductImage);

        #endregion

        #region Specification

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SpecificationInsert")]
        returndbmlSpecification SpecificationInsert(returndbmlSpecification objreturndbmlSpecification);

        #endregion

        #region ProductSpecification

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ProductSpecificationInsert")]
        returndbmlProductSpecification ProductSpecificationInsert(returndbmlProductSpecification objreturndbmlProductSpecification);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ProductSpecificationGetByProductId/{ProductId}")]
        returndbmlProductSpecification ProductSpecificationGetByProductId(string ProductId);

        #endregion

        #region SpecificationCatIdLink

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/SpecificationCatIdLinkInsert")]
        returndbmlSpecificationCatIdLink SpecificationCatIdLinkInsert(returndbmlSpecificationCatIdLink objreturndbmlSpecificationCatIdLink);

        #endregion

        #region ProductTag

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ProductTagInsert")]
        returndbmlProductTag ProductTagInsert(returndbmlProductTag objreturndbmlProductTag);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ProductTagGetbyProductId/{ProductId}")]
        returndbmlProductTag ProductTagGetbyProductId(string ProductId);


        #endregion

        #region Shipping Address

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ShippingAddressGetByUserId/{UserId}")]
        returndbmlShippingAddress ShippingAddressGetByUserId(string UserId);

        #endregion

        #region GatewayTrans
        [OperationContract]
        [WebInvoke(UriTemplate = "/GatewayTransInsert",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        returndbmlGatewayTrans GatewayTransInsert(returndbmlGatewayTrans objdbmlGatewayTrans);


        [OperationContract]
        [WebInvoke(Method = "GET",
             BodyStyle = WebMessageBodyStyle.Wrapped,
                  ResponseFormat = WebMessageFormat.Json, UriTemplate = "GatewayTransUpdate/{GatewayTransId}/{StatusId}/{StatusDescription}/{RefGateway}/{Amount}")]
        returndbmlGatewayTrans GatewayTransUpdate(string GatewayTransId, string StatusId, string StatusDescription, string RefGateway, string Amount);

        #endregion


        #region SendOTP

        [OperationContract]
        [WebInvoke(Method = "GET",
             BodyStyle = WebMessageBodyStyle.Wrapped,
                  ResponseFormat = WebMessageFormat.Json, UriTemplate = "SendOTP/{MobileNo}/{OTP}")]
        dbmlStatus SendOTP(string MobileNo, string OTP);

        #endregion
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
