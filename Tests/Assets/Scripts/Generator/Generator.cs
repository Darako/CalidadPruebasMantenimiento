using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class Generator : MonoBehaviour {

    private List<Punto> pointList;
    private List<FiguraGeometrica> squareList;
    private List<FiguraGeometrica> circleList;
    
    public InputField inputX;
    public InputField inputY;
    public InputField inputSquareArea;
    public InputField inputCirclRadio;

    public Dropdown dropdownPointsSquare;
    public Dropdown dropdownPointsCircle;
    public Dropdown dropdownSquare;
    public Dropdown dropdownCircle;
    public Dropdown dropdownSquareColor;
    public Dropdown dropdownCircleColor;

    public GameObject drawer;
    public GameObject square;
    public GameObject circle;
    private GameObject squareInstance;
    private GameObject circleInstance;

    public Material red;
    public Material green;
    public Material blue;
    public Material black;
    public Material white;

    public void Start() {
        this.pointList = new List<Punto>();
        this.squareList = new List<FiguraGeometrica>();
        this.circleList = new List<FiguraGeometrica>();
        dropdownPointsSquare.GetComponent<Dropdown>().ClearOptions();
        dropdownPointsCircle.GetComponent<Dropdown>().ClearOptions();
        dropdownSquare.GetComponent<Dropdown>().ClearOptions();
        dropdownCircle.GetComponent<Dropdown>().ClearOptions();
        dropdownSquareColor.GetComponent<Dropdown>().ClearOptions();
        dropdownCircleColor.GetComponent<Dropdown>().ClearOptions();
        string[] colors = { "Rojo", "Verde", "Azul", "Blanco", "Negro" };

        Dropdown.OptionData dropdownPointList = new Dropdown.OptionData();
        foreach (string color in colors) {
            dropdownPointList = new Dropdown.OptionData(color);
            dropdownSquareColor.GetComponent<Dropdown>().options.Add(dropdownPointList);
            dropdownSquareColor.GetComponent<Dropdown>().RefreshShownValue();
            dropdownCircleColor.GetComponent<Dropdown>().options.Add(dropdownPointList);
            dropdownCircleColor.GetComponent<Dropdown>().RefreshShownValue();
        }
        Analytics.SetUserGender(Gender.Unknown);
        Analytics.SetUserBirthYear(1987);
    }



    public void ChangeDropdowns() {

        dropdownPointsSquare.GetComponent<Dropdown>().ClearOptions();
        dropdownPointsCircle.GetComponent<Dropdown>().ClearOptions();
        dropdownSquare.GetComponent<Dropdown>().ClearOptions();
        dropdownCircle.GetComponent<Dropdown>().ClearOptions();
        //Refresh Point Lists
        Dropdown.OptionData dropdownPointList = new Dropdown.OptionData();
        if (pointList.Count > 0) {
            dropdownPointsSquare.GetComponent<Dropdown>().value = 0;
            dropdownPointsCircle.GetComponent<Dropdown>().value = 0;
            foreach (Punto point in pointList) {
                dropdownPointList = new Dropdown.OptionData(point.ToString());
                dropdownPointsSquare.GetComponent<Dropdown>().options.Add(dropdownPointList);
                dropdownPointsSquare.GetComponent<Dropdown>().RefreshShownValue();
                dropdownPointsCircle.GetComponent<Dropdown>().options.Add(dropdownPointList);
                dropdownPointsCircle.GetComponent<Dropdown>().RefreshShownValue();
            }
        }
        //Refresh Square Lists
        Dropdown.OptionData dropdownSquareList = new Dropdown.OptionData();
        if (squareList.Count > 0) {
            dropdownSquare.GetComponent<Dropdown>().value = 0;
            foreach (FiguraGeometrica square in squareList) {
                dropdownSquareList = new Dropdown.OptionData(square.Dibujar());
                dropdownSquare.GetComponent<Dropdown>().options.Add(dropdownSquareList);
                dropdownSquare.GetComponent<Dropdown>().RefreshShownValue();
            }
        }
        //Refresh Circle Lists
        Dropdown.OptionData dropdownCircleList = new Dropdown.OptionData();
        if (circleList.Count > 0) {
            dropdownCircle.GetComponent<Dropdown>().value = 0;
            foreach (FiguraGeometrica circle in circleList) {
                dropdownCircleList = new Dropdown.OptionData(circle.Dibujar());
                dropdownCircle.GetComponent<Dropdown>().options.Add(dropdownCircleList);
                dropdownCircle.GetComponent<Dropdown>().RefreshShownValue();
            }
        }    
    }  

    public void CreatePoint() {
        if (inputX.transform.FindChild("Text").GetComponent<Text>().text != "" && inputY.transform.FindChild("Text").GetComponent<Text>().text != "") {
            int x = int.Parse(inputX.transform.FindChild("Text").GetComponent<Text>().text);
            int y = int.Parse(inputY.transform.FindChild("Text").GetComponent<Text>().text);
            Punto point = new Punto(x, y);
            pointList.Add(point);
            ChangeDropdowns();
        }
        //Send Custom Event
        Dictionary<string, object> pointDictionary = new Dictionary<string, object>();
        int index = 0;
        foreach (Punto pointListed in pointList) {
            pointDictionary.Add(index.ToString(), pointListed.ToString());
            index++;
        }
        Analytics.CustomEvent("List Points", pointDictionary);
    }   

    public void CreateSquare() {
        if (dropdownPointsSquare.GetComponent<Dropdown>().value != null && 
            inputSquareArea.transform.FindChild("Text").GetComponent<Text>().text != "" &&
            dropdownSquareColor.GetComponent<Dropdown>().value != null) {
            int pointPosition = dropdownPointsSquare.GetComponent<Dropdown>().value;
            float side = float.Parse(inputSquareArea.transform.FindChild("Text").GetComponent<Text>().text);
            string color = ReturnColor(dropdownSquareColor.GetComponent<Dropdown>().value);
            Cuadrado square = new Cuadrado(pointList.ToArray()[pointPosition], color, side);
            squareList.Add(square);
            ChangeDropdowns();
        }
        //Send Custom CustomEvent
        Dictionary<string, object> squareDictionary = new Dictionary<string, object>();
        int index = 0;
        foreach (FiguraGeometrica squareListed in squareList) {
            squareDictionary.Add(index.ToString(), squareListed.Dibujar());
            index++;
        }
        Analytics.CustomEvent("List Squares", squareDictionary);
    }
    
    public void CreateCircle() {
        if (dropdownPointsCircle.GetComponent<Dropdown>().value != null &&
            inputCirclRadio.transform.FindChild("Text").GetComponent<Text>().text != "" &&
            dropdownPointsCircle.GetComponent<Dropdown>().value != null) {
            int pointPosition = dropdownPointsCircle.GetComponent<Dropdown>().value;
            float radio = float.Parse(inputCirclRadio.transform.FindChild("Text").GetComponent<Text>().text);
            string color = ReturnColor(dropdownCircleColor.GetComponent<Dropdown>().value);
            Circulo circle = new Circulo(pointList.ToArray()[pointPosition], color, radio);
            circleList.Add(circle);
            ChangeDropdowns();
        }
        //Send Custom Event
        Dictionary<string, object> circleDictionary = new Dictionary<string, object>();
        int index = 0;
        foreach (FiguraGeometrica circleListed in circleList) {
            circleDictionary.Add(index.ToString(), circleListed.Dibujar());
            index++;
        }
        Analytics.CustomEvent("List Circles", circleDictionary);
    }

    public void DrawSquare() {
        if(squareInstance != null) {
            Destroy(squareInstance);
        }
        if (circleInstance != null) {
            Destroy(circleInstance);
        }
        int squarePosition = dropdownSquare.GetComponent<Dropdown>().value;
        squareInstance = Instantiate<GameObject>(square) as GameObject;
        squareInstance.transform.parent = drawer.transform;
        float x = (float)squareList.ToArray()[squarePosition].Posicion.X * 0.5f / 2f;
        float y = (float)squareList.ToArray()[squarePosition].Posicion.Y * 0.5f / 2f;
        squareInstance.transform.localPosition = new Vector3(x, y, 0f);
        string color = squareList.ToArray()[squarePosition].Color;
        ChangeColor(color, squareInstance);
        float scale = Mathf.Sqrt(squareList.ToArray()[squarePosition].Area());
        ChangeScale(scale, squareInstance);
        //Send Custom Event   
        Analytics.CustomEvent("Draw Square", new Dictionary<string, object> {
            {"Drawing square", squareList.ToArray()[squarePosition].Dibujar() }
        });
        //Send Monetization Event
        Analytics.Transaction(squareList.ToArray()[squarePosition].Dibujar(), (decimal) scale * 2, "€", null, "Darako");
    }

    public void DrawCircle() {
        if (circleInstance != null) {
            Destroy(circleInstance);
        }
        if (squareInstance != null) {
            Destroy(squareInstance);
        }
        int circlePosition = dropdownSquare.GetComponent<Dropdown>().value;
        circleInstance = Instantiate<GameObject>(circle) as GameObject;
        circleInstance.transform.parent = drawer.transform;
        float x = (float)circleList.ToArray()[circlePosition].Posicion.X * 0.5f / 2f;
        float y = (float)circleList.ToArray()[circlePosition].Posicion.Y * 0.5f / 2f;
        circleInstance.transform.localPosition = new Vector3(x, y, 0f);
        string color = circleList.ToArray()[circlePosition].Color;
        ChangeColor(color, circleInstance);
        float scale = Mathf.Sqrt(circleList.ToArray()[circlePosition].Area());
        ChangeScale(scale, circleInstance);
        //Send Event   
        Analytics.CustomEvent("Draw Circle", new Dictionary<string, object> {
            {"Drawing circle", circleList.ToArray()[circlePosition].Dibujar() }
        });
        //Send Monetization Event
        Analytics.Transaction(circleList.ToArray()[circlePosition].Dibujar(), (decimal)scale * 2, "€", null, "Darako");
    }

    public string ReturnColor(int i) {
        string color = "";
        switch (i) {
            case 0: { color = "Rojo"; break; }
            case 1: { color = "Verde"; break; }
            case 2: { color = "Azul"; break; }
            case 3: { color = "Blanco"; break; }
            case 4: { color = "Negro"; break; }
        }
        return color;
    }

    public void ChangeColor(string color, GameObject figura) {
        Material[] materials;
        switch (color) {
            case "Rojo": {
                    figura.GetComponent<MeshRenderer>().material = Instantiate<Material>(red);
                    break;
                }
            case "Verde": {
                    figura.GetComponent<MeshRenderer>().material = Instantiate<Material>(green);
                    break;
                }
            case "Azul": {
                    figura.GetComponent<MeshRenderer>().material = Instantiate<Material>(blue);
                    break;
                }
            case "Blanco": {
                    figura.GetComponent<MeshRenderer>().material = Instantiate<Material>(white);
                    break;
                }
            case "Negro": {
                    figura.GetComponent<MeshRenderer>().material = Instantiate<Material>(black);
                    break;
                }
        }
    }

    public void ChangeScale(float scale, GameObject figura) {
        figura.transform.localScale *= scale / 2;
    }

    

}
