#pragma checksum "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bbdc36f3677488dfbb6ae418034629857cbe73d3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Logout), @"mvc.1.0.view", @"/Views/Shared/Logout.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/Logout.cshtml", typeof(AspNetCore.Views_Shared_Logout))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
using AspNet.Security.OpenIdConnect.Primitives;

#line default
#line hidden
#line 2 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
using System.Security.Claims;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bbdc36f3677488dfbb6ae418034629857cbe73d3", @"/Views/Shared/Logout.cshtml")]
    public class Views_Shared_Logout : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Tuple<OpenIdConnectRequest, ClaimsPrincipal>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(78, 1, true);
            WriteLiteral("\n");
            EndContext();
            BeginContext(131, 116, true);
            WriteLiteral("\n<div class=\"jumbotron\">\n    <h1>Log out</h1>\n    <p class=\"lead text-left\">Are you sure you want to sign out?</p>\n\n");
            EndContext();
#line 10 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
     if (Model.Item2 != null && Model.Item2.Identity != null) {

#line default
#line hidden
            BeginContext(311, 50, true);
            WriteLiteral("        <p class=\"lead text-left\">Connected user: ");
            EndContext();
            BeginContext(362, 25, false);
#line 11 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
                                             Write(Model.Item2.Identity.Name);

#line default
#line hidden
            EndContext();
            BeginContext(387, 5, true);
            WriteLiteral("</p>\n");
            EndContext();
#line 12 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
    }

#line default
#line hidden
            BeginContext(398, 103, true);
            WriteLiteral("\n    <form action=\"/connect/logout\" enctype=\"application/x-www-form-urlencoded\" method=\"post\">\n        ");
            EndContext();
            BeginContext(502, 23, false);
#line 15 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
   Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(525, 2, true);
            WriteLiteral("\n\n");
            EndContext();
#line 17 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
         foreach (var parameter in Model.Item1.GetParameters()) {

#line default
#line hidden
            BeginContext(593, 32, true);
            WriteLiteral("            <input type=\"hidden\"");
            EndContext();
            BeginWriteAttribute("name", " name=\"", 625, "\"", 646, 1);
#line 18 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
WriteAttributeValue("", 632, parameter.Key, 632, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("value", " value=\"", 647, "\"", 671, 1);
#line 18 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
WriteAttributeValue("", 655, parameter.Value, 655, 16, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(672, 4, true);
            WriteLiteral(" />\n");
            EndContext();
#line 19 "C:\source\Kramerica.BankID\Kramerica.BankID.OIDCServer\Views\Shared\Logout.cshtml"
        }

#line default
#line hidden
            BeginContext(686, 118, true);
            WriteLiteral("\n        <input class=\"btn btn-lg btn-success\" name=\"Authorize\" type=\"submit\" value=\"Yeah, sure\" />\n    </form>\n</div>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Tuple<OpenIdConnectRequest, ClaimsPrincipal>> Html { get; private set; }
    }
}
#pragma warning restore 1591
