using UnityEngine;
using System.Collections;

public class Punto : MonoBehaviour {

    private int x;
    private int y;

    public Punto(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public int X {
        get { return x; }
        set { this.x = value; }
    }

    public int Y {
        get { return y; }
        set { this.y = value; }
    }

    override
    public string ToString() {
        return "(X: " + this.x + ", Y: " + this.y+")";
    }
}
