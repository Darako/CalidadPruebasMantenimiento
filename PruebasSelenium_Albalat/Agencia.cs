using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RecogerAgenciasParaguay.Datos
{
    public class Agencia
    {
        public bool todosVacios { get; set; }
        public int id { get; set; }
        public string NIF { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        public string telefono { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string web { get; set; }
        public string clasificacion { get; set; }
        public string cp { get; set; }
        public string direccion { get; set; }
        public string poblacion { get; set; }
        public string departamento { get; set; }
        public string detalles { get; set; }
        public string representanteLegal { get; set; }

        public Agencia() 
        {
            todosVacios = true;
        }
        
        public void insertarDatosParaguay(IWebDriver pagina)
        {
            if (null != pagina)
            {
                if (0 < pagina.FindElements(By.Id("cpContenido_lblDepartamento")).Count)
                {
                    departamento = pagina.FindElement(By.Id("cpContenido_lblDepartamento")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblCiudad")).Count)
                {
                    poblacion = pagina.FindElement(By.Id("cpContenido_lblCiudad")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblDireccion")).Count)
                {
                    direccion = pagina.FindElement(By.Id("cpContenido_lblDireccion")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblTelefono")).Count)
                {
                    telefono = pagina.FindElement(By.Id("cpContenido_lblTelefono")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblProveedor")).Count)
                {
                    razonSocial = pagina.FindElement(By.Id("cpContenido_lblProveedor")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblNombreFantasia")).Count)
                {
                    nombreComercial = pagina.FindElement(By.Id("cpContenido_lblNombreFantasia")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblServicio")).Count)
                {
                    clasificacion = pagina.FindElement(By.Id("cpContenido_lblServicio")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblPaginaWeb")).Count)
                {
                    web = pagina.FindElement(By.Id("cpContenido_lblPaginaWeb")).Text;
                }

                if (0 < pagina.FindElements(By.Id("cpContenido_lblEmail")).Count)
                {
                    email = pagina.FindElement(By.Id("cpContenido_lblEmail")).Text;
                }
            }
        }

        public void insertarDatosACAVE(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-razon-social div.field-item.even")).Count)
                {
                    razonSocial = pagina.FindElement(By.CssSelector("div.field-name-field-razon-social div.field-item.even")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-title-field div.field-item.even")).Count)
                {
                    nombreComercial = pagina.FindElement(By.CssSelector("div.field-name-title-field div.field-item.even")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-nif div.field-item.even")).Count)
                {
                    NIF = pagina.FindElement(By.CssSelector("div.field-name-field-nif div.field-item.even")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-telefon div.field-item.even")).Count)
                {
                    telefono = pagina.FindElement(By.CssSelector("div.field-name-field-telefon div.field-item.even")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-fax div.field-item.even")).Count)
                {
                    fax = pagina.FindElement(By.CssSelector("div.field-name-field-fax div.field-item.even")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-email div.field-item.even")).Count)
                {
                    email = pagina.FindElement(By.CssSelector("div.field-name-field-email div.field-item.even")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-direccion .thoroughfare")).Count)
                {
                    direccion = pagina.FindElement(By.CssSelector("div.field-name-field-direccion .thoroughfare")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-direccion .postal-code")).Count)
                {
                    cp = pagina.FindElement(By.CssSelector("div.field-name-field-direccion .postal-code")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-direccion .locality")).Count)
                {
                    poblacion = pagina.FindElement(By.CssSelector("div.field-name-field-direccion .locality")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.field-name-field-direccion .state")).Count)
                {
                    departamento = pagina.FindElement(By.CssSelector("div.field-name-field-direccion .state")).Text;
                    todosVacios = false;
                }
            }
        }

        public void insertarDatosAV(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("span.fc_item_title")).Count)
                {
                    razonSocial = pagina.FindElement(By.CssSelector("span.fc_item_title")).Text;
                    nombreComercial = razonSocial;
                    todosVacios = false;
                }

                NIF = String.Empty;

                if (0 < pagina.FindElements(By.CssSelector("div.flexi.value.field_teléfono")).Count)
                {
                    telefono = pagina.FindElement(By.CssSelector("div.flexi.value.field_teléfono")).Text.Replace("\r"," ").Replace("\n"," ");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.flexi.value.field_fax")).Count)
                {
                    fax = pagina.FindElement(By.CssSelector("div.flexi.value.field_fax")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.flexi.value.field_Email a")).Count)
                {
                    email = pagina.FindElement(By.CssSelector("div.flexi.value.field_Email a")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.flexi.value.field_Domicilio")).Count)
                {
                    direccion = pagina.FindElement(By.CssSelector("div.flexi.value.field_Domicilio")).Text;
                    todosVacios = false;
                }

                cp = String.Empty;

                if (0 < pagina.FindElements(By.CssSelector("div.flexi.value.field_Población")).Count)
                {
                    poblacion = pagina.FindElement(By.CssSelector("div.flexi.value.field_Población")).Text;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.flexi.value.field_Isla")).Count)
                {
                    departamento = pagina.FindElement(By.CssSelector("div.flexi.value.field_Isla")).Text;
                    todosVacios = false;
                }
            }
        }

        public void insertarDatosAAVOT(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("h3.info_section_title")).Count)
                {
                    todosVacios = false;
                    razonSocial = pagina.FindElement(By.CssSelector("h3.info_section_title")).Text;
                    nombreComercial = razonSocial;
                }

                List<IWebElement> contenidos = pagina.FindElements(By.CssSelector("div.info.portfolio_single_content p")).ToList<IWebElement>();

                if (0 < contenidos.Count)
                {
                    foreach (IWebElement elementoContenido in contenidos)
                    {
                        if (elementoContenido.Text.Contains("Dirección:"))
                        {
                            todosVacios = false;
                            direccion = elementoContenido.Text.Remove(0, elementoContenido.Text.IndexOf("\n") + 1);
                        }
                        if (elementoContenido.Text.Contains("Teléfono:"))
                        {
                            todosVacios = false;
                            telefono = elementoContenido.Text.Remove(0, elementoContenido.Text.IndexOf("\n") + 1);
                        }
                        if (elementoContenido.Text.Contains("Email:"))
                        {
                            todosVacios = false;
                            email = elementoContenido.Text.Remove(0, elementoContenido.Text.IndexOf("\n") + 1);
                        }
                        if (elementoContenido.Text.Contains("Web:"))
                        {
                            todosVacios = false;
                            web = elementoContenido.Text.Remove(0, elementoContenido.Text.IndexOf("\n") + 1);
                        }
                    }
                }
            }
        }
        
        public void insertarDatosAEDAV(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("td.formulariosTitulos")).Count)
                {
                    todosVacios = false;
                    razonSocial = pagina.FindElement(By.CssSelector("td.formulariosTitulos")).Text;
                    nombreComercial = razonSocial;
                }

                IWebElement contenidoDelParrafo = pagina.FindElement(By.CssSelector("td.cuerpo p"));
                string[] contenidosDeLosCampos = contenidoDelParrafo.Text.Split('\n');

                if (0 < contenidosDeLosCampos.Length)
                {
                    if (0 < pagina.FindElements(By.CssSelector("a")).Count)
                    {
                        todosVacios = false;
                        contenidosDeLosCampos[0].Remove(0, contenidosDeLosCampos[0].IndexOf("\n") + 1);
                        contenidosDeLosCampos[0].Replace("\n", "");
                        direccion = contenidosDeLosCampos[0].Replace("\r", "");
                        string[] direccionPorPartes = contenidosDeLosCampos[1].Split(',');
                        departamento = direccionPorPartes[direccionPorPartes.Length - 1].Replace("\r", "");
                        string[] poblacionPorPartes = direccionPorPartes[direccionPorPartes.Length - 2].Split('\n');
                        poblacion = poblacionPorPartes[poblacionPorPartes.Length - 1].Replace("\r", "");
                    }
                    foreach (string elementoContenido in contenidosDeLosCampos)
                    {
                        
                        if (elementoContenido.Contains("Web:"))
                        {
                            todosVacios = false;
                            elementoContenido.Remove(0, elementoContenido.IndexOf("\n") + 1);
                            web = elementoContenido.Trim().Replace("Web:","").Replace("\r", "").Replace("\n", "");
                        }
                        if (elementoContenido.Contains("E-mail:"))
                        {
                            todosVacios = false;
                            elementoContenido.Remove(0, elementoContenido.IndexOf("\n") + 1);
                            email = elementoContenido.Trim().Replace("E-mail:","").Replace(";", " ").Replace("\r", "").Replace("\n", "");
                            
                        }
                        if (elementoContenido.Contains("Telf.:"))
                        {
                            todosVacios = false;
                            elementoContenido.Remove(0, elementoContenido.IndexOf("\n") + 1);
                            telefono = elementoContenido.Trim().Replace("Telf.:","").Replace("\r", "").Replace("\n", "");
                        }
                        if (elementoContenido.Contains("Fax:"))
                        {
                            todosVacios = false;
                            elementoContenido.Remove(0, elementoContenido.IndexOf("\n") + 1);

                            fax = elementoContenido.Trim().Replace("Fax:", "").Replace("\r", "").Replace("\n", "");
                        }
                        if (elementoContenido.Contains("Idiomas hablados:"))
                        {
                            todosVacios = false;
                            elementoContenido.Remove(0, elementoContenido.IndexOf("\n") + 1);
                            elementoContenido.Replace("\n", "");
                            detalles = elementoContenido.Replace("\r", "");
                            
                        }
                        
                    }
                }
            }
        }

        public void insertarDatosAPAV(IWebElement elemento)
        {
            if (null != elemento)
            {
                todosVacios = true;
                IWebElement direccionAgencia = elemento.FindElement(By.CssSelector("p"));
                string[] contenidosDeLosCampos = elemento.Text.Split('\n');
                int contador = 0;
                bool direccionDosLineas = false;
                if (0 < contenidosDeLosCampos.Length)
                {
                    foreach (string elementoContenido in contenidosDeLosCampos)
                    {
                        switch (contador)
                        {
                            case 0:
                                razonSocial = elementoContenido.Replace("\r", "");
                                nombreComercial = razonSocial;
                                todosVacios = false;
                                break;
                            case 1:
                                //Console.WriteLine("NombreComercial y Telefono separados con split");
                                string[] nombreYTelefono = elementoContenido.Split(')');
                                if (1 < nombreYTelefono.Length) { 
                                    representanteLegal = nombreYTelefono[0].Replace("(","");
                                    telefono = nombreYTelefono[1].Trim().Replace("Teléfono:","").Replace("\r","");
                                }
                                else
                                {
                                    telefono = nombreYTelefono[0].Trim().Replace("Teléfono:", "").Replace("\r", "");
                                }
                                todosVacios = false;
                                break;
                            case 2:
                                //Console.WriteLine("Direccion quitandole el retorno de carro");
                                direccion = direccionAgencia.Text.Replace("\r", "").Replace("\n", " ");
                                todosVacios = false;
                                break;
                            case 3:
                                string elementoAux = elementoContenido.Replace("\r", "").Replace("\n", " ");
                                
                                if (direccion.Contains(elementoAux))
                                {
                                    //Console.WriteLine("Si la direccion ocupa dos lineas se indica en la siguiente variable");
                                    direccionDosLineas = true;
                                }
                                else
                                {
                                    //Console.WriteLine("Si la direccion ya está completada hay que tratar el CP");
                                    string[] codigoYNombreCP = elementoContenido.Split('–');
                                    if (1 < codigoYNombreCP.Length)
                                    {
                                        cp = codigoYNombreCP[0].Replace("\r","");
                                        departamento = codigoYNombreCP[1].Replace("\r", "");
                                    }
                                    else
                                    {
                                        departamento = codigoYNombreCP[0].Replace("\r", "");
                                    }
                                    todosVacios = false;
                                }
                                break;
                            case 4:
                                if (direccionDosLineas)
                                {
                                    //Console.WriteLine("Si la direccion tiene 2 lineas entonces ahora se trata el CP");
                                    string[] codigoYNombreCP = elementoContenido.Split('–');
                                    if (1 < codigoYNombreCP.Length)
                                    {
                                        cp = codigoYNombreCP[0].Replace("\r", "");
                                        departamento = codigoYNombreCP[1].Replace("\r", "");
                                    }
                                    else
                                    {
                                        departamento = codigoYNombreCP[0].Replace("\r", "");
                                    }
                                    todosVacios = false;
                                }
                                else
                                {
                                    //Console.WriteLine("Sino se trata el email");
                                    email = elementoContenido.Replace("\r","");
                                    todosVacios = false;
                                }
                                break;
                            case 5:
                                if (direccionDosLineas)
                                {
                                    //Console.WriteLine("Si la direccion tiene 2 lineas entonces ahora se trata el Email");
                                    email = elementoContenido.Replace("\r", "");
                                    todosVacios = false;
                                }
                                else
                                {
                                    //Console.WriteLine("Sino se trata la direccion Web");
                                    web = elementoContenido.Replace("\r", "");
                                    todosVacios = false;
                                }
                                break;
                            case 6:
                                //Console.WriteLine("Por último si existe este elemento es la direccion Web");
                                web = elementoContenido.Replace("\r", "");
                                todosVacios = false;
                                break;
                            default:
                                //Console.WriteLine("Default case");
                                break;
                        }
                           
                            contador++;
                    }
                }
            }
        }


        public void insertarDatosFETAVE(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("div#headerLogoTexto")).Count)
                {
                    razonSocial = pagina.FindElement(By.CssSelector("div#headerLogoTexto")).Text.Replace("\r", "").Replace("\n", "");
                    nombreComercial = razonSocial;
                    todosVacios = false;
                }

                NIF = String.Empty;
                if (0 < pagina.FindElements(By.CssSelector("li.titulo_cif")).Count)
                {
                    NIF = pagina.FindElement(By.CssSelector("li.titulo_cif")).Text.Replace("\r", "").Replace("\n", "");
                    NIF = NIF.Replace("CIF: ","");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.telefono")).Count)
                {
                    telefono = pagina.FindElement(By.CssSelector("li.telefono")).Text.Replace("\r", "").Replace("\n", "");
                    telefono = telefono.Replace("Teléfono: ","");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.fax")).Count)
                {
                    fax = pagina.FindElement(By.CssSelector("li.fax")).Text.Replace("\r", "").Replace("\n", "");
                    fax = fax.Replace("Fax: ","");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.email")).Count)
                {
                    email = pagina.FindElement(By.CssSelector("li.email")).Text.Replace("\r", "").Replace("\n", "");
                    email = email.Replace("Email: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.direccion")).Count)
                {
                    direccion = pagina.FindElement(By.CssSelector("li.direccion")).Text.Replace("\r", "").Replace("\n", "");
                    string[] direccionDescompuesta = direccion.Split('(');
                    string cpSeguidoDePoblacion = direccionDescompuesta[1];
                    string departamentoConBasura = direccionDescompuesta[2];
                    departamento = departamentoConBasura.Replace(")","").Replace("\n","").Replace("\r","");
                    string[] cpYPoblacionDescompuesto = cpSeguidoDePoblacion.Split(')');
                    cp = cpYPoblacionDescompuesto[0].Replace("\n","").Replace("\r","");
                    poblacion = cpYPoblacionDescompuesto[1].Replace("\n","").Replace("\r","");
                    todosVacios = false;
                }

                //cp = String.Empty;

                //if (3 < pagina.FindElements(By.CssSelector("li.direccion span")).Count)
                //{
                //    List<IWebElement> spansCP = pagina.FindElements(By.CssSelector("li.direccion span")).ToList<IWebElement>();
                //    cp = spansCP[spansCP.Count() - 3].Text.Replace("\r", "").Replace("\n", "");
                //    cp = cp.Replace("(", "").Replace(")", "");
                //    todosVacios = false;
                //}

                //if (2 < pagina.FindElements(By.CssSelector("li.direccion span")).Count)
                //{
                //    List<IWebElement> spansPob = pagina.FindElements(By.CssSelector("li.direccion span")).ToList<IWebElement>();
                //    poblacion = spansPob[spansPob.Count() - 2].Text.Replace("\r", "").Replace("\n", "");
                //    todosVacios = false;
                //}

                //if (1 < pagina.FindElements(By.CssSelector("li.direccion span")).Count)
                //{
                //    List<IWebElement> spansDep = pagina.FindElements(By.CssSelector("li.direccion span")).ToList<IWebElement>();
                //    departamento = spansDep[spansDep.Count() - 1].Text.Replace("\r", "").Replace("\n", "");
                //    departamento = departamento.Replace("(", "").Replace(")", "");
                //    todosVacios = false;
                //}
            }
        }


        public void insertarDatosSpainDMCS(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("div.content.clearfix div h2")).Count)
                {
                    razonSocial = pagina.FindElement(By.CssSelector("div.content.clearfix div h2")).Text.Replace("\r", "").Replace("\n", "");
                    nombreComercial = razonSocial;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("div.content.clearfix p")).Count)
                {
                    List<IWebElement> parrafos = pagina.FindElements(By.CssSelector("div.content.clearfix p")).ToList<IWebElement>();
                    todosVacios = false;
                    direccion = parrafos[1].Text.Replace("\r", " ").Replace("\n", " ");
                    telefono = parrafos[2].Text.Replace("\r", " ").Replace("\n", " ");
                    telefono = telefono.Replace("Telf.: ", "").Replace("Phone: ", "").Replace("Phone.: ", "");
                    
                    string[] emailYWeb = parrafos[3].Text.Split('\n');
                    email = emailYWeb[0].Replace("\r", "").Replace("\n", "");
                    email = email.Replace("e-mail: ", "");

                    web = emailYWeb[1].Replace("\r", "").Replace("\n", "");
                    web = web.Replace("web: ","");

                    string pattern = @"\d{5}";
                    foreach (Match match in Regex.Matches(direccion, pattern, RegexOptions.IgnoreCase))
                    {
                        cp = match.Value;
                    }
                    // No se puede recoger el departamento, ni el CP ni la poblacion, debido a que no sigue el mismo formato en las distintas páginas.
                    //if (!pagina.Url.Equals("http://www.spaindmcs.com/en/miembros-asociados/21-fichasmiembrosasociados/165-team-andaluces-slu")) 
                    //{
                    //    string[] cpYpoblacionYdepartamento = direccion.Split('.');
                    //    cp = cpYpoblacionYdepartamento[0];
                    //    poblacion = cpYpoblacionYdepartamento[1];
                    //    departamento = cpYpoblacionYdepartamento[2];
                    //}

                }
            }
        }


        public void insertarDatosAVIBA(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("div#logo_agencia h1")).Count)
                {
                    razonSocial = pagina.FindElement(By.CssSelector("div#logo_agencia h1")).Text.Replace("\r", "").Replace("\n", "");
                    nombreComercial = razonSocial;
                    todosVacios = false;
                }

                NIF = String.Empty;
                if (0 < pagina.FindElements(By.CssSelector("li.titulo_cif")).Count)
                {
                    NIF = pagina.FindElement(By.CssSelector("li.titulo_cif")).Text.Replace("\r", "").Replace("\n", "");
                    NIF = NIF.Replace("CIF: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.telefono")).Count)
                {
                    telefono = pagina.FindElement(By.CssSelector("li.telefono")).Text.Replace("\r", "").Replace("\n", "");
                    telefono = telefono.Replace("Teléfono: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.fax")).Count)
                {
                    fax = pagina.FindElement(By.CssSelector("li.fax")).Text.Replace("\r", "").Replace("\n", "");
                    fax = fax.Replace("Fax: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.email")).Count)
                {
                    email = pagina.FindElement(By.CssSelector("li.email")).Text.Replace("\r", "").Replace("\n", "");
                    email = email.Replace("Email: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("li.direccion")).Count)
                {
                    direccion = pagina.FindElement(By.CssSelector("li.direccion")).Text.Replace("\r", "").Replace("\n", "");
                    todosVacios = false;
                }

                cp = String.Empty;

                if (3 < pagina.FindElements(By.CssSelector("li.direccion span")).Count)
                {
                    List<IWebElement> spansCP = pagina.FindElements(By.CssSelector("li.direccion span")).ToList<IWebElement>();
                    cp = spansCP[spansCP.Count() - 3].Text.Replace("\r", "").Replace("\n", "");
                    cp = cp.Replace("(", "").Replace(")", "");
                    todosVacios = false;
                }

                if (2 < pagina.FindElements(By.CssSelector("li.direccion span")).Count)
                {
                    List<IWebElement> spansPob = pagina.FindElements(By.CssSelector("li.direccion span")).ToList<IWebElement>();
                    poblacion = spansPob[spansPob.Count() - 2].Text.Replace("\r", "").Replace("\n", "");
                    todosVacios = false;
                }

                if (1 < pagina.FindElements(By.CssSelector("li.direccion span")).Count)
                {
                    List<IWebElement> spansDep = pagina.FindElements(By.CssSelector("li.direccion span")).ToList<IWebElement>();
                    departamento = spansDep[spansDep.Count() - 1].Text.Replace("\r", "").Replace("\n", "");
                    departamento = departamento.Replace("(", "").Replace(")", "");
                    todosVacios = false;
                }
            }
        }

        public void insertarDatosAEVAV(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                if (0 < pagina.FindElements(By.CssSelector("h1#title")).Count)
                {
                    razonSocial = pagina.FindElement(By.CssSelector("h1#title")).Text.Replace("\r", "").Replace("\n", "");
                    nombreComercial = razonSocial;
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("ul.datos_oficina li")).Count)
                {
                    List<IWebElement> listaLi = pagina.FindElements(By.CssSelector("ul.datos_oficina li")).ToList<IWebElement>();
                    foreach(IWebElement li in listaLi)
                    {
                        if(li.Text.Contains("CIF:"))
                        {
                            NIF = li.Text.Trim().Replace("CIF:", "").Replace("\r", "").Replace("\n", "");
                            todosVacios = false;
                        }
                    }
                }

                if (0 < pagina.FindElements(By.CssSelector("ul.datos_oficina li.telefono")).Count)
                {
                    telefono = pagina.FindElement(By.CssSelector("ul.datos_oficina li.telefono")).Text.Replace("\r", "").Replace("\n", "");
                    telefono = telefono.Replace("Teléfono: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("ul.datos_oficina li.fax")).Count)
                {
                    fax = pagina.FindElement(By.CssSelector("ul.datos_oficina li.fax")).Text.Replace("\r", "").Replace("\n", "");
                    fax = fax.Replace("Fax: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("ul.datos_oficina li.email")).Count)
                {
                    email = pagina.FindElement(By.CssSelector("ul.datos_oficina li.email")).Text.Replace("\r", "").Replace("\n", "");
                    email = email.Replace("Email: ", "");
                    todosVacios = false;
                }

                if (0 < pagina.FindElements(By.CssSelector("ul.datos_oficina li.direccion")).Count)
                {
                    direccion = pagina.FindElement(By.CssSelector("ul.datos_oficina li.direccion")).Text.Replace("\r", "").Replace("\n", "");
                    string pattern = @"\d{5}";
                    foreach (Match match in Regex.Matches(direccion, pattern, RegexOptions.IgnoreCase))
                    {
                        cp = match.Value;
                    }
                    poblacion = direccion.Split('-')[2].Trim();
                    direccion = direccion.Split('-')[0].Trim();
                    //string[] direccionDescompuesta = direccion.Split('(');
                    //string cpSeguidoDePoblacion = direccionDescompuesta[1];
                    //string departamentoConBasura = direccionDescompuesta[2];
                    //departamento = departamentoConBasura.Replace(")", "").Replace("\n", "").Replace("\r", "");
                    //string[] cpYPoblacionDescompuesto = cpSeguidoDePoblacion.Split(')');
                    //cp = cpYPoblacionDescompuesto[0].Replace("\n", "").Replace("\r", "");
                    //poblacion = cpYPoblacionDescompuesto[1].Replace("\n", "").Replace("\r", "");
                    todosVacios = false;
                }
            }
        }


        public void insertarDatosColombia(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                List<IWebElement> contenidos = pagina.FindElements(By.CssSelector("div.box_center table.table_box tr")).ToList<IWebElement>();
                email = String.Empty;
                telefono = String.Empty;
                if (0 < contenidos.Count)
                {
                    foreach (IWebElement elementoContenido in contenidos)
                    {
                        if (elementoContenido.Text.Contains("Razón Social"))
                        {
                            todosVacios = false;
                            razonSocial = elementoContenido.FindElement(By.CssSelector("td")).Text;
                            nombreComercial = razonSocial;
                        }
                        if (elementoContenido.Text.Contains("Nit"))
                        {
                            todosVacios = false;
                            NIF = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Nombre del Representante Legal"))
                        {
                            todosVacios = false;
                            representanteLegal = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Correo Electrónico Reportado por la DIAN"))
                        {
                            todosVacios = false;
                            email += elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Teléfono Principal"))
                        {
                            todosVacios = false;
                            telefono += elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Teléfono Celular"))
                        {
                            if (!elementoContenido.FindElement(By.CssSelector("td")).Text.Equals(String.Empty) && !telefono.Contains(elementoContenido.FindElement(By.CssSelector("td")).Text))
                            {
                                todosVacios = false;
                                telefono += ", " + elementoContenido.FindElement(By.CssSelector("td")).Text;
                            }
                        }
                        if (elementoContenido.Text.Contains("Teléfono de Notificaciones"))
                        {
                            if (!elementoContenido.FindElement(By.CssSelector("td")).Text.Equals(String.Empty) && !telefono.Contains(elementoContenido.FindElement(By.CssSelector("td")).Text))
                            {
                                todosVacios = false;
                                telefono += ", " + elementoContenido.FindElement(By.CssSelector("td")).Text;
                            }

                        }
                        if (elementoContenido.Text.Contains("Correo Electrónico") && !elementoContenido.Text.Contains("Correo Electrónico Reportado por la DIAN"))
                        {
                            if (!elementoContenido.FindElement(By.CssSelector("td")).Text.Equals(String.Empty) && !email.Contains(elementoContenido.FindElement(By.CssSelector("td")).Text))
                            {
                                todosVacios = false;
                                email += ", "+ elementoContenido.FindElement(By.CssSelector("td")).Text;
                            }
                        }
                        if (elementoContenido.Text.Contains("Fax"))
                        {
                            todosVacios = false;
                            fax = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Dirección Comercial"))
                        {
                            todosVacios = false;
                            direccion = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Departamento") && !elementoContenido.Text.Contains("Departamento de Notificaciones"))
                        {
                            todosVacios = false;
                            departamento = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Código Municipio Comercial"))
                        {
                            todosVacios = false;
                            cp = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Municipio") && !elementoContenido.Text.Contains("Código Municipio Comercial") && !elementoContenido.Text.Contains("Municipio de Notificaciones"))
                        {
                            todosVacios = false;
                            poblacion = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                    }
                }
            }
        }

        public void insertarDatosChile(IWebElement elemento)
        {
            if (null != elemento)
            {
                todosVacios = true;

                List<IWebElement> contenidoDividido = elemento.FindElements(By.CssSelector("tr")).ToList<IWebElement>();
               
                if (String.Empty != contenidoDividido[0].Text)
                {
                    todosVacios = false;
                    razonSocial = contenidoDividido[0].Text;
                    nombreComercial = razonSocial;
                }
                if (0 < contenidoDividido.Count)
                {
                    foreach (IWebElement elementoContenido in contenidoDividido)
                    {
                        
                        if (elementoContenido.Text.Contains("E-mail:"))
                        {
                            todosVacios = false;
                            email = elementoContenido.Text.Replace("E-mail:","").Trim();
                        }
                        if (elementoContenido.Text.Contains("Fono:"))
                        {
                            todosVacios = false;
                            telefono = elementoContenido.Text.Replace("Fono:", "").Trim();
                        }
                        if (elementoContenido.Text.Contains("Dirección:"))
                        {
                            todosVacios = false;
                            direccion = elementoContenido.Text.Replace("Dirección:", "").Trim();
                        }
                        //if (elementoContenido.Text.Contains("Municipio") && !elementoContenido.Text.Contains("Código Municipio Comercial") && !elementoContenido.Text.Contains("Municipio de Notificaciones"))
                        //{
                        //    todosVacios = false;
                        //    poblacion = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        //}
                        if (elementoContenido.Text.Contains("Sitio Web :"))
                        {
                            todosVacios = false;
                            web = elementoContenido.Text.Replace("Sitio Web :", "").Trim();
                        }
                        if (elementoContenido.Text.Contains("Clase o Tipo de Servicio:"))
                        {
                            todosVacios = false;

                            detalles = elementoContenido.Text.Replace("Clase o Tipo de Servicio:", "").Trim();
                        }
                    }
                }
            }
        }

        public void insertarDatosHonduras(IWebElement elemento)
        {
            if (null != elemento)
            {
                todosVacios = true;

                List<IWebElement> contenidoDividido = elemento.FindElements(By.CssSelector("td")).ToList<IWebElement>();

                foreach (IWebElement contenido in contenidoDividido)
                {

                    if (contenido.GetAttribute("class").Equals("views-field views-field-title views-align-left") || contenido.GetAttribute("class").Equals("views-field views-field-title"))
                    {
                        razonSocial = contenido.Text.Replace("\r", " ").Replace("\n", " ");
                        nombreComercial = razonSocial;
                        todosVacios = false;
                    }
                    if (contenido.GetAttribute("class").Equals("views-field views-field-field-to-telefonos"))
                    {
                        telefono = contenido.Text.Replace("\r", " ").Replace("\n", " ");
                        todosVacios = false;
                    }

                    if (contenido.GetAttribute("class").Equals("views-field views-field-field-website"))
                    {
                        if (!contenido.Text.Equals(String.Empty))
                        {
                            web = contenido.FindElement(By.CssSelector("a")).GetAttribute("href").Replace("\r", "").Replace("\n", "");
                            todosVacios = false;
                        }
                    }

                    if (contenido.GetAttribute("class").Equals("views-field views-field-field-website-1"))
                    {
                        if (!contenido.Text.Equals(String.Empty))
                        {
                            string aux = contenido.FindElement(By.CssSelector("a")).GetAttribute("href").Replace("\r", "").Replace("\n", "");
                            if (!web.Contains(aux))
                            {
                                todosVacios = false;
                                web += ", " + aux;
                            }
                        }
                    }

                    if (contenido.GetAttribute("class").Equals("views-field views-field-field-to-email"))
                    {
                        email = contenido.Text.Replace("\r", "").Replace("\n", "");
                        todosVacios = false;
                    }
                }
            }
        }

        public void insertarDatosCostaRica(IWebDriver pagina)
        {
            if (null != pagina)
            {
                todosVacios = true;

                List<IWebElement> contenidos = pagina.FindElements(By.CssSelector("div.box_center table.table_box tr")).ToList<IWebElement>();
                email = String.Empty;
                telefono = String.Empty;
                if (0 < contenidos.Count)
                {
                    foreach (IWebElement elementoContenido in contenidos)
                    {
                        if (elementoContenido.Text.Contains("Razón Social"))
                        {
                            todosVacios = false;
                            razonSocial = elementoContenido.FindElement(By.CssSelector("td")).Text;
                            nombreComercial = razonSocial;
                        }
                        if (elementoContenido.Text.Contains("Nit"))
                        {
                            todosVacios = false;
                            NIF = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Nombre del Representante Legal"))
                        {
                            todosVacios = false;
                            representanteLegal = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Correo Electrónico Reportado por la DIAN"))
                        {
                            todosVacios = false;
                            email += elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Teléfono Principal"))
                        {
                            todosVacios = false;
                            telefono += elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Teléfono Celular"))
                        {
                            if (!elementoContenido.FindElement(By.CssSelector("td")).Text.Equals(String.Empty) && !telefono.Contains(elementoContenido.FindElement(By.CssSelector("td")).Text))
                            {
                                todosVacios = false;
                                telefono += ", " + elementoContenido.FindElement(By.CssSelector("td")).Text;
                            }
                        }
                        if (elementoContenido.Text.Contains("Teléfono de Notificaciones"))
                        {
                            if (!elementoContenido.FindElement(By.CssSelector("td")).Text.Equals(String.Empty) && !telefono.Contains(elementoContenido.FindElement(By.CssSelector("td")).Text))
                            {
                                todosVacios = false;
                                telefono += ", " + elementoContenido.FindElement(By.CssSelector("td")).Text;
                            }

                        }
                        if (elementoContenido.Text.Contains("Correo Electrónico") && !elementoContenido.Text.Contains("Correo Electrónico Reportado por la DIAN"))
                        {
                            if (!elementoContenido.FindElement(By.CssSelector("td")).Text.Equals(String.Empty) && !email.Contains(elementoContenido.FindElement(By.CssSelector("td")).Text))
                            {
                                todosVacios = false;
                                email += ", " + elementoContenido.FindElement(By.CssSelector("td")).Text;
                            }
                        }
                        if (elementoContenido.Text.Contains("Fax"))
                        {
                            todosVacios = false;
                            fax = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Dirección Comercial"))
                        {
                            todosVacios = false;
                            direccion = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Departamento") && !elementoContenido.Text.Contains("Departamento de Notificaciones"))
                        {
                            todosVacios = false;
                            departamento = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Código Municipio Comercial"))
                        {
                            todosVacios = false;
                            cp = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                        if (elementoContenido.Text.Contains("Municipio") && !elementoContenido.Text.Contains("Código Municipio Comercial") && !elementoContenido.Text.Contains("Municipio de Notificaciones"))
                        {
                            todosVacios = false;
                            poblacion = elementoContenido.FindElement(By.CssSelector("td")).Text;
                        }
                    }
                }
            }
        }




        public string getCSVParaguay()
        {
            return "0;2;;" + razonSocial + ";" + nombreComercial + ";" + clasificacion + ";" + web + ";" + direccion + ";" + poblacion + ";;;;1;;;" + telefono + ";;" + email + ";PY;" + dameIDDepartamentoParaguay(departamento) + ";;;;;;;;;;;;;;;;";
        }

        public string getCSVACAVE()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" +dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVAV()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoAV(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVAAVOT()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoAAVOT(direccion ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVAEDAV() 
        {
            return !todosVacios? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }
        public string getCSVAPAV()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;"+ (representanteLegal ?? "") +";;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVFETAVE()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }


        public string getCSVSpainDMCS()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVAVIBA()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVAEVAV()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVColombia()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;;;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoColombia(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVChile()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;" + (representanteLegal ?? "") + ";;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoEspana(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }

        public string getCSVHonduras()
        {
            return !todosVacios ? "0;2;" + (NIF ?? "") + ";" + (razonSocial ?? "") + ";" + (nombreComercial ?? "") + ";" + (clasificacion ?? "") + ";" + (web ?? "") + ";" + (direccion ?? "") + ";" + (poblacion ?? "") + ";;;;1;" + (representanteLegal ?? "") + ";;" + (telefono ?? "") + ";" + (fax ?? "") + ";" + (email ?? "") + ";ES;" + dameIDDepartamentoHonduras(departamento ?? "") + ";" + (cp ?? "") + ";;;;;;;;;;;;;;;" : "";
        }
        private string dameIDDepartamentoParaguay(string nombre)
        {
            string nombreTratado = nombre.Trim().ToUpperInvariant();
            int idDepartamento = 0;

            switch (nombreTratado)
            {
                case "ASUNCIÓN": idDepartamento = 1085; break;
                case "CONCEPCIÓN": idDepartamento = 1086; break;
                case "SAN PEDRO": idDepartamento = 1087; break;
                case "CORDILLERA": idDepartamento = 1088; break;
                case "GUAIRÁ": idDepartamento = 1089; break;
                case "CAAGUAZÚ": idDepartamento = 1090; break;
                case "CAAZAPÁ": idDepartamento = 1091; break;
                case "ITAPÚA": idDepartamento = 1092; break;
                case "MISIONES": idDepartamento = 1093; break;
                case "PARAGUARÍ": idDepartamento = 1094; break;
                case "ALTO PARANÁ": idDepartamento = 1095; break;
                case "CENTRAL": idDepartamento = 1096; break;
                case "ÑEEMBUCÚ": idDepartamento = 1097; break;
                case "AMAMBAY": idDepartamento = 1098; break;
                case "CANINDEYÚ": idDepartamento = 1099; break;
                case "PRESIDENTE HAYES": idDepartamento = 1100; break;
                case "BOQUERÓN": idDepartamento = 1101; break;
                case "ALTO PARAGUAY": idDepartamento = 1102; break;
            }

            return (idDepartamento == 0)? nombre: "" + idDepartamento;
        }

        private string dameIDDepartamentoEspana(string nombre)
        {
            string nombreTratado = nombre.Trim().ToUpperInvariant();
            int idDepartamento = 0;

            switch (nombreTratado)
            {
                case "ALAVA": idDepartamento = 242; break;
                case "VITORIA": idDepartamento = 242; break;
                case "ALBACETE": idDepartamento = 243; break;
                case "ALICANTE": idDepartamento = 244; break;
                case "ALMERIA": idDepartamento = 245; break;
                case "AVILA": idDepartamento = 246; break;
                case "BADAJOZ": idDepartamento = 247; break;
                case "ISLAS BALEARES": idDepartamento = 248; break;
                case "BALEARES": idDepartamento = 248; break;
                case "MALLORCA": idDepartamento = 248; break;
                case "BARCELONA": idDepartamento = 249; break;
                case "BCN": idDepartamento = 249; break;
                case "BARCELONAº": idDepartamento = 249; break;
                case "(BARCELONA)": idDepartamento = 249; break;
                case "BARCELONADA": idDepartamento = 249; break;
                case "BARCELON": idDepartamento = 249; break;
                case "BURGOS": idDepartamento = 250; break;
                case "CACERES": idDepartamento = 251; break;
                case "CADIZ": idDepartamento = 252; break;
                case "CASTELLON": idDepartamento = 253; break;
                case "CIUDAD REAL": idDepartamento = 254; break;
                case "CORDOBA": idDepartamento = 255; break;
                case "LA CORUÑA": idDepartamento = 256; break;
                case "CUENCA": idDepartamento = 257; break;
                case "GERONA": idDepartamento = 258; break;
                case "GIRONA": idDepartamento = 258; break;
                case "GRANADA": idDepartamento = 259; break;
                case "GUADALAJARA": idDepartamento = 260; break;
                case "GUIPUZCOA": idDepartamento = 261; break;
                case "HUELVA": idDepartamento = 262; break;
                case "HUESCA": idDepartamento = 263; break;
                case "JAEN": idDepartamento = 264; break;
                case "LEON": idDepartamento = 265; break;
                case "LERIDA": idDepartamento = 266; break;
                case "LLEIDA": idDepartamento = 266; break;
                case "LA RIOJA": idDepartamento = 267; break;
                case "LUGO": idDepartamento = 268; break;
                case "MADRID": idDepartamento = 269; break;
                case "MALAGA": idDepartamento = 270; break;                                  
                case "MÁLAGA": idDepartamento = 270; break;                                  
                case "Malaga": idDepartamento = 270; break;                                  
                case "Málaga": idDepartamento = 270; break;                                  
                case "MURCIA": idDepartamento = 271; break;
                case "NAVARRA": idDepartamento = 272; break;
                case "ORENSE": idDepartamento = 273; break;
                case "ASTURIAS": idDepartamento = 274; break;
                case "GIJON": idDepartamento = 274; break;
                case "PALENCIA": idDepartamento = 275; break;
                case "LAS PALMAS": idDepartamento = 276; break;
                case "PONTEVEDRA": idDepartamento = 277; break;
                case "SALAMANCA": idDepartamento = 278; break;
                case "ARONA": idDepartamento = 279; break;                                  
                case "PLAYA DE LAS AMERICAS": idDepartamento = 279; break;                 
                case "Puerto de la Cruz": idDepartamento = 279; break;                     
                case "PUERTO DE LA CRUZ": idDepartamento = 279; break;                    
                case "STA. CRUZ DE TENERIFE": idDepartamento = 279; break;
                case "SANTA CURZ DE TENERIFE": idDepartamento = 279; break;             
                case "SANTA CRUZ DE TENERIFE": idDepartamento = 279; break;                
                case "SAN MIGUEL DE ABONA": idDepartamento = 279; break;                  
                case "GUIA DE ISORA": idDepartamento = 279; break;                         
                case "LAS GALLETAS. ARONA": idDepartamento = 279; break;               
                case "ADEJE": idDepartamento = 279; break;                               
                case "SAN SEBASTIAN DE LA GOMERA": idDepartamento = 279; break;          
                case "SANTA CRUZ DE LA PALMA": idDepartamento = 279; break;                 
                case "LOS LLANOS DE ARIDANE (LA PALMA)": idDepartamento = 279; break;      
                case "VALVERDE (EL HIERRO)": idDepartamento = 279; break;                 
                case "FRONTERA (EL HIERRO)": idDepartamento = 279; break;                 
                case "LA LAGUNA": idDepartamento = 279; break;                             
                case "PLAYA DE LAS AMERICAS. ARONA": idDepartamento = 279; break;           
                case "LOS CRISTIANOS. ARONA": idDepartamento = 279; break;                  
                case "LOS CRISTIANOS": idDepartamento = 279; break;                         
                case "TENERIFE": idDepartamento = 279; break;
                case "CANTABRIA": idDepartamento = 280; break;
                case "SANTANDER": idDepartamento = 280; break;
                case "SEGOVIA": idDepartamento = 281; break;
                case "SEVILLA": idDepartamento = 282; break;
                case "SORIA": idDepartamento = 283; break;
                case "TARRAGONA": idDepartamento = 284; break;
                case "TERUEL": idDepartamento = 285; break;
                case "TOLEDO": idDepartamento = 286; break;
                case "VALENCIA": idDepartamento = 287; break;
                case "VALLADOLID": idDepartamento = 288; break;
                case "VIZCAYA": idDepartamento = 289; break;
                case "BILBAO": idDepartamento = 289; break;
                case "ZAMORA": idDepartamento = 290; break;
                case "ZARAGOZA": idDepartamento = 291; break;
                case "CEUTA": idDepartamento = 292; break;
                case "MELILLA": idDepartamento = 293; break;
            }

            return (idDepartamento == 0) ? nombre : "" + idDepartamento;
        }

        private string dameIDDepartamentoColombia(string nombre)
        {
            string nombreTratado = nombre.Trim().ToLowerInvariant();
            int idDepartamento = 0;

            switch (nombreTratado)
            {
					case "antioquia": idDepartamento = 767; break;
					case "bolívar": idDepartamento = 768; break;
					case "boyacá": idDepartamento = 769; break;
					case "caldas": idDepartamento = 770; break;
					case "cauca": idDepartamento = 771; break;
					case "cundinamarca": idDepartamento = 772; break;
					case "huila": idDepartamento = 773; break;
					case "la guajira": idDepartamento = 774; break;
					case "meta": idDepartamento = 775; break;
					case "nariño": idDepartamento = 776; break;
					case "norte de santander": idDepartamento = 777; break;
					case "santander": idDepartamento = 778; break;
					case "sucre": idDepartamento = 779; break;
					case "tolima": idDepartamento = 780; break;
					case "valle del cauca": idDepartamento = 781; break;
					case "risalda": idDepartamento = 782; break;
					case "atlántico": idDepartamento = 783; break;
					case "córdoba": idDepartamento = 784; break;
					case "san andrés, providencia y santa catalina": idDepartamento = 785; break;
					case "arauca": idDepartamento = 786; break;
					case "casanare": idDepartamento = 787; break;
					case "amazonas": idDepartamento = 788; break;
					case "caquetá": idDepartamento = 789; break;
					case "chocó": idDepartamento = 790; break;
					case "guainía": idDepartamento = 791; break;
					case "guaviare": idDepartamento = 792; break;
					case "putumayo": idDepartamento = 793; break;
					case "quindío": idDepartamento = 794; break;
					case "vaupés": idDepartamento = 795; break;
					case "bogotá": idDepartamento = 796; break;
					case "vichada": idDepartamento = 797; break;
					case "cesar": idDepartamento = 798; break;
					case "magdalena": idDepartamento = 799; break;

			}

            return (idDepartamento == 0) ? nombre : "" + idDepartamento;
        }


        private string dameIDDepartamentoHonduras(string nombre)
        {
            string nombreTratado = nombre.Trim().ToLowerInvariant();
            int idDepartamento = 0;

            switch (nombreTratado)
            {
					case "atlántida": idDepartamento = 800; break;
                    case "atlantida": idDepartamento = 800; break;
					case "choluteca": idDepartamento = 801; break;
					case "colón": idDepartamento = 802; break;
					case "comayagua": idDepartamento = 803; break;
					case "copán": idDepartamento = 804; break;
					case "copán ruinas y occidente": idDepartamento = 804; break;
					case "cortés": idDepartamento = 805; break;
                    case "san pedro sula": idDepartamento = 805; break;
					case "el paraíso": idDepartamento = 806; break;
					case "francisco morazán": idDepartamento = 807; break;
					case "gracias a dios": idDepartamento = 808; break;
					case "intibucá": idDepartamento = 809; break;
					case "islas de la bahía": idDepartamento = 810; break;
					case "la paz": idDepartamento = 811; break;
					case "lempira": idDepartamento = 812; break;
					case "ocotepeque": idDepartamento = 813; break;
					case "olancho": idDepartamento = 814; break;
					case "santa bárbara": idDepartamento = 815; break;
					case "valle": idDepartamento = 816; break;
					case "yoro": idDepartamento = 817; break;
					case "distrito central": idDepartamento = 818; break;
					case "tegucigalpa": idDepartamento = 818; break;
                    

			}

            return (idDepartamento == 0) ? nombre : "" + idDepartamento;
        }

        private string dameIDDepartamentoAV(string nombre)
        {
            string nombreTratado = nombre.Trim().ToUpperInvariant();
            int idDepartamento = 0;

            switch (nombreTratado)
            {
                case "GRAN CANARIA": idDepartamento = 276; break;
                case "LANZAROTE": idDepartamento = 276; break;
                case "FUERTEVENTURA": idDepartamento = 276; break;

                case "TENERIFE": idDepartamento = 279; break;
                case "EL HIERRO": idDepartamento = 279; break;
                case "GOMERA": idDepartamento = 279; break;
                case "LA PALMA": idDepartamento = 279; break;
            }

            return (idDepartamento == 0) ? nombre : "" + idDepartamento;
        }

        private string dameIDDepartamentoAAVOT(string direccion)
        {
            string idDepartamento = String.Empty;
            if (null != direccion)
            {
                string[] elementosDireccion = direccion.Split(',');

                int contadorElementos = elementosDireccion.Length -1;
                while (0 < contadorElementos && String.IsNullOrEmpty(idDepartamento)){
                    string departamento = dameIDDepartamentoEspana(elementosDireccion[contadorElementos]);
                    if (departamento != elementosDireccion[contadorElementos]){
                        idDepartamento = departamento;
                    }
                    contadorElementos--;
                }
            }
            return idDepartamento;
        }
    }
}
