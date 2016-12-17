using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

static class HTTPS
{
    private static CookieContainer cookies = new CookieContainer();
    private static bool connected = false;

    private static bool OnValidationCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

    public static string HttpGet(string URI)
    {
        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security
        .RemoteCertificateValidationCallback(OnValidationCallback);
        System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(URI);
        req.CookieContainer = cookies;
        System.Net.WebResponse resp = req.GetResponse();
        System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
        return sr.ReadToEnd().Trim();
    }



    private static string HttpPost(string URI, string Parameters)
    {
        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(OnValidationCallback);
        System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(URI);
        req.CookieContainer = cookies;
        req.ContentType = "application/x-www-form-urlencoded";
        req.Method = "POST";
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
        req.ContentLength = bytes.Length;
        System.IO.Stream os = req.GetRequestStream();
        os.Write(bytes, 0, bytes.Length);
        os.Close();
        System.Net.WebResponse resp = req.GetResponse();
        if (resp == null) return null;
        System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
        return sr.ReadToEnd().Trim();
    }
