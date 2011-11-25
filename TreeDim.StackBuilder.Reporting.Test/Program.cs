﻿#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using log4net;
using log4net.Config;

using TreeDim.StackBuilder.Basics;
using TreeDim.StackBuilder.Engine;
using TreeDim.StackBuilder.Graphics;
using TreeDim.StackBuilder.Reporting;
#endregion

namespace TreeDim.StackBuilder.Reporting.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Program));
            XmlConfigurator.Configure();

            try
            {
                // check arguments
                if (args.Length != 1)
                {
                    log.Info("No command argument. Exiting...");
                    return;
                }
                if (!File.Exists(args[0]))
                { 
                    log.Info(string.Format("File {0} could not be found. Exiting...", args[0]));
                    return;
                }

                string filePath = args[0];
                // load document
                Document doc = new Document(filePath,  new DocumentListenerLog());
                // get first analysis
                List<PalletAnalysis> analyses = doc.Analyses;
                if (analyses.Count == 0)
                {
                    log.Info("Document has no analysis -> Exiting...");
                    return;
                }
                // build output file path
                string outputFilePath = Path.ChangeExtension(Path.GetTempFileName(), "doc");
                string templatePath = @"..\..\..\TreeDim.StackBuilder.Reporting\ReportTemplates\";
                ReporterMSWord reporter = new ReporterMSWord();
                reporter.BuildAnalysisReport(new ReportData(analyses[0], analyses[0].GetSelSolutionBySolutionIndex(0)), templatePath, outputFilePath);

                Console.WriteLine("Saved report to: {0}", outputFilePath);

                // Display resulting report in Word
                Process.Start(new ProcessStartInfo(outputFilePath));                
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        #region DocumentListener -> Log
        internal class DocumentListenerLog : IDocumentListener
        {
            #region Data members
            static protected ILog _log = LogManager.GetLogger(typeof(Program));
            #endregion

            #region Override
            public void OnNewDocument(Document doc)
            {
                _log.Info(string.Format("Opened document {0}", doc.Name));
            }
            public void OnNewTypeCreated(Document doc, ItemBase itemBase)
            {
                _log.Info(string.Format("Loaded item {0}", itemBase.Name));
            }
            public void OnNewAnalysisCreated(Document doc, PalletAnalysis analysis)
            {
                _log.Info(string.Format("Loaded analysis {0}", analysis.Name));
            }
            public void OnNewCaseAnalysisCreated(Document doc, CaseAnalysis caseAnalysis)
            {
                _log.Info(string.Format("Loaded case analysis {0}", caseAnalysis.Name));
            }
            /*
            public void OnNewSolutionAdded(Document doc, PalletAnalysis analysis, SelSolution selectedSolution)
            {
                _log.Info(string.Format("Selected solution added : {0}", selectedSolution.Name));
            }
            public void OnNewCaseSolutionAdded(Document doc, CaseAnalysis analysis, SelCaseSolution selectedSolution)
            { 
                _log.Info(string.Format("Selected case solution added : {0}", selectedSolution.Name));
            }
            */ 
            public void OnNewTruckAnalysisCreated(Document doc, PalletAnalysis analysis, SelSolution selectedSolution, TruckAnalysis truckAnalysis)
            { 
            }
            public void OnNewECTAnalysisCreated(Document doc, PalletAnalysis analysis, SelSolution selectedSolution, ECTAnalysis ectAnalysis)
            { 
            }
            public void OnTypeRemoved(Document doc, ItemBase itemBase)
            { 
            }
            public void OnAnalysisRemoved(Document doc, PalletAnalysis analysis)
            { 
            }
            public void OnCaseAnalysisRemoved(Document doc, CaseAnalysis caseAnalysis)
            {
            }
            /*
            public void OnSolutionRemoved(Document doc, PalletAnalysis analysis, SelSolution selectedSolution)
            {
                _log.Info(string.Format("Selected solution removed {0}", selectedSolution.Name));
            }
            public void OnCaseAnalysisSolutionRemoved(Document doc, CaseAnalysis analysis, SelCaseSolution selectedSolution)
            {
                _log.Info(string.Format("Selected solution {0}", selectedSolution.Name));
            }
            */ 
            public void OnTruckAnalysisRemoved(Document doc, PalletAnalysis analysis, SelSolution selectedSolution, TruckAnalysis truckAnalysis)
            { 
            }
            public void OnECTAnalysisRemoved(Document doc, PalletAnalysis analysis, SelSolution selectedSolution, ECTAnalysis ectAnalysis)
            { 
            }
            public void OnDocumentClosed(Document doc)
            { 
            }
            #endregion
        }
        #endregion
    }
}