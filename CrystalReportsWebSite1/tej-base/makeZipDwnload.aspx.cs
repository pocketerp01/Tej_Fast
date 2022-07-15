using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
//using Ionic.Zip;
using System.Web.UI.WebControls;
//using Ionic.Zlib;

public partial class makeZipDwnload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = Session["FileName"].ToString();
        string filePaths = Session["FilePath"].ToString();

        //using (ZipFile zip = new ZipFile())
        //{
        //    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //    zip.CompressionLevel = CompressionLevel.None;
        //    zip.AddDirectoryByName("Files");
        //    foreach (string str in filePath.Split(','))
        //    {
        //        string zfilePath = str;
        //        zip.AddFile(zfilePath, "Files");
        //    }
        //    Response.Clear();
        //    Response.BufferOutput = false;
        //    string zipName = String.Format("{0}.zip", fileName + "_" + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss"));
        //    Response.ContentType = "application/zip";
        //    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        //    zip.Save(Response.OutputStream);
        //    //Response.End();
        //    Response.Close();
        //}

        string zipName = String.Format("{0}", fileName + "_" + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss"));
        Response.AddHeader("Content-Disposition", "attachment; filename=" + zipName + ".zip");
        Response.ContentType = "application/zip";

        //using (var zipStream = new Ionic.Zip.ZipOutputStream(Response.OutputStream))
        //{
        //    foreach (string filePath in filePaths.Split(','))
        //    {
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        //        var fileEntry = new ZipEntry(Path.GetFileName(filePath))
        //        {
        //            Size = fileBytes.Length
        //        };

        //        zipStream.PutNextEntry(fileEntry.Name);
        //        zipStream.Write(fileBytes, 0, fileBytes.Length);
        //    }

        //    zipStream.Flush();
        //    zipStream.Close();
        //}

        Page.ClientScript.RegisterStartupScript(this.GetType(), "CloseScript", "window.close();", true);
    }
}