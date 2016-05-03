using UnityEngine;
using System.Collections;

public class Cuadrado : FiguraGeometrica {

    private float lado;

    public Cuadrado(Punto posicion, string color, float lado) : base(posicion, color) {
        this.lado = lado;
    }

   public float Lado {
        get { return lado;  }
        set { this.lado = value; }
    }

    override
    public float Area() {
        return lado * lado;
    }

    override
    public string Dibujar() {
        return "C: " + base.Color + ", L: " + this.lado +", "+ this.Posicion.ToString();
    }
}
