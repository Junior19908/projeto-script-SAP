using System;
using System.IO;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class CabecalhoPDFGenerator
{
    private readonly XmlDocument _xmlDoc;
    private readonly XmlNamespaceManager _namespaceManager;

    public CabecalhoPDFGenerator(string xmlFilePath)
    {
        _xmlDoc = new XmlDocument();
        _xmlDoc.Load(xmlFilePath);

        _namespaceManager = new XmlNamespaceManager(_xmlDoc.NameTable);
        _namespaceManager.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
    }

    public void GeneratePDF(string pdfFilePath)
    {
        // Extrair informações do cabeçalho
        string versao = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:ide/nfe:versao");
        string cUF = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:ide/nfe:cUF");
        string cNF = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:ide/nfe:cNF");
        string natOp = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:ide/nfe:natOp");

        // Extrair informações do emitente
        string CNPJ_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:CNPJ");
        string xNome_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:xNome");
        string xLgr_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:enderEmit/nfe:xLgr");
        string nro_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:enderEmit/nfe:nro");
        string xBairro_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:enderEmit/nfe:xBairro");
        string cMun_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:enderEmit/nfe:cMun");
        string xMun_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:enderEmit/nfe:xMun");
        string UF_emitente = ExtractHeaderValue("nfe:NFe/nfe:infNFe/nfe:emit/nfe:enderEmit/nfe:UF");

        // Criação do documento PDF
        Document document = new Document();
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFilePath, FileMode.Create));
        document.Open();

        // Criação da tabela
        PdfPTable table = new PdfPTable(2);

        // Estilo da tabela
        table.DefaultCell.Border = Rectangle.NO_BORDER;
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 1f, 3f });

        // Adicionar células com os dados do cabeçalho
        AddCell(table, "Versão");
        AddCell(table, versao);
        AddCell(table, "cUF");
        AddCell(table, cUF);
        AddCell(table, "cNF");
        AddCell(table, cNF);
        AddCell(table, "natOp");
        AddCell(table, natOp);
        AddCell(table, "CNPJ emitente");
        AddCell(table, CNPJ_emitente);
        AddCell(table, "Nome emitente");
        AddCell(table, xNome_emitente);
        AddCell(table, "Endereço emitente");
        AddCell(table, $"{xLgr_emitente}, {nro_emitente}, {xBairro_emitente}, {cMun_emitente}, {xMun_emitente}, {UF_emitente}");

        // Adicionar tabela ao documento
        document.Add(table);

        // Fechar o documento
        document.Close();

        Console.WriteLine("Cabeçalho em PDF criado com sucesso.");
    }

    private string ExtractHeaderValue(string xpath)
    {
        XmlNode node = _xmlDoc.SelectSingleNode(xpath, _namespaceManager);
        return node?.InnerText;
    }

    private void AddCell(PdfPTable table, string text)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text));
        cell.Padding = 5;
        table.AddCell(cell);
    }
}
