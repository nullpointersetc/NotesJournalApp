#region "NotesAspFrontEnd/Stylesheet.cs"
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using HttpLoggingFields =
    Microsoft.AspNetCore.HttpLogging.HttpLoggingFields;

//BOOM
//BOOM
//BOOM

using AuthenticationHeaderValue =
    System.Net.Http.Headers.AuthenticationHeaderValue;

using List_of_NoteResponse =
    System.Collections.Generic.List<
        NullPointersEtc.NotesJournalApp.NotesAspFrontEnd.NoteResponse>;

using HttpStatusCode = System.Net.HttpStatusCode;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesAspFrontEnd;

public static class Stylesheet
{
    public static string PageStyles()
        => "<style>" +
            ":root {" +
            "   font-family: Georgia, serif;" +
            "   color: #173f43;" +
            "   background: #f4f0e6;" +
            "} " +
            "* {" +
            "   box-sizing: border-box;" +
            "} " +
            "body {" +
            "   margin: 0;" +
            "   min-height: 100vh;" +
            "   background: linear-gradient(135deg, #f4f0e6, #dce9df);" +
            "} " +
            ".login-shell {" +
            "   min-height: 100vh;" +
            "   display: grid;" +
            "   place-items: center;" +
            "   padding: 24px;" +
            "} " +
            ".login-panel {" +
            "   width: min(100%, 440px);" +
            "   padding: 48px;" +
            "   background: #fffdf7;" +
            "   border-left: 7px solid #df6b42;" +
            "   box-shadow: 12px 12px 0 #b5cfc3;" +
            "} " +
            "h1 {" +
            "   margin: 0 0 10px;" +
            "   font-size: clamp(2.4rem, 7vw, 4.5rem);" +
            "   line-height: .95;" +
            "} " +
            ".muted {" +
            "   color: #58706c;" +
            "} " +
            ".eyebrow {" +
            "   margin: 0 0 20px;" +
            "   color: #df6b42;" +
            "   font: bold 0.8rem/1.2 Arial, sans-serif;" +
            "   letter-spacing: .12em;" +
            "   text-transform: uppercase;" +
            "} " +
            "label {" +
            "   display: block;" +
            "   margin: 24px 0 7px;" +
            "   font: bold .85rem Arial, sans-serif;" +
            "} " +
            "input {" +
            "   width: 100%;" +
            "   padding: 13px;" +
            "   border: 1px solid #a9c0b6;" +
            "   background: #f4f0e6;" +
            "   font: 1rem Georgia, serif;" +
            "} " +
            "button {" +
            "   margin-top: 26px;" +
            "   padding: 13px 20px;" +
            "   border: 0;" +
            "   background: #173f43;" +
            "   color: white;" +
            "   font: bold .9rem Arial, sans-serif;" +
            "   cursor: pointer;" +
            "} " +
            ".secondary {" +
            "   margin: 0;" +
            "   background: transparent;" +
            "   color: #173f43;" +
            "   border: 1px solid #173f43;" +
            "} " +
            ".error {" +
            "   padding: 10px;" +
            "   color: #a43d2d;" +
            "   background: #f9ddd2;" +
            "} " +
            ".notes-shell {" +
            "   max-width: 1180px;" +
            "   margin: auto;" +
            "   padding: 48px 24px;" +
            "} " +
            "header {" +
            "   display: flex;" +
            "   justify-content: space-between;" +
            "   align-items: end;" +
            "   margin-bottom: 36px;" +
            "} " +
            "table {" +
            "   width: 100%;" +
            "   border-collapse: collapse;" +
            "   background: #fffdf7;" +
            "} " +
            "th, td {" +
            "   padding: 16px;" +
            "   border-bottom: 1px solid #d3ded6;" +
            "   text-align: left;" +
            "   vertical-align: top;" +
            "} " +
            "th {" +
            "   background: #173f43;" +
            "   color: white;" +
            "   font: bold .8rem Arial, sans-serif;" +
            "   text-transform: uppercase;" +
            "} " +
            ".empty {" +
            "   text-align: center;" +
            "   color: #58706c;" +
            "} " +
            "@media (max-width: 700px) {" +
            "   .login-panel {" +
            "       padding: 32px 24px;" +
            "   } " +
            "   header {" +
            "       align-items: start;" +
            "       gap: 20px;" +
            "   }" +
            "   table {" +
            "       display: block;" +
            "       overflow-x: auto;" +
            "       white-space: nowrap;" +
            "   } " +
            "} " +
            "</style>";
}
#endregion "NotesAspFrontEnd/Stylesheet.cs"
