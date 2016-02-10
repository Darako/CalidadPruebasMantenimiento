using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;
using DTFramework.Web.Test;
using DTFramework.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using RecogerAgenciasParaguay.Datos;
using System.Net;

namespace RecogerCPInternacionales
{
    [TestFixture]
    class RecolectorAgencias
    {

        private const int TIEMPO_ESPERA = 5000;
		 private const string urlBase = "http://postcodebase.com/";
		 private const string urlTraduccionBase = "http://www.wordreference.com/es/translation.asp?tranword=";
		 private const string urlGoogleTraduccionBase = "https://translate.google.es/#en/es/";
		 private const string urlGoogleMapsBase = "https://www.google.es/maps/place/";
		 private const int INTERVALOR_GUARDAR_CP = 10;

		 private FileLogs dtLog = new FileLogs();

		 private IWebDriver driver;
		 private IWebDriver driverTraduccion;
         private WebDriverWait driverWait;
         private WebDriverWait driverWaitTraduccion;
        private StringBuilder verificationErrors;

        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(1, 0, 0));
            driverWait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(30));

            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
				{
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
		  public void RecogerAgenciasParaguay()
        {
            driver.Navigate().GoToUrl("http://www.registurparaguay.gov.py/ListadoLocales.aspx");
            SelectElement servicio = new SelectElement(driver.FindElement(By.Id("cpContenido_ddlServicios")));
            servicio.SelectByIndex(6);
            driver.FindElement(By.Id("cpContenido_btnBuscar")).Click();
            Thread.Sleep(TIEMPO_ESPERA);
            List<Agencia> listadoAgencias = new List<Agencia>();

            int paginaActual = 1;
            bool haySiguientePagina = true;

            while(haySiguientePagina)
            {
                List<IWebElement> filas = driver.FindElements(By.CssSelector("#cpContenido_gvLocales table")).ToList<IWebElement>();
                if (null != filas && 2 < filas.Count)
                {
                    int numeroAgenciasPagina = filas.Count - 2; // Se quita la cabecera y la paginación
                    int numeroAgencia = 0;
                    while (numeroAgencia < numeroAgenciasPagina)
                    {
                        if (0 == driver.FindElements(By.Id("cpContenido_gvLocales_lbVerLocal_" + numeroAgencia)).Count)
                            Thread.Sleep(TIEMPO_ESPERA);
                        driver.FindElement(By.Id("cpContenido_gvLocales_lbVerLocal_" + numeroAgencia)).Click();
                        if (0 == driver.FindElements(By.Id("cpContenido_btnVolver")).Count)
                            Thread.Sleep(TIEMPO_ESPERA);
                        Agencia agencia = new Agencia();
                        agencia.insertarDatosParaguay(driver);
                        listadoAgencias.Add(agencia);
                        driver.FindElement(By.Id("cpContenido_btnVolver")).Click();
                        numeroAgencia++;
                    }
                }
                paginaActual++;
                haySiguientePagina = 0 < driver.FindElements(By.LinkText("" + paginaActual)).Count;
                if (haySiguientePagina) { 
                    driver.FindElement(By.LinkText("" + paginaActual)).Click();
                    Thread.Sleep(TIEMPO_ESPERA);
                }
            }
            
            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;A5;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVParaguay() + "\n";
            }
            File.WriteAllText("AgenciasParaguay.csv", csv,Encoding.UTF8);
        }


        [Test]
        public void RecogerAgenciasACAVE()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();

            int paginaActual = 1;
            bool haySiguientePagina = true;

            IWebDriver driverAgencia = new FirefoxDriver();
            driverAgencia.Manage().Window.Maximize();
            driverAgencia.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(1, 0, 0));
            while (haySiguientePagina)
            {

                driver.Navigate().GoToUrl("http://www.acave.travel/es/agencias?&&title=&field_llicencia_value=&name_list=All&field_postal_code_multiple=&field_province_multiple=&page=" + (paginaActual - 1));
                
                Acciones.EsperarCargaPagina(driver);
                List<IWebElement> agenciasEnPagina = driver.FindElements(By.ClassName("views-field-title")).ToList<IWebElement>();
                if (null != agenciasEnPagina && 1 < agenciasEnPagina.Count)
                {
  agenciasEnPagina.Remove(agenciasEnPagina.First());
                    foreach (IWebElement enlaceAgencia in agenciasEnPagina)
                    {
                        if (0 < enlaceAgencia.FindElements(By.TagName("a")).Count)
                        {
                            driverAgencia.Navigate().GoToUrl(enlaceAgencia.FindElement(By.TagName("a")).GetAttribute("href"));
                            Acciones.EsperarCargaPagina(driverAgencia);
                            Agencia agencia = new Agencia();
                            agencia.insertarDatosACAVE(driverAgencia);
                            listadoAgencias.Add(agencia);
                        }
                    }
                }
                paginaActual++;
                haySiguientePagina = 0 < driver.FindElements(By.LinkText("" + paginaActual)).Count;
                if (haySiguientePagina)
                {
                    driver.FindElement(By.LinkText("" + paginaActual)).Click();
                    Acciones.EsperarCargaPagina(driver);
                }
            }
            driverAgencia.Close();

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVACAVE() + "\n";
            }
            File.WriteAllText("AgenciasACAVE.csv", csv, Encoding.UTF8);
        }
        [Test]
        public void RecogerAgenciasAV()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();

            int paginaActual = 1;
            bool haySiguientePagina = true;

            driver.Navigate().GoToUrl("http://www.aavfgl.org/index.php?option=com_flexicontent&view=category&cid=49&Itemid=37");
            Acciones.EsperarCargaPagina(driver);
            
            IWebDriver driverAgencia = new FirefoxDriver();
            driverAgencia.Manage().Window.Maximize();
            driverAgencia.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(1, 0, 0));
            while (haySiguientePagina)
            {
                List<IWebElement> agenciasEnPagina = driver.FindElements(By.CssSelector("ul.introblock.one li")).ToList<IWebElement>();
                if (null != agenciasEnPagina && 1 < agenciasEnPagina.Count)
                {
                    foreach (IWebElement enlaceAgencia in agenciasEnPagina)
                    {
                        if (0 < enlaceAgencia.FindElements(By.LinkText("Read more...")).Count)
                        {
                            driverAgencia.Navigate().GoToUrl(enlaceAgencia.FindElement(By.LinkText("Read more...")).GetAttribute("href"));
                            Acciones.EsperarCargaPagina(driverAgencia);
                            Agencia agencia = new Agencia();
                            agencia.insertarDatosAV(driverAgencia);
                            listadoAgencias.Add(agencia);
                        }
                    }
                }
                paginaActual++;
                haySiguientePagina = 0 < driver.FindElements(By.LinkText("" + paginaActual)).Count;
                if (haySiguientePagina)
                {
                    driver.FindElement(By.LinkText("" + paginaActual)).Click();
                    Acciones.EsperarCargaPagina(driver);
                }
            }
            driverAgencia.Close();

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVAV() + "\n";
            }
            File.WriteAllText("AgenciasAV.csv", csv, Encoding.UTF8);
        }


        [Test]
        public void RecogerAgenciasAAVOT()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();

            bool haySiguientePagina = true;

            driver.Navigate().GoToUrl("http://aavot.es/portfolio_page/viajes-britour/");
            Acciones.EsperarCargaPagina(driver);

            while (haySiguientePagina)
            {
                Thread.Sleep(5000);
                Agencia agencia = new Agencia();
                agencia.insertarDatosAAVOT(driver);
                listadoAgencias.Add(agencia);

                haySiguientePagina = 0 < driver.FindElements(By.CssSelector("div.portfolio_next a")).Count;
                if (haySiguientePagina)
                {
                    driver.FindElement(By.CssSelector("div.portfolio_next a")).Click();
                    Acciones.EsperarCargaPagina(driver);
                }
            }

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVAAVOT() + "\n";
            }
            File.WriteAllText("AgenciasAAVOT.csv", csv, Encoding.UTF8);
        }

        [Test]
        public void RecogerAgenciasAEDAV()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();

            driver.Navigate().GoToUrl("http://www.aedav-andalucia.org/secciones/listado.php");
            Acciones.EsperarCargaPagina(driver);

            List<IWebElement> contenidos = driver.FindElements(By.CssSelector("tr.formulariosRow1 , tr.formulariosRow2")).ToList<IWebElement>();
            
            if (0 < contenidos.Count)
            {
                int contador = contenidos.Count-1;
                while (0 <= contador)
                {
                    contenidos = driver.FindElements(By.CssSelector("tr.formulariosRow1 , tr.formulariosRow2")).ToList<IWebElement>();
                    IWebElement elementoContenido = contenidos[contador];
                    elementoContenido.Click();
                    
                    Acciones.EsperarCargaPagina(driver);
                    //Thread.Sleep(5000);
                    Agencia agencia = new Agencia();
                    agencia.insertarDatosAEDAV(driver);
                    listadoAgencias.Add(agencia);

                    driver.FindElement(By.LinkText("Volver")).Click();
                    Acciones.EsperarCargaPagina(driver);
                    contador--;
                    //driver.Navigate().Refresh();
                }
            }

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVAEDAV() + "\n";
            }
            File.WriteAllText("AgenciasAEDAV.csv", csv, Encoding.UTF8);
        }


        [Test]
        public void RecogerAgenciasAPAV()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();

            Acciones.EsperarCargaPagina(driver);
            bool seguirRecorriendoPaginas = true;
            int contadorPaginas = 1;
            while (seguirRecorriendoPaginas)
            {
                driver.Navigate().GoToUrl("http://www.apavtenerife.com/web/es/5-asociados-apav.php?cat=0&keyword=&page=" + contadorPaginas);
                Acciones.EsperarCargaPagina(driver);
                List<IWebElement> contenidos = driver.FindElements(By.CssSelector("div#pasoc")).ToList<IWebElement>();
                contadorPaginas++;
                if (0 < contenidos.Count)
                {
                    int contador = contenidos.Count - 1;
                    while (0 <= contador)
                    {
                        IWebElement elementoContenido = contenidos[contador];
                        Agencia agencia = new Agencia();
                        agencia.insertarDatosAPAV(elementoContenido);
                        listadoAgencias.Add(agencia);
                        contador--;
                    }
                }
                else 
                {
                    seguirRecorriendoPaginas = false;
                }
            }
            

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVAPAV() + "\n";
            }
            File.WriteAllText("AgenciasAPAV.csv", csv, Encoding.UTF8);
        }

        [Test]
        public void RecogerAgenciasFETAVE()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();
            List<Agencia> listadoAgenciasVacias = new List<Agencia>();
            driver.Navigate().GoToUrl("http://fetave.es/agencias/busqueda-agencias.shtm");
            Acciones.EsperarCargaPagina(driver);
            driver.FindElement(By.Name("BUSCAR")).Click();
            Acciones.EsperarCargaPagina(driver);

            //Estas lineas se incluyen para hacer un caso de prueba mas corto que acceda directamente a la última página de los listados
            //driver.Navigate().GoToUrl("http://fetave.es/agencias/lista-agencias.shtm?pag=3");
            //Acciones.EsperarCargaPagina(driver);

            bool seguirRecorriendoPaginas = true;
            bool primeraVuelta = true;
            int contadorPaginas = 1;
            while (seguirRecorriendoPaginas)
            {
                if (!primeraVuelta)
                {   
                    driver.Navigate().GoToUrl("http://fetave.es/agencias/lista-agencias.shtm?pag=" + contadorPaginas);
                    Acciones.EsperarCargaPagina(driver);
                }
                primeraVuelta = false;

                List<IWebElement> contenidos = driver.FindElements(By.CssSelector("form table tr+tr")).ToList<IWebElement>();
                
                if (0 < contenidos.Count)
                {
                    int contador = contenidos.Count - 1;
                    IWebDriver driverConcreto = new FirefoxDriver();
                    while (0 <= contador)
                    {
                        contenidos = driver.FindElements(By.CssSelector("form table tr+tr")).ToList<IWebElement>();
                        
                        IWebElement elementoContenido = contenidos[contador];
                        
                        string url = elementoContenido.GetAttribute("onclick");
                        int posicionInicial = url.IndexOf("(")+2;
                        url = url.Remove(0,posicionInicial);
                        int posicionFinal = url.IndexOf(")")-1;
                        url = url.Remove(posicionFinal, (url.Length-posicionFinal) );

                         
                        driverConcreto.Navigate().GoToUrl(url);
                        Acciones.EsperarCargaPagina(driverConcreto);
                        
                        Agencia agencia = new Agencia();
                        agencia.insertarDatosFETAVE(driverConcreto);
                        agencia.web = url;
                        if (agencia.todosVacios) 
                        {
                            List<IWebElement> tdToList = elementoContenido.FindElements(By.CssSelector("td")).ToList<IWebElement>();
                            agencia.razonSocial = tdToList[0].Text;
                            listadoAgenciasVacias.Add(agencia);
                        }
                        else 
                        {
                            listadoAgencias.Add(agencia);
                        }
                        
                        contador--;

                        
                    }
                    driverConcreto.Close();
                }
                else
                {
                    seguirRecorriendoPaginas = false;
                }
                contadorPaginas++;
            }


            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVFETAVE() + "\n";
            }
            File.WriteAllText("AgenciasFETAVE.csv", csv, Encoding.UTF8);

            string nombreYUrl = "A continuación se muestran las agencias vacías (Razón Social - Url Web)\n";
            foreach (Agencia agencia in listadoAgenciasVacias)
            {
                nombreYUrl += "Razón Social: "+ agencia.razonSocial + "  - URL: " + agencia.web + "\n";
            }
            File.WriteAllText("AgenciasVaciasFETAVE.txt", nombreYUrl, Encoding.UTF8);
        }


        //[Test]
        //public void RecogerAgenciasAEVISE()
        //{
        //    List<Agencia> listadoAgencias = new List<Agencia>();

        //    driver.Navigate().GoToUrl("http://aevise.eu/?q=nuestros_asociados");
        //    Acciones.EsperarCargaPagina(driver);

        //    List<IWebElement> contenidos = driver.FindElements(By.CssSelector("div.view-content a")).ToList<IWebElement>();
            
        //    if (0 < contenidos.Count)
        //    {
        //        int contador = contenidos.Count-1;
        //        while (0 <= contador)
        //        {
        //            contenidos = driver.FindElements(By.CssSelector("div.view-content a")).ToList<IWebElement>();
        //            IWebElement elementoContenido = contenidos[contador];
        //            elementoContenido.Click();
                    
        //            Acciones.EsperarCargaPagina(driver);
        //            //Thread.Sleep(5000);
        //            Agencia agencia = new Agencia();
        //            agencia.insertarDatosAEDAV(driver);
        //            listadoAgencias.Add(agencia);

        //            driver.FindElement(By.LinkText("Volver")).Click();
        //            Acciones.EsperarCargaPagina(driver);
        //            contador--;
        //            //driver.Navigate().Refresh();
        //        }
        //    }

        //    string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
        //    foreach (Agencia agencia in listadoAgencias)
        //    {
        //        csv += agencia.getCSVAEDAV() + "\n";
        //    }
        //    File.WriteAllText("AgenciasAEVISE.csv", csv, Encoding.UTF8);
        //}



        [Test]
        public void RecogerAgenciasSpainDMCS()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();

            driver.Navigate().GoToUrl("http://www.spaindmcs.com/es/miembros-asociados");
            Acciones.EsperarCargaPagina(driver);

            List<IWebElement> contenidos = driver.FindElements(By.CssSelector("div#miembrosasociados a")).ToList<IWebElement>();

            if (0 < contenidos.Count)
            {
                int contador = contenidos.Count - 1;
                while (0 <= contador)
                {
                    contenidos = driver.FindElements(By.CssSelector("div#miembrosasociados a")).ToList<IWebElement>();
                    IWebElement elementoContenido = contenidos[contador];
                    //elementoContenido.Click();
                    string url = elementoContenido.GetAttribute("href");
                    IWebDriver driverConcreto = new FirefoxDriver();
                    driverConcreto.Navigate().GoToUrl(url);
                    Acciones.EsperarCargaPagina(driverConcreto);

                    //Thread.Sleep(5000);
                    Agencia agencia = new Agencia();
                    agencia.insertarDatosSpainDMCS(driverConcreto);
                    listadoAgencias.Add(agencia);

                    //driver.FindElement(By.LinkText("Volver")).Click();
                    driverConcreto.Close();
                    Acciones.EsperarCargaPagina(driver);
                    contador--;
                    //driver.Navigate().Refresh();
                }
            }

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVSpainDMCS() + "\n";
            }
            File.WriteAllText("AgenciasSpainDMCS.csv", csv, Encoding.UTF8);
        }



        [Test]
        public void RecogerAgenciasAVIBA()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();
            List<Agencia> listadoAgenciasVacias = new List<Agencia>();
            driver.Navigate().GoToUrl("http://www.aviba.net/agencias/lista-agencias.shtm");
            Acciones.EsperarCargaPagina(driver);

            //Estas lineas se incluyen para hacer un caso de prueba mas corto que acceda directamente a la última página de los listados
            //driver.FindElement(By.LinkText("Ultimo")).Click();
            //Acciones.EsperarCargaPagina(driver);


            bool seguirRecorriendoPaginas = true;
            bool primeraVuelta = true;
            while (seguirRecorriendoPaginas)
            {
                if (!primeraVuelta)
                {
                    driver.FindElement(By.LinkText("Siguiente")).Click();
                    Acciones.EsperarCargaPagina(driver);
                }
                primeraVuelta = false;

                List<IWebElement> contenidos = driver.FindElements(By.CssSelector("tr.tr_busqueda_normal")).ToList<IWebElement>();
                string condicionParada = driver.FindElement(By.CssSelector("div.dataTables_info")).Text;
                string[] parametrosCondicion = condicionParada.Split(' ');

                if (0 < contenidos.Count)
                {
                    int contador = contenidos.Count - 1;
                    IWebDriver driverConcreto = new FirefoxDriver();
                    while (0 <= contador)
                    {
                        contenidos = driver.FindElements(By.CssSelector("tr.tr_busqueda_normal")).ToList<IWebElement>();

                        IWebElement elementoContenido = contenidos[contador];
                        Agencia agencia = new Agencia();
                        bool urlVacia = true;
                        if (0 < elementoContenido.FindElements(By.CssSelector("td.td_busqueda a")).Count)
                        {
                            string url = elementoContenido.FindElement(By.CssSelector("td.td_busqueda a")).GetAttribute("href");
                            urlVacia = false;
                            driverConcreto.Navigate().GoToUrl(url);
                            Acciones.EsperarCargaPagina(driverConcreto);

                            
                            agencia.insertarDatosAVIBA(driverConcreto);
                            agencia.web = url;
                        }
                        if (agencia.todosVacios || urlVacia)
                        {
                            agencia.razonSocial = elementoContenido.FindElement(By.CssSelector("td.td_busqueda.sorting_1")).Text;
                            agencia.poblacion = elementoContenido.FindElement(By.CssSelector("td.td_busqueda.sorting_2")).Text;
                            agencia.telefono = elementoContenido.FindElement(By.CssSelector("td.td_busqueda.sorting_3")).Text;
                            listadoAgenciasVacias.Add(agencia);
                        }
                        else
                        {
                            listadoAgencias.Add(agencia);
                        }

                        contador--;

                        
                        if (parametrosCondicion[parametrosCondicion.Length - 2].Equals(parametrosCondicion[parametrosCondicion.Length - 4]))
                        {
                            seguirRecorriendoPaginas = false;
                        }
                    }
                        driverConcreto.Quit();
                }
                
            }


            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVAVIBA() + "\n";
            }
            File.WriteAllText("AgenciasAVIBA.csv", csv, Encoding.UTF8);

            csv = "A continuación se muestran las agencias vacías (Razón Social - Población - Número de teléfono - Url Web)\n";
            foreach (Agencia agencia in listadoAgenciasVacias)
            {
                csv += "Razón Social: " + agencia.razonSocial + "  - Población: " + agencia.poblacion + "  - Teléfono: " + agencia.telefono + "  - URL: " + agencia.web + "\n";
            }
            File.WriteAllText("AgenciasVaciasAVIBA.txt", csv, Encoding.UTF8);
        }


        [Test]
        public void RecogerAgenciasAEVAV()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();
            List<Agencia> listadoAgenciasVacias = new List<Agencia>();
            driver.Navigate().GoToUrl("http://www.aevav.es/agencias/lista.shtm");
            Acciones.EsperarCargaPagina(driver);

            //Estas lineas se incluyen para hacer un caso de prueba mas corto que acceda directamente a la última página de los listados
            //driver.FindElement(By.LinkText("Ultimo")).Click();
            //Acciones.EsperarCargaPagina(driver);
            

            bool seguirRecorriendoPaginas = true;
            bool primeraVuelta = true;
            IWebDriver driverConcreto = new FirefoxDriver();
            while (seguirRecorriendoPaginas)
            {
                if (!primeraVuelta)
                {
                    driver.FindElement(By.LinkText("›")).Click();
                    Acciones.EsperarCargaPagina(driver);
                }
                primeraVuelta = false;

                List<IWebElement> contenidos = driver.FindElements(By.CssSelector("tr.tr_busqueda_normal")).ToList<IWebElement>();

                if (0 < contenidos.Count)
                {
                    
                    int contador = contenidos.Count - 1;
                    
                    while (0 <= contador)
                    {
                        contenidos = driver.FindElements(By.CssSelector("tr.tr_busqueda_normal")).ToList<IWebElement>();

                        //IWebElement elementoContenido = contenidos[contenidos.Count - contador];
                        IWebElement elementoContenido = contenidos[contador];
                        
                        string url = elementoContenido.GetAttribute("onclick");
                        int posicionInicial = url.IndexOf("'") + 1;
                        url = url.Remove(0, posicionInicial);
                        int posicionFinal = url.IndexOf("'");
                        url = url.Remove(posicionFinal, (url.Length - posicionFinal));
                        
                        WebRequest request = WebRequest.Create(url);
                        HttpWebResponse response;
                        bool condicion = false;
                        try
                        {
                            response = (HttpWebResponse)request.GetResponse();
                            condicion = response.ResponseUri.AbsoluteUri.Equals(url.ToLower()+"/");
                        }
                        catch (Exception e) 
                        { 
                            condicion = false; 
                        }
                        Agencia agencia = new Agencia();
                        if (condicion)
                        {
                            driverConcreto.Navigate().GoToUrl(url);
                            Acciones.EsperarCargaPagina(driverConcreto);

                            agencia.insertarDatosAEVAV(driverConcreto);
                        }
                        agencia.web = url;
                        
                        if (agencia.todosVacios)
                        {
                            List<IWebElement> datosSimples = elementoContenido.FindElements(By.CssSelector("td.td_busqueda")).ToList<IWebElement>();
                            agencia.razonSocial = datosSimples[0].Text;
                            agencia.poblacion = datosSimples[1].Text;
                            agencia.telefono = datosSimples[2].Text;
                            listadoAgenciasVacias.Add(agencia);
                        }
                        else
                        {
                            listadoAgencias.Add(agencia);
                        }

                        contador--;


                        if (!(0 < driver.FindElements(By.LinkText("›")).Count))
                        {
                            seguirRecorriendoPaginas = false;
                        }
                    }
                    
                }

            }
            driverConcreto.Quit();

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVAEVAV() + "\n";
            }
            File.WriteAllText("AgenciasAEVAV.csv", csv, Encoding.UTF8);

            csv = "A continuación se muestran las agencias vacías (Razón Social - Población - Número de teléfono - Url Web)\n";
            foreach (Agencia agencia in listadoAgenciasVacias)
            {
                csv += "Razón Social: " + agencia.razonSocial + "  - Población: " + agencia.poblacion + "  - Teléfono: " + agencia.telefono + "  - URL: " + agencia.web + "\n";
            }
            File.WriteAllText("AgenciasVaciasAEVAV.txt", csv, Encoding.UTF8);
        }


        [Test]
        public void RecogerAgenciasColombia()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();

            driver.Navigate().GoToUrl("http://rnt.rue.com.co/");
            Acciones.EsperarCargaPagina(driver);

            List<IWebElement> contenidos = driver.FindElements(By.CssSelector("tbody tr td a")).ToList<IWebElement>();

            if (0 < contenidos.Count)
            {
                int contador = contenidos.Count - 1;
                IWebDriver driverCiudades = new FirefoxDriver();    //Se encarga de abrir las paginas de las ciudades
                IWebDriver driverAgencias = new FirefoxDriver();    //Se encarga de abrir las paginas de las agencias de cada ciudad
                while (0 <= contador)
                {
                    contenidos = driver.FindElements(By.CssSelector("tbody tr td a")).ToList<IWebElement>();
                    IWebElement elementoContenido = contenidos[contador];
                    //elementoContenido.Click();
                    string url = elementoContenido.GetAttribute("href");
                    
                    driverCiudades.Navigate().GoToUrl(url);
                    Acciones.EsperarCargaPagina(driverCiudades);

                    //clikar para entrar en prestadores de servicios turisticos
                    driverCiudades.FindElement(By.LinkText("Prestadores de Servicios Turísticos")).Click();
                    Acciones.EsperarCargaPagina(driverCiudades);
                    // clikar en el chek de estado "activo"
                    //Lista de opciones (hay que coger la 2): select#establecimiento_filters_establecimiento_estado_id option o bien buscarlo por linktext y hacer un clik
                    //driverCiudades.FindElement(By.CssSelector("select#establecimiento_filters_establecimiento_estado_id")).Click();
                    SelectElement select = new SelectElement(driverCiudades.FindElement(By.CssSelector("select#establecimiento_filters_establecimiento_estado_id")));
                    select.SelectByText("Activo");
                    
                    //clikar en categoria y seleccionar agencias de viajes
                    select = new SelectElement(driverCiudades.FindElement(By.CssSelector("select#establecimiento_filters_categoria_establecimiento_id")));
                    select.SelectByText("AGENCIA DE VIAJES");
                    
                    //clikar en buscar
                    driverCiudades.FindElement(By.Name("submit")).Click();
                    
                    //while mientras seguirRecorriendoPaginas=true
                    bool seguirRecorriendoPaginas=true;
                    while (seguirRecorriendoPaginas)
                    {
                        //sacar contenidos de la lista de agencias
                        
                        int contadorAgencias = driverCiudades.FindElements(By.CssSelector("div.table_box.lista_establecimientos tr")).Count - 2;    //-1 porque el primer tr contiene los titulos
                        //while mientras 0<contador agencias
                        while (0 <= contadorAgencias)
                        {
                            List<IWebElement> contenidoListaAgencias = driverCiudades.FindElements(By.CssSelector("div.table_box.lista_establecimientos tr")).ToList<IWebElement>();
                            //sacar url agencia y hacer navigate driver agencias
                            IWebElement elementoListaAgencias = contenidoListaAgencias[contadorAgencias+1].FindElement(By.CssSelector("td a")); //Se suma uno, ya que se le ha restado uno de más al contador, y es el primer elemento el que no nos interesa.
                            
                            string urlAgencia = elementoListaAgencias.GetAttribute("href");

                            driverAgencias.Navigate().GoToUrl(urlAgencia);
                            Acciones.EsperarCargaPagina(driverAgencias);

                            Agencia agencia = new Agencia();
                            agencia.insertarDatosColombia(driverAgencias);
                            agencia.web = urlAgencia;
                            listadoAgencias.Add(agencia);
                            
                            contadorAgencias--;
                        }
                        //end while contador agencias
                        //if numero de paginas = pagina actual seguirRecorriendoPaginas=false
                        List<IWebElement> parametrosParada = driverCiudades.FindElements(By.CssSelector("div.pagination_desc strong")).ToList<IWebElement>();
                        try 
                        { 
                            string numerosDePagina = parametrosParada[1].Text;
                            string numeroPaginaActual = numerosDePagina.Split(' ')[0];
                            string numeroPaginaFinal = numerosDePagina.Split(' ')[2];
                            if(numeroPaginaActual.Equals(numeroPaginaFinal))
                            {
                                seguirRecorriendoPaginas = false;
                            }
                            driverCiudades.FindElements(By.CssSelector("div.pagination a.imagen_pagina")).ToList<IWebElement>()[1].Click();
                            Acciones.EsperarCargaPagina(driverCiudades);
                        }
                        catch (Exception e) 
                        { 
                            seguirRecorriendoPaginas = false; 
                        }
                    }
                    //end while seguir recogiendoPaginas
                    Acciones.EsperarCargaPagina(driver);
                    contador--;
                    //driver.Navigate().Refresh();
                }
                driverAgencias.Close();
                driverCiudades.Close();
                
            }

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVColombia() + "\n";
            }
            File.WriteAllText("AgenciasColombia.csv", csv, Encoding.UTF8);
        }


        [Test]
        public void RecogerAgenciasChile()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();
            driver.Navigate().GoToUrl("http://www.sernatur.cl/buscador-de-servicios-turisticos/");
            Acciones.EsperarCargaPagina(driver);
            //driver.FindElements(By.CssSelector("div#tab1/table/tbody/tr/td/a"))[2].Click();
            driver.FindElements(By.CssSelector("div#tabs"))[0].FindElements(By.CssSelector("table"))[0].FindElements(By.CssSelector("tbody"))[0].FindElements(By.CssSelector("tr"))[0].FindElements(By.CssSelector("td"))[1].FindElements(By.CssSelector("a"))[2].Click();
            driver.FindElement(By.LinkText("Buscar")).Click();
            Acciones.EsperarCargaPagina(driver);
            bool seguirRecorriendoPaginas = true;
            while (seguirRecorriendoPaginas)
            {

                List<IWebElement> contenidos = driver.FindElements(By.CssSelector("div#searchcontent>table>tbody>tr")).ToList<IWebElement>();
                if (0 < contenidos.Count)
                {
                    int contador = contenidos.Count - 1;
                    while (0 <= contador)
                    {
                        IWebElement elementoContenido = contenidos[contador];
                        Agencia agencia = new Agencia();
                        agencia.insertarDatosChile(elementoContenido);
                        listadoAgencias.Add(agencia);
                        contador--;
                    }
                    if (!(0 < driver.FindElements(By.LinkText("Siguiente")).Count()))
                    {
                        seguirRecorriendoPaginas = false;
                    }
                    else 
                    {
                        driver.FindElement(By.LinkText("Siguiente")).Click();
                        Acciones.EsperarCargaPagina(driver);
                    }
                }
                else
                {
                    seguirRecorriendoPaginas = false;
                }
            }


            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVChile() + "\n";
            }
            File.WriteAllText("AgenciasChile.csv", csv, Encoding.UTF8);
        }

        [Test]
        public void RecogerAgenciasHonduras()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();
            int numeroDeUrls = 2;
            
            List<String> listaUrl = new List<String>();
            listaUrl.Add("http://www.honduras.travel/es/con-quien/agencias-de-viaje");
            listaUrl.Add("http://www.honduras.travel/es/con-quien/tour-operadores");
            while (0 < numeroDeUrls)
            {
                string url = String.Empty;
                url = listaUrl[numeroDeUrls - 1];
                numeroDeUrls--;
                driver.Navigate().GoToUrl(url);
                Acciones.EsperarCargaPagina(driver);
                List<IWebElement> tablas = driver.FindElements(By.CssSelector("div.main.columns table.views-table")).ToList<IWebElement>();
                if (0 < tablas.Count)
                {
                    int contadorTablas = tablas.Count - 1;
                    while (0 <= contadorTablas)
                    {
                        IWebElement contenidoTabla = tablas[contadorTablas];
                        List<IWebElement> filas = contenidoTabla.FindElements(By.CssSelector("tbody tr")).ToList<IWebElement>();
                        if (0 < filas.Count)
                        {
                            int contadorFilas = filas.Count - 1;
                            while (0 <= contadorFilas)
                            {
                                Agencia agencia = new Agencia();
                                agencia.insertarDatosHonduras(filas[contadorFilas]);
                                agencia.departamento = contenidoTabla.FindElement(By.CssSelector("caption")).Text.Trim();
                                listadoAgencias.Add(agencia);
                                contadorFilas--;
                            }
                        }
                        contadorTablas--;
                    }
                       
                }
                    
                
            }

            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVHonduras() + "\n";
            }
            File.WriteAllText("AgenciasHonduras.csv", csv, Encoding.UTF8);
        }


        [Test]
        public void RecogerAgenciasCostaRica()
        {
            List<Agencia> listadoAgencias = new List<Agencia>();
            driver.Navigate().GoToUrl("http://www.visitcostarica.com/ict/paginas/categoria.asp?idcate=67&sPais=14");
            Acciones.EsperarCargaPagina(driver);
            bool seguirRecorriendoPaginas = true;
            IWebDriver driverPagina = new FirefoxDriver();
            while (seguirRecorriendoPaginas)
            {

                List<IWebElement> contenidos = driver.FindElements(By.CssSelector("div.span-b-margin-10")).ToList<IWebElement>();
                if (0 < contenidos.Count)
                {
                    int contador = contenidos.Count - 1;
                    while (0 <= contador)
                    {
                        IWebElement elementoContenido = contenidos[contador];
                        string url = elementoContenido.FindElement(By.CssSelector("a")).GetAttribute("href");
                        driverPagina.Navigate().GoToUrl(url);
                        Acciones.EsperarCargaPagina(driverPagina);
                        Agencia agencia = new Agencia();
                        agencia.insertarDatosCostaRica(driverPagina);
                        listadoAgencias.Add(agencia);
                        contador--;
                    }
                    if (!(0 < driver.FindElements(By.LinkText("Next")).Count()))
                    {
                        seguirRecorriendoPaginas = false;
                    }
                    else
                    {
                        driver.FindElement(By.LinkText("Next")).Click();
                        Acciones.EsperarCargaPagina(driver);
                    }
                }
                else
                {
                    seguirRecorriendoPaginas = false;
                }
            }


            string csv = "Provedor;Cliente;RUC;RAZÓN SOCIAL;NOMBRE COMERCIAL;CLASIFICACION;WEB;DIRECCIÓN DEL ESTABLECIMIENTO;DEPARTAMENTO/PROVINCIA/DISTRITO;TIPO DE TURISMO;MODALIDAD DE TURISMO;FEC.INIC;Estado;REPRE. LEGAL;DNI REPRE. LEGAL;TELF.;FAX;CORREO ELECTRÓNICO;Pais;StateId;CP;A6;A7;A8;A9;A10;A1;A2;A3;A4;A5;A6;A7;A8;A9;A10\n";
            foreach (Agencia agencia in listadoAgencias)
            {
                csv += agencia.getCSVAPAV() + "\n";
            }
            File.WriteAllText("AgenciasCostaRica.csv", csv, Encoding.UTF8);
        }
		 /// <summary>
		 /// Método que busca el fichero JSON donde está guardad la información de los paises y la recupera en formato texto
		 /// </summary>
		 /// <param name="nombreFichero"></param>
		 /// <returns></returns>
		  protected String cargarFicheroTexto(String nombreFichero)
		  {
			  string texto = string.Empty;
			  if (!String.IsNullOrEmpty(nombreFichero) && File.Exists(nombreFichero))
			  {
				  texto = File.ReadAllText(nombreFichero);
			  }
			  return texto;
		  }


          //#region Recuperación de los paises de la Web y creación de un CSV y un JSON con los datos recuperados
          //private void recuperarPaises(string urlListadoPaises)
          //{
          //    List<Paises> paises = recuperarPaisesWeb(urlBase);
          //    if (null != paises)
          //    {
          //        string csv = "id;nombre;url\n";

          //        foreach (Paises pais in paises)
          //        {
          //            csv += pais.idPais + ";" + pais.nombre + ";" + pais.urlPais + "\n";
          //        }
          //        File.WriteAllText(@"paises.csv", csv);
          //        FileLogs guardarPaises = new FileLogs(@"paises.json");
          //        guardarPaises.EscribirLog(JsonConvert.SerializeObject(paises));
          //    }
          //}

          ///// <summary>
          ///// Método que recuperará los nombres de los paises que hay en la página web inspeccionada
          ///// </summary>
          ///// <returns></returns>
          //private List<Paises> recuperarPaisesWeb(string urlListadoPaises)
          //{
          //    List<Paises> paises = new List<Paises>();

          //    if (!String.IsNullOrEmpty(urlListadoPaises))
          //    {
          //        driver.Navigate().GoToUrl(urlListadoPaises);
          //        Acciones.EsperarCargaPagina(driver);
          //        IWebElement bloquePaises = null;
          //        List<IWebElement> leyendas = driver.FindElements(By.TagName("legend")).ToList<IWebElement>();
          //        int contadorLeyendas = 0;
          //        while (null == bloquePaises && contadorLeyendas < leyendas.Count)
          //        {
          //            IWebElement leyenda = leyendas[contadorLeyendas];
          //            contadorLeyendas++;

          //            if (leyenda.Text.Equals("Country Postcode (ZIP)"))
          //            {
          //                bloquePaises = Acciones.GetParent(leyenda);
          //            }

          //        }

          //        if (null != bloquePaises)
          //        {
          //            List<IWebElement> IWebPaises = new List<IWebElement>();
          //            List<IWebElement> bloquesPaises = bloquePaises.FindElements(By.ClassName("menu")).ToList<IWebElement>();
          //            GestorLocalizaciones gestionLocalizaciones = new GestorLocalizaciones();

          //            //recorre todos los elementos de bloquepaises y se almacenan ListaPaises: Lista paises cada vez que se ejecute el bucle cogerá los sucesivos valores que tiene bloque paises
          //            foreach (IWebElement listaPaises in bloquesPaises)
          //            {
          //                IWebPaises.AddRange(listaPaises.FindElements(By.TagName("a")));
          //            }

          //            foreach (IWebElement iWebPais in IWebPaises)
          //            {
          //                Paises paisNuevo = new Paises(iWebPais);
          //                string traduccion = dameTraduccionGoogle(paisNuevo.nombre);
          //                if (!String.IsNullOrEmpty(traduccion))
          //                    paisNuevo.nombreTraducido = traduccion;

          //                paisNuevo.idPais = gestionLocalizaciones.buscarIdBD(paisNuevo);

          //                if (0 >= paisNuevo.idPais)
          //                {
          //                    traduccion = dameTraduccionWordRefernce(paisNuevo.nombre);
          //                    if (!String.IsNullOrEmpty(traduccion))
          //                        paisNuevo.nombreTraducido = traduccion;

          //                    paisNuevo.idPais = gestionLocalizaciones.buscarIdBD(paisNuevo);

          //                }

          //                if (0 < paisNuevo.idPais)
          //                    paisNuevo.nombre = paisNuevo.nombreTraducido;

          //                paises.Add(paisNuevo);
          //            }
          //        }

          //    }

          //    return paises;
          //}

          //#endregion
    }
}
