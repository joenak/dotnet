using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using iTextSharp.text;
using iTextSharp.text.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace stmt_generator
{
  class Program
  {
      static void Main(string[] args)
      {
        Console.WriteLine("...starting generator...");
        try
        {
          string connString = @"Server=Server1;Database=Database1;Trusted_Connection=true";
          SqlConnection conn = new SqlConnection(connString);
          conn.Open();
          string tsql = @"SELECT * FROM Database1.dbo.Table1 (NOLOCK)";
          SqlCommand cmd = new SqlCommand(tsql, conn);
          SqlDataReader dr = cmd.ExecuteReader();
          while (dr.Read())
          {
            GenerateStatement(id, statementDate, statementData);
          }

          dr.Close();
          cmd.Dispose();
          conn.Close();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("...finished...");
        Console.Read();
      }

      public static void GenerateStatement(int id, DateTime statementDate, string statementData)
      {
        string fileName = string.Format(@"C:\output\Monthly_{0}_{1}.pdf", id, statementDate);
        Console.WriteLine(fileName);
        using (var output = new MemoryStream())
        {
          using (var pdfDocument = new Document(PageSize.LETTER))
          {
            var pdfWriter = PdfWriter.GetInstance(pdfDocument, output);
            pdfWriter.CloseStream = false;
          }

          byte[] b = output.ToArray();

          using (FileStream fs = File.Create(fileName))
          {
            fs.Write(b, 0, (int)b.Length)
          }
        }
      }
  }
}
