using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using paytm;
using Razorpay.Api;
using Stadhawk.Laundry.Utility.Enums;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.ViewModel.RequestModel;
using StadhawkLaundry.ViewModel.ResponseModel;

namespace StadhawkLaundry.API.Controllers
{
    public class PaymentTransactionController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unit;
        public PaymentTransactionController(IOptions<AppSettings> appSettings, IUnitOfWork unit)
        {
            _appSettings = appSettings.Value;
            _unit = unit;
        }
        [HttpGet]
        public async Task<ActionResult> Index(int orderId, string pgType)
        {
            PaymetIfoRequestViewModel model = null;
            var resultData = new PaymentOrderResponceViewModel();
            var data = new PaymentOrderResponceViewModel();
            if (string.IsNullOrEmpty(pgType))
                pgType = _appSettings.PGType;

            if (pgType.Equals("2"))
                pgType = _appSettings.PGType;

            EnumType.PaymentGateWayType paymentGateWayType = (EnumType.PaymentGateWayType)Enum.Parse(typeof(EnumType.PaymentGateWayType), pgType);
            string pgRequestData = string.Empty;
            bool isExist = false;
            switch (paymentGateWayType)
            {
                #region RAZORPAY PAYMENT GATEWAY REQUEST
                case EnumType.PaymentGateWayType.RAZORPAY:

                    ViewBag.PGType = EnumType.PaymentGateWayType.RAZORPAY.ToString();
                    resultData = _unit.IOrder.GetOrderDetails(orderId).UserObject;
                    if (resultData.IsBookingOn == false)
                    {
                        var redirectUrl = _appSettings.ReturnUrl + "?Status=" + "BookingClosed";
                        Response.Redirect(redirectUrl);
                    }
                    if (resultData != null)
                    {
                        var options = new Dictionary<string, object>();
                        var hashData = new Hashtable();
                        var htNotes = new Hashtable();
                        htNotes.Add("amount", resultData.Price);
                        options.Add("notes", htNotes);
                        //order detail
                        options.Add("amount", (resultData.Price * 100));
                        options.Add("receipt", resultData.InvoiceNo);
                        options.Add("currency", _appSettings.TransactionCurrency);
                        options.Add("payment_capture", "1");
                        //Razor pay client
                        RazorpayClient client = new RazorpayClient(_appSettings.RazorpayKey, _appSettings.RazorpaySecret);
                        Order order = client.Order.Create(options);

                        //hash data
                        hashData.Add("data-key", _appSettings.RazorpayKey);
                        hashData.Add("data-amount", (resultData.Price * 100));
                        hashData.Add("data-name", _appSettings.Company);
                        hashData.Add("data-description", _appSettings.RazorPayDescription);
                        hashData.Add("data-order_id", order["id"].ToString());
                        hashData.Add("data-image", _appSettings.RazorPayLogo);
                        hashData.Add("data-prefill.name", resultData.CustomerName);
                        hashData.Add("data-prefill.email", resultData.EmailId);
                        hashData.Add("data-prefill.contact", resultData.MobileNo);
                        hashData.Add("data-theme.color", _appSettings.RazorPayColor);
                        //serialized object
                        var orderSerialized = JsonConvert.SerializeObject(order);
                        var orderResponse = JsonConvert.DeserializeObject<RazorpayGateWayOrderResponseViewModel>(orderSerialized);
                        model = new PaymetIfoRequestViewModel
                        {
                            InvoiceNo = orderResponse.Attributes.Receipt,
                            PgRequest = orderSerialized,
                            PgType = EnumType.PaymentGateWayType.RAZORPAY.ToString()
                        };
                        try
                        {
                            if (!isExist)
                            {
                                var result = _unit.IOrder.SaveCustomerPaymentInfo(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                        }
                        //post form data
                        var hashtable = new Hashtable(hashData);
                        pgRequestData = PostFormData("PaymentResponse", hashtable);
                    }
                    break;

                #endregion
                #region PAYTM PAYMENT GATEWAY REQUEST

                case EnumType.PaymentGateWayType.PAYTM:
                    data = _unit.IOrder.GetOrderDetails(orderId).UserObject;
                    if (data != null)
                    {
                        if (data.IsBookingOn == false)
                        {
                            var redirectUrl = _appSettings.ReturnUrl + "?Status=" + "BookingClosed";
                            Response.Redirect(redirectUrl);
                        }
                    }


                    // var data = _unit.Order.GetRezorPaymenOrdertDetails(orderId).UserObject;
                    if (data != null)
                    {
                        if (data.IsBookingOn == false)
                        {
                            var redirectUrl = _appSettings.ReturnUrl + "?Status=" + "BookingClosed";
                            Response.Redirect(redirectUrl);
                        }

                        isExist = _unit.IOrder.IsOrderRefExist(data.InvoiceNo, pgType).UserObject;

                        /* initialize a TreeMap object */
                        Dictionary<String, String> paytmParams = new Dictionary<String, String>();
                        /* Find your MID in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
                        paytmParams.Add("MID", _appSettings.PAYTM_MID);
                        /* Find your WEBSITE in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
                        paytmParams.Add("WEBSITE", _appSettings.PAYTM_WEBSITE);
                        /* Find your INDUSTRY_TYPE_ID in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
                        paytmParams.Add("INDUSTRY_TYPE_ID", _appSettings.PAYTM_INDUSTRY_TYPE_ID);
                        /* WEB for website and WAP for Mobile-websites or App */
                        paytmParams.Add("CHANNEL_ID", _appSettings.PAYTM_CHANNEL_ID);
                        /* Enter your unique order id */
                        paytmParams.Add("ORDER_ID", data.InvoiceNo);
                        /* unique id that belongs to your customer */
                        paytmParams.Add("CUST_ID", data.CustomerId);
                        /* customer's mobile number */
                        paytmParams.Add("MOBILE_NO", data.MobileNo);
                        /* customer's email */
                        paytmParams.Add("EMAIL", data.EmailId);
                        /* Amount in INR that is payble by customer
                        * this should be numeric with optionally having two decimal points*/
                        paytmParams.Add("TXN_AMOUNT", data.Price.ToString());
                        /* on completion of transaction, we will send you the response on this URL */
                        paytmParams.Add("CALLBACK_URL", _appSettings.ReturnUrl);

                        /* Generate checksum for parameters we have  */
                        string checksum = paytm.CheckSum.generateCheckSum(_appSettings.PAYTM_MERCHANT_KEY, paytmParams);
                        try
                        {
                            model = new PaymetIfoRequestViewModel
                            {
                                InvoiceNo = data.InvoiceNo,
                                PgRequest = JsonConvert.SerializeObject(paytmParams, Formatting.Indented),
                                PgType = EnumType.PaymentGateWayType.PAYTM.ToString()
                            };

                            if (!isExist)
                            {
                                var result = _unit.IOrder.SaveCustomerPaymentInfo(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                        }
                        /* Prepare HTML Form and Submit to Paytm */
                        string htmlString = "";
                        htmlString += "<html>";
                        htmlString += "<head>";
                        htmlString += "<title>Merchant Checkout Page</title>";
                        htmlString += "</head>";
                        htmlString += "<body>";
                        htmlString += "<center><h1>Please do not refresh this page...</h1></center>";
                        htmlString += "<form method='post' action='" + (_appSettings.IsLivePayment == "Y" ? _appSettings.PAYTM_LIVE_URL : _appSettings.PAYTM_STAGING_URL) + "' name='paytm_form'>";
                        foreach (string key in paytmParams.Keys)
                        {
                            htmlString += "<input type='hidden' name='" + key + "' value='" + paytmParams[key] + "'>";
                        }
                        htmlString += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
                        htmlString += "</form>";
                        htmlString += "<script type='text/javascript'>";
                        htmlString += "document.paytm_form.submit();";
                        htmlString += "</script>";
                        htmlString += "</body>";
                        htmlString += "</html>";
                        //request data
                        pgRequestData = htmlString;
                    }
                    break;
                    #endregion
            }
            TempData["PaytmType"] = pgType;
            return View(model: pgRequestData);

        }
        // GET: PaymentTransaction
        [HttpPost]
        public void PaymentResponse()
        {
            string paymentStatus = string.Empty;
            string paymentGatewayType = string.Empty;
            if (Request.Form.Keys.Count > 0)
            {
                if (!string.IsNullOrEmpty(Request.Form["razorpay_payment_id"]))
                {
                    paymentGatewayType = "RAZORPAY";
                }
                else
                {
                    paymentGatewayType = "PAYTM";
                }
            }
            EnumType.PaymentGateWayType paymentGateWayType = (EnumType.PaymentGateWayType)Enum.Parse(typeof(EnumType.PaymentGateWayType), paymentGatewayType);
            string redirectUrl = string.Empty;
            string orderRefContain = string.Empty;
            switch (paymentGateWayType)
            {
                #region RAZORPAY PAYMENT GATEWAY RESPONSE

                case EnumType.PaymentGateWayType.RAZORPAY:
                    RazorpayClient client = new RazorpayClient(_appSettings.RazorpayKey, _appSettings.RazorpaySecret);
                    //order
                    var order = client.Order.Fetch(Request.Form["razorpay_order_id"]);
                    var orderSerialized = JsonConvert.SerializeObject(order);
                    var orderResponse = JsonConvert.DeserializeObject<RazorpayGateWayOrderResponseViewModel>(orderSerialized);
                    //payment
                    string paymentId = Request.Form["razorpay_payment_id"];
                    var payment = client.Payment.Fetch(paymentId);
                    var paymentSerialized = JsonConvert.SerializeObject(payment);
                    var paymentResponse = JsonConvert.DeserializeObject<RazorpayGateWayResponseViewModel>(paymentSerialized);
                    //save payment detail status wise

                    orderRefContain = orderResponse.Attributes.Receipt.ToString().Substring(0, 2);
                    switch (paymentResponse.Attributes.Status.ToLower())
                    {
                        case "created":
                            break;
                        case "authorized":
                            var authorizedmodel = new PaymetIfoRequestViewModel
                            {
                                InvoiceNo = orderResponse.Attributes.Receipt,
                                PgResponse = "Authorized",
                                PgType = EnumType.PaymentGateWayType.RAZORPAY.ToString()
                            };
                            _unit.IOrder.SaveCustomerPaymentInfo(authorizedmodel);
                            break;
                        case "captured":
                            var model = new PaymetIfoRequestViewModel
                            {
                                InvoiceNo = orderResponse.Attributes.Receipt,
                                PgResponse = paymentSerialized,
                                PgType = EnumType.PaymentGateWayType.RAZORPAY.ToString(),
                                PGTransactionId = paymentResponse.Attributes.Id,
                                Status = paymentResponse.Attributes.Status,
                            };
                            try
                            {
                                paymentStatus = "OK";
                                var paymentResponseSave = (dynamic)null;
                                paymentResponseSave = _unit.IOrder.SaveCustomerPaymentInfo(model);
                                redirectUrl = _appSettings.RazorPayReturnUrl + "?Status=" + paymentStatus;
                                ViewBag.Message = "Transaction Successful.";
                            }
                            catch (Exception ex)
                            {
                                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                            }
                            break;

                        case "refunded":
                            ViewBag.Message = "Transaction Refunded.";
                            break;

                        case "failed":
                            paymentStatus = "F";
                            var failedModel = new PaymetIfoRequestViewModel
                            {
                                InvoiceNo = orderResponse.Attributes.Receipt,
                                PgResponse = paymentSerialized,
                                PgType = EnumType.PaymentGateWayType.RAZORPAY.ToString(),
                                PGTransactionId = paymentResponse.Attributes.Id,
                                Status = paymentResponse.Attributes.Status,
                            };
                            try
                            {
                                var paymentResponseSave = _unit.IOrder.SaveCustomerPaymentInfo(failedModel);
                                //var paymentResponseSave = _unit.Order.SaveCustomerPaymentInfo(failedModel);
                                redirectUrl = _appSettings.RazorPayReturnUrl + "?Status=" + paymentStatus;
                                ViewBag.Message = "Transaction Failed.";
                            }
                            catch (Exception ex)
                            {
                                ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                            }
                            redirectUrl = _appSettings.ReturnUrl + "?Status=" + paymentStatus;
                            ViewBag.Message = "Transaction Failed. Please contact our support team.";
                            break;
                    }
                    break;
                #endregion

                #region PAYTM PAYMENT GATEWAY RESPONSE
                case EnumType.PaymentGateWayType.PAYTM:

                    if (Request.Form.Keys.Count > 0)
                    {
                        string paytmChecksum = string.Empty; ;
                        string pgType = EnumType.PaymentGateWayType.PAYTM.ToString();

                        Dictionary<string, string> parameters = new Dictionary<string, string>();
                        try
                        {
                            foreach (string key in Request.Form.Keys)
                            {
                                if (Request.Form[key].ToString().IndexOf("|") != -1)
                                {
                                    parameters.Add(key.Trim(), "");
                                }
                                else
                                {
                                    parameters.Add(key.Trim(), Request.Form[key].ToString().Trim());
                                }
                            }
                            if (parameters.ContainsKey("CHECKSUMHASH"))
                            {
                                paytmChecksum = parameters["CHECKSUMHASH"];
                                parameters.Remove("CHECKSUMHASH");
                            }
                            if (CheckSum.verifyCheckSum(_appSettings.PAYTM_MERCHANT_KEY, parameters, paytmChecksum))
                            {
                                parameters.Add("IS_CHECKSUM_VALID", "Y");
                            }
                            else
                            {
                                parameters.Add("IS_CHECKSUM_VALID", "N");
                            }
                        }
                        catch (Exception ex)
                        {
                            parameters.Add("IS_CHECKSUM_VALID", "N");
                        }
                        //Payment response after serialization
                        var responseJSON = JsonConvert.SerializeObject(parameters);
                        PaytmPaymentResponse responseObject = JsonConvert.DeserializeObject<PaytmPaymentResponse>(responseJSON);
                        if (responseObject != null)
                        {
                            //re-verify request
                            Dictionary<string, string> reVerifyParameters = new Dictionary<string, string>();
                            reVerifyParameters.Add("MID", responseObject.MID);
                            reVerifyParameters.Add("ORDER_ID", responseObject.ORDERID);
                            string reVerifyCheckSum = paytm.CheckSum.generateCheckSum(_appSettings.PAYTM_MERCHANT_KEY, reVerifyParameters);
                            reVerifyParameters.Add("CHECKSUMHASH", reVerifyCheckSum);
                            //re-verify request json
                            string verifiedRequestJSON = JsonConvert.SerializeObject(reVerifyParameters);
                            //call post web request for transaction status
                            var verifiedResponseJSON = PostWebRequest(_appSettings.IsLivePayment.Equals("Y") ? _appSettings.PAYTM_TRANSACTION_STATUS_LIVE_URL : _appSettings.PAYTM_TRANSACTION_STATUS_STAGING_URL, verifiedRequestJSON);
                            //deserialize json to object
                            PaytmPaymentResponse verifiedResponseObject = JsonConvert.DeserializeObject<PaytmPaymentResponse>(verifiedResponseJSON);
                            //re-veryfy transaction amount
                            // string orderRefContain = string.Empty;
                            if (verifiedResponseObject != null && verifiedResponseObject.TXNAMOUNT == responseObject.TXNAMOUNT)
                            {
                                switch (verifiedResponseObject.STATUS.ToUpper())
                                {
                                    case "TXN_SUCCESS":
                                        var model = new PaymetIfoRequestViewModel
                                        {
                                            InvoiceNo = verifiedResponseObject.ORDERID,
                                            PgResponse = verifiedResponseJSON,
                                            PgType = pgType,
                                            PGTransactionId = verifiedResponseObject.TXNID,
                                            Status = verifiedResponseObject.STATUS.Replace("TXN_SUCCESS", "captured"),
                                        };
                                        try
                                        {
                                            paymentStatus = "OK";
                                            orderRefContain = model.InvoiceNo.ToString().Substring(0, 2);
                                            var paymentResponseSave = (dynamic)null;
                                            paymentResponseSave = _unit.IOrder.SaveCustomerPaymentInfo(model);

                                            redirectUrl = _appSettings.RazorPayReturnUrl + "?Status=" + paymentStatus;
                                            ViewBag.Message = "Transaction Successful.";
                                        }
                                        catch (Exception ex)
                                        {
                                            ErrorTrace.Logger(LogArea.ApplicationTier, ex);
                                        }
                                        break;

                                    case "TXN_FAILURE":
                                        paymentStatus = "F";
                                        var failedModel = new PaymetIfoRequestViewModel
                                        {
                                            InvoiceNo = verifiedResponseObject.ORDERID,
                                            PgResponse = verifiedResponseJSON,
                                            PgType = pgType,
                                            PGTransactionId = verifiedResponseObject.TXNID,
                                            Status = verifiedResponseObject.STATUS.Replace("TXN_FAILURE", "failed"),
                                        };
                                        redirectUrl = _appSettings.ReturnUrl + "?Status=" + paymentStatus;
                                        ViewBag.Message = "Transaction Failed. Please contact our support team.";
                                        break;

                                    case "PENDING":
                                        var pendingmodel = new PaymetIfoRequestViewModel
                                        {
                                            InvoiceNo = Request.Form["ORDER_ID"],
                                            PgResponse = null,
                                            PgType = pgType,
                                            PGTransactionId = null,
                                            Status = "pending",
                                        };
                                        _unit.IOrder.SaveCustomerPaymentInfo(pendingmodel);

                                        break;
                                }
                            }

                            redirectUrl = _appSettings.ReturnUrl + "?Status=" + paymentStatus;
                        }
                    }
                    break;
                    #endregion
            }
            Response.Redirect(redirectUrl);
        }

        public string PaymentResult()
        {
            string str = "Payment success";
            return str;
        }
        //POST FORM DATA
        public static string PostFormData(string url, Hashtable data)
        {
            //Set a name for the form
            string formID = "form1";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            strForm.Append("<script src=\"https://checkout.razorpay.com/v1/checkout.js\"");
            foreach (System.Collections.DictionaryEntry key in data)
            {
                strForm.Append(" " + key.Key + "=\"" + key.Value + "\"");
            }
            strForm.Append("></script></form>");

            return strForm.ToString();
        }

        /// <summary>
        /// BYTE TO HEXA CONVERSION
        /// </summary>
        /// <param name="byData"></param>
        /// <returns></returns>
        public static string byteToHexString(byte[] byData)
        {
            StringBuilder sb = new StringBuilder((byData.Length * 2));
            for (int i = 0; (i < byData.Length); i++)
            {
                int v = (byData[i] & 255);
                if ((v < 16))
                {
                    sb.Append('0');
                }

                sb.Append(v.ToString("X"));

            }

            return sb.ToString();
        }

        /// <summary>
        /// Post web request for revery transaction status
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public string PostWebRequest(string requestUrl, string jsonValue)
        {
            string responseData = string.Empty;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                httpWebRequest.Headers.Add("ContentType", "application/json");
                httpWebRequest.Method = "POST";
                using (StreamWriter requestWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    requestWriter.Write(jsonValue);
                }
                using (StreamReader responseReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.ApplicationTier, "PostWebRequest():" + ex.ToString());
            }
            return responseData;
        }
    }
}