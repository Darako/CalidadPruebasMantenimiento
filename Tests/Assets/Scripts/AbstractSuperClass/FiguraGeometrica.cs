using UnityEngine;
using System.Collections;

public abstract class FiguraGeometrica {

    private Punto posicion;

    private string color;

    public FiguraGeometrica(Punto posicion, string color) {
        this.posicion = posicion;
        this.color = color;
    }

    public void Mover(Punto nuevaPos) {
        posicion = nuevaPos;
        Dibujar();
    }

    public string Color {
        get { return color; }
        set { this.color = value; }
    }
    
    public Punto Posicion {
        get {
            return posicion;
        }
    }    

    public abstract string Dibujar();
  
    public abstract float Area();
}
