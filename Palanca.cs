using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulacionCanicas
{

    internal interface IImpactable
    {
        public bool Impactar();
    }

    internal enum Posicion
    {
        izquierda,
        derecha
    }

    internal enum MomentoCambio
    {
        antesDePasar,
        luegoDePasar,
        nunca
    }

    internal struct Coors
    {
        public Coors(int y, int x) => (this.Y, this.X) = (y, x);
        public int Y { get; set; }
        public int X { get; set; }
    }

    internal class Entrada
    {
        char input;
        IImpactable destino;

        public Entrada(char input, IImpactable destino) => (this.input, this.destino) = (input, destino);

        public bool Lanzar() { return destino.Impactar(); }
    }

    internal class Salida : IImpactable
    {
        string id;
        bool aceptacion;

        public Salida(string id, bool aceptacion = false) => (this.id, this.aceptacion) = (id, aceptacion);

        public bool Impactar() { return aceptacion; }
    }

    internal class Palanca : IImpactable
    {
        Coors coors;

        string[] imagenIzquierda =  {@"   /",
                                     @"  o ",
                                     @" /  "};

        string[] imagenDerecha = {@"\   ",
                                  @" o  ",
                                  @"  \ "};


        string id;
        MomentoCambio momentoCambio;
        IImpactable? izquierda;
        IImpactable? derecha;
        Posicion posicion;
        Posicion posicionInicial;

        public string Id { get => id; }

        public Palanca(string id, Coors coors, IImpactable? izquierda, IImpactable? derecha, Posicion posicionInicial, MomentoCambio momentoCambio = MomentoCambio.luegoDePasar)
        {
            this.id = id;
            this.coors = coors;
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.posicion = this.posicionInicial = posicionInicial;
            this.momentoCambio = momentoCambio;
        }

        private string[] ObtenerImagen() => posicion == Posicion.izquierda ? imagenIzquierda : imagenDerecha;

        private IImpactable? ObtenerDestino() => posicion == Posicion.izquierda ? izquierda : derecha;

        private void Cambiar() => posicion = posicion == Posicion.izquierda ? Posicion.derecha : Posicion.izquierda;

        public void Reset() => posicion = posicionInicial;

        public bool Impactar()
        {
            //Console.WriteLine($"{this.id} Impactado!");

            if (momentoCambio == MomentoCambio.antesDePasar)
            {
                Cambiar();
                //Console.WriteLine($"{this.id} cambiado hacia la {posicion.ToString()}");
            }
            IImpactable? destino = ObtenerDestino();
            //Console.WriteLine($"Enviando canica hacia la {posicion.ToString()}");

            if (momentoCambio == MomentoCambio.luegoDePasar) 
            {
                Cambiar();
                //Console.WriteLine($"{this.id} cambiado hacia la {posicion.ToString()}");
            }

            if (destino != null)
                return destino.Impactar();
            else
                return false;
            
        }

        public string[] Dibujar(string[] fondo)
        {
            string[] imagen = ObtenerImagen();

            int lineaFondo = coors.Y;
            int lineaImagen = 0;
            for (; lineaImagen < imagen.Length; lineaFondo++, lineaImagen++)
                fondo[lineaFondo] = fondo[lineaFondo].Remove(coors.X, imagen[lineaImagen].Length).Insert(coors.X, imagen[lineaImagen]);
            return fondo;
        }
    }
}
