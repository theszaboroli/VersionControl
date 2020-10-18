﻿using _6.gyak.Entities;
using _6.gyak.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace _6.gyak
{
    public partial class Form1 : Form
    { BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<String> Currencies = new BindingList<string>();
        public string rslt;
       
        public Form1()
        {
            InitializeComponent();

            GetCurrencies();
            RefreshData();

        }

        private void GetCurrencies()
        {
            var mnbService2 = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody();
            var response = mnbService2.GetCurrencies(request);
            var result = response.GetCurrenciesResult;
            var xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    var childElement = (XmlElement)element.ChildNodes[i];
                    Currencies.Add(childElement.InnerText);
                }
            }
        }

        private void RefreshData()
        {
            Rates.Clear();
            GetExchangeRates();
            dataGridView1.DataSource = Rates;

            GetXML();

            GetDiagram();
            comboBox1.DataSource = Currencies;
            
        }

        private void GetDiagram()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void GetXML()
        {
            
            var xml = new XmlDocument();
            xml.LoadXml(rslt);

           
            foreach (XmlElement element in xml.DocumentElement)
            {
               
                var rate = new RateData();
                Rates.Add(rate);

              
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

               
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }

        private void GetExchangeRates()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString(),
            };

          
            var response = mnbService.GetExchangeRates(request);

         
            var result = response.GetExchangeRatesResult;
            rslt = result;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
