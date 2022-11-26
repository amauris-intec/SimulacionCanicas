using SimulacionCanicas;
    
string[] fondo = @"
                   A                B                   
                |    |            |    |                
                |    |            |    |                
                |    |            |    |                
                |    |            |    |                
               /      \          /      \               
              /        \        /        \              
             /          \      /          \             
            /     /\     \    /     /\     \            
           /     /  \     \  /     /  \     \           
          /     /    \     \/     /    \     \          
         /     /      \          /      \     \         
        /     /        \        /        \     \        
        \     \        /        \        /     /        
         \     \      /          \      /     /         
          \     \    /     /\     \    /     /          
           \     \  /     /  \     \  /     /           
            \     \/     /    \     \/     /            
             \          /      \          /             
              \        /        \        /              
               \      /          \      /               
                |    |            |    |                
                |    |            |    |                
                |    |            |    |                
                |    |            |    |                
                |    |            |    |                
                |    |            |    |                
                   C                D                   ".Split('\n');

Salida c = new Salida("C");
Salida d = new Salida("D", aceptacion: true);

Palanca x2 = new Palanca("x2", coors: new Coors(y: 13, x: 26), izquierda: c, derecha: d, posicionInicial: Posicion.izquierda);
Palanca x1 = new Palanca("x1", coors: new Coors(y:  6, x: 17), izquierda: c, derecha: x2, posicionInicial: Posicion.izquierda);
Palanca x3 = new Palanca("x3", coors: new Coors(y:  6, x: 35), izquierda: x2, derecha: d, posicionInicial: Posicion.izquierda);

Entrada a = new Entrada('A', x1);
Entrada b = new Entrada('B', x3);

List<string> cadenasAceptadas = new List<string>();

string cadena = "";
bool aceptada = false;
string msg = "";
bool repetida = false;
bool agregada = false;

try
{
    do
    {
        fondo = x1.Dibujar(fondo);
        fondo = x2.Dibujar(fondo);
        fondo = x3.Dibujar(fondo);

        Console.Clear();

        if (aceptada)
            Console.ForegroundColor = repetida ? ConsoleColor.Yellow : ConsoleColor.Green;
        else
            Console.ResetColor();

        foreach (string linea in fondo)
            Console.WriteLine(linea);

        Console.WriteLine(msg);


        ConsoleKeyInfo key = Console.ReadKey(true);

        msg = aceptada && agregada ? "" : msg;
        agregada = false;

        if (key.KeyChar.ToString().ToLower() == "a")
        {
            cadena += "A";
            msg += "A";
            aceptada = a.Lanzar();
            repetida = aceptada && cadenasAceptadas.Find(x => x.Equals(cadena)) != null;
        }
        else if (key.KeyChar.ToString().ToLower() == "b")
        {
            cadena += "B";
            msg += "B";
            aceptada = b.Lanzar();
            repetida = aceptada && cadenasAceptadas.Find(x => x.Equals(cadena)) != null;
        }
        else if (key.Key == ConsoleKey.Escape)
        {
            Console.WriteLine($"Total de Cadenas encontradas: \n\t{string.Join("\n\t", cadenasAceptadas)}");
            break;
        }
        else if (key.Key == ConsoleKey.Enter && aceptada)
        {
            agregada = true;
            Console.WriteLine($"Total de Cadenas encontradas: \n\t{string.Join("\n\t", cadenasAceptadas)}");

            if (cadenasAceptadas.Find(x => x.Equals(cadena)) != null)
            { 
                msg = $"Ya se había descubierto esta cadena: {cadena}";
            }
            else
            {
                msg = $"Nueva cadena descubierta!: {cadena}";
                cadenasAceptadas.Add(cadena);
            }

            cadena = "";
            x1.Reset();
            x2.Reset();
            x3.Reset();

        }

    }
    while (true);

}
finally { Console.ResetColor(); };
Console.WriteLine("Press any key to close...");
Console.ReadKey(true);

