using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using GhostscriptSharp;
using GhostscriptSharp.Settings;

namespace DefaultNamespace
{
    public class ReceiptCreator : MonoBehaviour
    {
        private static string _uniqueCode;
        private static string _pdfName;
        private static string _path;

        public static void CreateReceiptPdf(Dictionary<string, string> basketMap, List<Dictionary<string,string>> itemMetaList)
        {
            if(string.IsNullOrEmpty(_pdfName))
            {
                _pdfName = "Receipt";
                CreatePdf (_pdfName, basketMap, itemMetaList); 
            }
            else
            {
                CreatePdf(_pdfName, basketMap, itemMetaList);
            }
        }
   
        private static void OnSubmit(string text)
        {
            _pdfName = text;
        }
   
        private static void CreatePdf(string fileName, Dictionary<string, string> basketMap, List<Dictionary<string,string>> itemMetaList)
        {
            var doc = new Document(new Rectangle(225, 460));
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(@"C:\YR\Receipt\" + fileName + ".pdf", FileMode.Create ));
            }
            catch(System.Exception e)
            {
                Debug.LogError("Exception, PdfWriter.GetInstance");
            }

            float totalPrice = 0;
            foreach (var map in itemMetaList)
            {
                foreach (var pair in map)
                {
                    if (pair.Key == "ITEM_PRICE")
                    {
                        float.TryParse(pair.Value, out var itemPrice);
                        totalPrice += itemPrice;
                    }
                }
            }
            
            doc.Open();
            var par1 = new Paragraph(basketMap["UNIQUE_CODE"] + "\n") {Alignment = Element.ALIGN_CENTER};
            var chunks = new List<Chunk>()
            {
                new Chunk(basketMap["COLLECTION_NAME"] + " " + basketMap["SURNAME"] + "\n"),
                new Chunk("Total Items: " + itemMetaList.Count  + "\n"),
                new Chunk("Total Price: " + totalPrice + "\n"),
            };
            
            for (int i = 0; i < itemMetaList.Count; i++)
            {
                chunks.Add(new Chunk("----------------" + "\n"));
                chunks.Add(new Chunk((i + 1) + " of " + itemMetaList.Count + "\n"));
                chunks.Add(new Chunk(itemMetaList[i]["UNIQUE_CODE"] + "\n"));
                chunks.Add(new Chunk(itemMetaList[i]["ARTWORK_DETAILS"] + "\n"));
                // BARCODE
                chunks.Add(new Chunk(itemMetaList[i]["ITEM_UPC"] + "\n"));
                // Product details
                chunks.Add(new Chunk(itemMetaList[i]["ITEM_PRICE_CURRENCY"] + "" + itemMetaList[i]["ITEM_PRICE"] + "\n"));
            }
            chunks.Add(new Chunk("----------------" + "\n"));
            chunks.Add(new Chunk(itemMetaList[0]["TIMESTAMP"] + "\n"));

            for (int i = 0; i < chunks.Count; i++)
            {
                par1.Add(chunks[i]);
            }

            doc.Add(par1);
            
            doc.Close ();
            
            PdfToJpg(@"C:\YR\Receipt\" + fileName + ".pdf", fileName, GhostscriptDevices.jpeg, GhostscriptPageSizes.b5, 200, 200);
        }

        
        private static void PdfToJpg(string path, string fileName, GhostscriptDevices devise, GhostscriptPageSizes pageFormat, int qualityX, int qualityY)
        {
            var settingsForConvert = new GhostscriptSettings {Device = devise};
            var pageSize = new GhostscriptPageSize {Native = pageFormat};
            settingsForConvert.Size = pageSize;
            settingsForConvert.Resolution = new System.Drawing.Size(qualityX, qualityY);
            
            GhostscriptWrapper.GenerateOutput(path, @"C:\YR\Receipt\" + fileName + "_" + ".jpg", settingsForConvert); // here you could set path and name for out put file.
        }
   
        public string GetPdfName()
        {
            return _pdfName;
        }
    }
}