using UnityEngine;
using System.Collections;

public class Circulo : FiguraGeometrica {

    private float radio;

    public Circulo(Punto posicion, string color, float radio) : base(posicion, color) {
        this.radio = radio;
    }

    public float Radio {
        get { return radio; }
        set { this.radio = value; }
    }

    override    
    public float Area() {
        return 3.14f * Mathf.Pow(radio, 2.0f);
    }

    override
    public string Dibujar() {
        return "C: " + base.Color + ", R: " + this.radio + ", " + this.Posicion.ToString();
    }
}
