using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfServiceOffer
{

    #region User
    [DataContract]
    public class returndbmlUser
    {
        [DataMember]
        public ObservableCollection<dbmlUser> objdbmlUser { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion
    #region Offer
    [DataContract]
    public class returndbmlOffer
    {
        [DataMember]
        public ObservableCollection<dbmlOffer> objdbmlOffer { get; set; }
        [DataMember]
        public ObservableCollection<dbmlOfferResult> objdbmlOfferResult { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    public class returndbmlOfferDetailGetbyOfferIdResult
    {
        [DataMember]
        public ObservableCollection<OfferDetailGetbyOfferIdResult> objOfferDetailGetbyOfferIdResult { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion
    #region Shop
    [DataContract]
    public class returndbmlShop
    {
        [DataMember]
        public ObservableCollection<dbmlShop> objdbmlShop { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion
    #region Vendor
    [DataContract]
    public class returndbmlVendor
    {
        [DataMember]
        public ObservableCollection<dbmlVendor> objdbmlVendor { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion
    #region TopVendorOffer
    [DataContract]
    public class returndbmlTopVendorOffer
    {
        [DataMember]
        public ObservableCollection<dbmlTopVendorOffer> objdbmlTopVendorOffer { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    public class returndbmlTopVendorOfferGetAllResult
    {
        [DataMember]
        public ObservableCollection<TopVendorOfferGetAllResult> objdbmlTopVendorOfferGetAllResult { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }





    #endregion

    #region Category
    [DataContract]
    public class returndbmlCategory
    {
        [DataMember]
        public ObservableCollection<dbmlCategory> objdbmlCategory { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion

    #region SubCategory
    [DataContract]
    public class returndbmlSubCategory
    {
        [DataMember]
        public ObservableCollection<dbmlSubCategory> objdbmlSubCategory { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion


    #region Location
    [DataContract]
    public class returndbmlLocation
    {
        [DataMember]
        public ObservableCollection<dbmlLocation> objdbmlLocation { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion
    #region Campaign
    [DataContract]
    public class returndbmlCampaign
    {
        [DataMember]
        public ObservableCollection<dbmlCampaign> objdbmlCampaign { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion
    #region RedeemOffer
    [DataContract]
    public class returndbmlRedeemOffer
    {
        [DataMember]
        public ObservableCollection<dbmlRedeemOffer> objdbmlRedeemOffer { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion
    #region BannerImage
    [DataContract]
    public class returndbmlBannerImage
    {
        [DataMember]
        public ObservableCollection<dbmlBannerImage> objdbmlBannerImage { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion

    #region OfferSource
    [DataContract]
    public class returndbmlOfferSource
    {
        [DataMember]
        public ObservableCollection<dbmlOfferSource> objdbmlOfferSource { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion

    #region OfferCount
    [DataContract]
    public class returndbmlOfferCount
    {
        [DataMember]
        public ObservableCollection<dbmlOfferCount> objdbmlOfferCount { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion

    #region BuyNow
    [DataContract]
    public class returndbmlBuyNow
    {
        [DataMember]
        public ObservableCollection<dbmlOrderHeader> objdbmlOfferHeader { get; set; }
        [DataMember]
        public ObservableCollection<dbmlOrderDetail> objdbmlOfferDetail { get; set; }
        //[DataMember]
        //public ObservableCollection<dbmlGatwayTrans> objdbmlGatwayTrans { get; set; }
        [DataMember]
        public ObservableCollection<dbmlShippingAddress> objdbmlShippingAddress { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion


    #region Product

    [DataContract]
    public class returndbmlProduct
    {
        [DataMember]
        public ObservableCollection<dbmlProduct> objdbmlProduct { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion


    public class returndbmlProductGetbyCatIdResult
    {
        [DataMember]
        public ObservableCollection<ProductDetailGetbyProductIdResult> objProductDetailGetbyProductIdResult { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }


    #region VendorOtherInfo

    [DataContract]
    public class returndbmlVendorOtherInfo
    {
        [DataMember]
        public ObservableCollection<dbmlVendorOtherInfo> objdbmlVendorOtherInfo { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion


    #region ProductImage

    [DataContract]
    public class returndbmlProductImage
    {
        [DataMember]
        public ObservableCollection<dbmlProductImage> objdbmlProductImage { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion

    #region Specification

    [DataContract]
    public class returndbmlSpecification
    {
        [DataMember]
        public ObservableCollection<dbmlSpecification> objdbmlSpecification { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion


    #region ProductSpecification

    [DataContract]
    public class returndbmlProductSpecification
    {
        [DataMember]
        public ObservableCollection<dbmlProductSpecification> objdbmlProductSpecification { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion


    #region SpecificationCatIdLink

    [DataContract]
    public class returndbmlSpecificationCatIdLink
    {
        [DataMember]
        public ObservableCollection<dbmlSpecificationCatIdLink> objdbmlSpecificationCatIdLink { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion

    #region ProductTag

    [DataContract]
    public class returndbmlProductTag
    {
        [DataMember]
        public ObservableCollection<dbmlProductTag> objdbmlProductTag { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion

    #region ShippingAddress
    [DataContract]
    public class returndbmlShippingAddress
    {
        [DataMember]
        public ObservableCollection<dbmlShippingAddress> objdbmlShippingAddress { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    #endregion


    #region GatewayTrans
    [DataContract]
    public class returndbmlGatewayTrans
    {
        [DataMember]
        public ObservableCollection<dbmlGatwayTrans> objdbmlGatewayTrans { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
    #endregion

    #region PayuMoney

    public class PayuMoneyResponceStatus
    {

        public List<result> result { get; set; }
        public string status { get; set; }
        public string message { get; set; }       

    }
    public class result
    {
        public string merchantTransactionId { get; set; }
        public int paymentId { get; set; }
        public string status { get; set; }
        public decimal amount { get; set; }

    }
    #endregion

    public class dbmlSendSMS
    {
        [DataMember]
        public string MobileNo { get; set;}
        [DataMember]
        public string OfferId{get;set;}
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

}