using AuthenticationService.Applicaion.Abstractions.Services;
using AuthenticationService.Infrastructure.Implementations.Services.Models;
using EmailClient;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Infrastructure.Implementations.Services
{
    class VerificationService(ILogger<VerificationService> logger, IOptions<VerificationOptions> options) : IVerificationService
    {
        private const string SUBJECT = "Подтверждение регистрации";

        public async Task VerificateEmail(string email)
        {

            using (var channel = GrpcChannel.ForAddress(options.Value.EmailServiceAddress))
            {
                var client = new Sender.SenderClient(channel);
                var request = new SendMailRequest { Body = GenerateBody(options.Value.ConfirmationPageLink), Subject = SUBJECT };
                request.To.Add(email);

                await client.SendMailAsync(request);
            }

        }

        private string GenerateBody(string confirmationLink)
        {
            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n" +
                "<html dir=\"ltr\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" lang=\"ru\">\r\n " +
                "<head>\r\n" +
                "  <meta charset=\"UTF-8\">\r\n" +
                "  <meta content=\"width=device-width, initial-scale=1\" name=\"viewport\">\r\n  " +
                "  <meta name=\"x-apple-disable-message-reformatting\">\r\n  " +
                "  <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n  " +
                "  <meta content=\"telephone=no\" name=\"format-detection\">\r\n  " +
                "  <title>Новое письмо</title><!--[if (mso 16)]>\r\n    " +
                "  <style type=\"text/css\">\r\n    " +
                "    a {text-decoration: none;}\r\n   " +
                "  </style>\r\n    " +
                "<![endif]--><!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]--><!--[if gte mso 9]>\r\n" +
                "<noscript>\r\n" +
                "         <xml>\r\n" +
                "           <o:OfficeDocumentSettings>\r\n" +
                "           <o:AllowPNG></o:AllowPNG>\r\n" +
                "           " +
                "<o:PixelsPerInch>96</o:PixelsPerInch>\r\n" +
                "           </o:OfficeDocumentSettings>\r\n" +
                "         </xml>\r\n" +
                "      </noscript>\r\n" +
                "<![endif]--><!--[if mso]><xml>\r\n" +
                "    <w:WordDocument xmlns:w=\"urn:schemas-microsoft-com:office:word\">\r\n" +
                "      <w:DontUseAdvancedTypographyReadingMail/>\r\n" +
                "    </w:WordDocument>\r\n    " +
                "</xml><![endif]-->\r\n" +
                "  <style type=\"text/css\">\r\n" +
                ".rollover:hover .rollover-first {\r\n" +
                "  max-height:0px!important;\r\n" +
                "  display:none!important;\r\n" +
                "}\r\n" +
                ".rollover:hover .rollover-second {\r\n" +
                "  max-height:none!important;\r\n" +
                "  display:block!important;\r\n" +
                "}\r\n" +
                ".rollover span {\r\n" +
                "  font-size:0px;\r\n" +
                "}\r\n" +
                "u + .body img ~ div div {\r\n" +
                "  display:none;\r\n" +
                "}\r\n" +
                "#outlook a {\r\n" +
                "  padding:0;\r\n" +
                "}\r\n" +
                "span.MsoHyperlink,\r\n" +
                "span.MsoHyperlinkFollowed {\r\n" +
                "  color:inherit;\r\n" +
                "  mso-style-priority:99;\r\n" +
                "}\r\n" +
                "a.es-button {\r\n" +
                "  mso-style-priority:100!important;\r\n" +
                "  text-decoration:none!important;\r\n" +
                "}\r\n" +
                "a[x-apple-data-detectors],\r\n" +
                "#MessageViewBody " + "a {\r\n  " +
                "  color:inherit!important;\r\n  " +
                "text-decoration:none!important;\r\n" +
                "  font-size:inherit!important;\r\n" +
                "  font-family:inherit!important;\r\n" +
                "  font-weight:inherit!important;\r\n" +
                "  line-height:inherit!important;\r\n" +
                "}\r\n" +
                ".es-desk-hidden {\r\n" +
                "  display:none;\r\n" +
                "  float:left;\r\n" +
                "  overflow:hidden;\r\n" +
                "  width:0;\r\n" +
                "  max-height:0;\r\n" +
                "  line-height:0;\r\n" +
                "  mso-hide:all;\r\n" +
                "}\r\n" +
                "@media only screen and (max-width:600px) {.es-m-p0r { padding-right:0px!important } .es-m-p0l { padding-left:0px!important } .es-p-default { } *[class=\"gmail-fix\"] { display:none!important } p, a { line-height:150%!important } h1, h1 a { line-height:120%!important } h2, h2 a { line-height:120%!important } h3, h3 a { line-height:120%!important } h4, h4 a { line-height:120%!important } h5, h5 a { line-height:120%!important } h6, h6 a { line-height:120%!important } .es-header-body p { } .es-content-body p { } .es-footer-body p { } .es-infoblock p { } h1 { font-size:36px!important; text-align:left } h2 { font-size:26px!important; text-align:left } h3 { font-size:20px!important; text-align:left } h4 { font-size:24px!important; text-align:left } h5 { font-size:20px!important; text-align:left } h6 { font-size:16px!important; text-align:left } .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a { font-size:36px!important } .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a { font-size:26px!important } .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a { font-size:20px!important } .es-header-body h4 a, .es-content-body h4 a, .es-footer-body h4 a { font-size:24px!important } .es-header-body h5 a, .es-content-body h5 a, .es-footer-body h5 a { font-size:20px!important } .es-header-body h6 a, .es-content-body h6 a, .es-footer-body h6 a { font-size:16px!important } .es-menu td a { font-size:12px!important } .es-header-body p, .es-header-body a { font-size:14px!important } .es-content-body p, .es-content-body a { font-size:16px!important } .es-footer-body p, .es-footer-body a { font-size:14px!important } .es-infoblock p, .es-infoblock a { font-size:12px!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3, .es-m-txt-c h4, .es-m-txt-c h5, .es-m-txt-c h6 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3, .es-m-txt-r h4, .es-m-txt-r h5, .es-m-txt-r h6 { text-align:right!important } .es-m-txt-j, .es-m-txt-j h1, .es-m-txt-j h2, .es-m-txt-j h3, .es-m-txt-j h4, .es-m-txt-j h5, .es-m-txt-j h6 { text-align:justify!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3, .es-m-txt-l h4, .es-m-txt-l h5, .es-m-txt-l h6 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-m-txt-r .rollover:hover .rollover-second, .es-m-txt-c .rollover:hover .rollover-second, .es-m-txt-l .rollover:hover .rollover-second { display:inline!important } .es-m-txt-r .rollover span, .es-m-txt-c .rollover span, .es-m-txt-l .rollover span { line-height:0!important; font-size:0!important; display:block } .es-spacer { display:inline-table } a.es-button, button.es-button { font-size:20px!important; padding:10px 20px 10px 20px!important; line-height:120%!important } a.es-button, button.es-button, .es-button-border { display:inline-block!important } .es-m-fw, .es-m-fw.es-fw, .es-m-fw .es-button { display:block!important } .es-m-il, .es-m-il .es-button, .es-social, .es-social td, .es-menu { display:inline-block!important } .es-adaptive table, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .adapt-img { width:100%!important; height:auto!important } .es-mobile-hidden, .es-hidden { display:none!important } .es-desk-hidden { width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } tr.es-desk-hidden { display:table-row!important } table.es-desk-hidden { display:table!important } td.es-desk-menu-hidden { display:table-cell!important } .es-menu td { width:1%!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } .h-auto { height:auto!important } .img-3060 { width:100px!important } }\r\n" +
                "" +
                "@media screen and (max-width:384px) {.mail-message-content { width:414px!important } }\r\n</style>\r\n </head>\r\n <body class=\"body\" style=\"width:100%;height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0\">\r\n" +
                "  <div dir=\"ltr\" class=\"es-wrapper-color\" lang=\"ru\" style=\"background-color:#FAFAFA\"><!--[if gte mso 9]>\r\n" +
                "\t\t\t<v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\">\r\n" +
                "\t\t\t\t<v:fill type=\"tile\" color=\"#fafafa\"></v:fill>\r\n" +
                "\t\t\t</v:background>\r\n" +
                "\t\t<![endif]-->\r\n" +
                "   <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" class=\"es-wrapper\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#FAFAFA\">\r\n" +
                "     <tr>\r\n" +
                "      <td valign=\"top\" style=\"padding:0;Margin:0\">\r\n" +
                "       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"es-content\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important\">\r\n" +
                "       </table>\r\n       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"es-header\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top\">\r\n         <tr>\r\n" +
                "          <td align=\"center\" bgcolor=\"#b4eeff\" style=\"padding:0;Margin:0;background-color:#b4eeff\">\r\n" +
                "           <table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"es-header-body\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px\">\r\n" +
                "             <tr>\r\n" +
                "              <td align=\"left\" style=\"Margin:0;padding-top:10px;padding-right:20px;padding-bottom:10px;padding-left:20px\">\r\n" +
                "               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n" +
                "                 <tr>\r\n" +
                "                  <td valign=\"top\" align=\"center\" class=\"es-m-p0r\" style=\"padding:0;Margin:0;width:560px\">\r\n" +
                "                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n" +
                "                     <tr>\r\n" +
                "                      <td align=\"center\" style=\"padding:0;Margin:0;font-size:0px\"><img src=\"https://ftiwnvo.stripocdn.email/content/guids/CABINET_5aec7edcfcf42d0a7602c655f8ef6bcdd9139caddb3e3a29be5104b210a7bff5/images/image_YPT.jpeg\" alt=\"\" width=\"150\" title=\"Logo\" class=\"img-3060\" style=\"display:block;font-size:12px;border:0;outline:none;text-decoration:none\"></td>\r\n" +
                "                     </tr>\r\n" +
                "                   </table></td>\r\n" +
                "                 </tr>\r\n" +
                "               </table></td>\r\n" +
                "             </tr>\r\n" +
                "           </table></td>\r\n" +
                "         </tr>\r\n" +
                "       </table>\r\n" +
                "       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"es-content\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important\">\r\n" +
                "         <tr>\r\n" +
                "          <td align=\"center\" style=\"padding:0;Margin:0\">\r\n" +
                "           <table bgcolor=\"#ffffff\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" class=\"es-content-body\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px\">\r\n" +
                "             <tr>\r\n" +
                "              <td align=\"left\" style=\"Margin:0;padding-right:20px;padding-left:20px;padding-top:90px;padding-bottom:30px\">\r\n" +
                "               <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n" +
                "                 <tr>\r\n" +
                "                  <td align=\"center\" valign=\"top\" style=\"padding:0;Margin:0;width:560px\">\r\n" +
                "                   <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px\">\r\n" +
                "                     <tr>\r\n" +
                "                      <td align=\"center\" style=\"padding:0;Margin:0;padding-bottom:10px\"><h1 class=\"es-m-txt-c\" style=\"Margin:0;font-family:arial, 'helvetica neue', helvetica, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:46px;font-style:normal;font-weight:bold;line-height:46px;color:#333333\">Confirm Your Email</h1></td>\r\n" +
                "                     </tr>\r\n" +
                "                     <tr>\r\n" +
                "                      <td align=\"center\" class=\"es-m-p0r es-m-p0l\" style=\"Margin:0;padding-top:5px;padding-right:40px;padding-bottom:5px;padding-left:40px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">You’ve received this message because your email address has been registered with our site. Please click the button below to verify your email address and confirm that you are the owner of this account.</p></td>\r\n" +
                "                     </tr>\r\n" +
                "                     <tr>\r\n" +
                "                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:5px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">If you did not register with us, please disregard this email.</p></td>\r\n" +
                "                     </tr>\r\n" +
                "                     <tr>\r\n" +
                $"                      <td align=\"center\" style=\"padding:0;Margin:0;padding-top:10px;padding-bottom:10px\"><span class=\"es-button-border\" style=\"border-style:solid;border-color:#2CB543;background:#1f77c4;border-width:0px;display:inline-block;border-radius:6px;width:auto\"><a href=\"{confirmationLink}\" target=\"_blank\" class=\"es-button\" style=\"mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:20px;padding:10px 30px 10px 30px;display:inline-block;background:#1f77c4;border-radius:6px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:24px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #1f77c4;padding-left:30px;padding-right:30px\">CONFIRM YOUR EMAIL</a></span></td>\r\n" +
                "                     </tr>\r\n" +
                "                     <tr>\r\n" +
                "                      <td align=\"center\" class=\"es-m-p0r es-m-p0l\" style=\"Margin:0;padding-top:5px;padding-right:40px;padding-bottom:5px;padding-left:40px\"><p style=\"Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px\">Once confirmed, this email will be uniquely associated with your account.</p></td>\r\n" +
                "                     </tr>\r\n" +
                "                   </table></td>\r\n" +
                "                 </tr>\r\n" +
                "               </table></td>\r\n" +
                "             </tr>\r\n" +
                "           </table></td>\r\n         </tr>\r\n       </table>\r\n" +
                "       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"es-footer\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top\">\r\n" +
                "       </table>\r\n" +
                "       <table cellpadding=\"0\" cellspacing=\"0\" align=\"center\" class=\"es-content\" role=\"none\" style=\"mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important\">\r\n" +
                "       </table></td>\r\n" +
                "     </tr>\r\n" +
                "   </table>\r\n" +
                "  </div>\r\n" +
                " </body>\r\n" +
                "</html>";

        }
    }

}
